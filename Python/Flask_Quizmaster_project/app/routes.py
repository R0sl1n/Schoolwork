from flask import render_template, flash, redirect, url_for, request, send_from_directory
from app import app, db
from app.forms import LoginForm, RegistrationForm
from flask_login import current_user, login_user, logout_user, login_required
from werkzeug.urls import url_parse
from app.models import User, Quiz_Question, Quiz_Response, Type, Category
from functools import wraps
from sqlalchemy import text, and_, update
from app.forms import AnswerForm
from sqlalchemy.orm import joinedload
#This function is borrowed from StackOverflow - full link will be supplied.
def admin_required(func):
    """
    Modified login_required decorator to restrict access to admin group.
    """
    @wraps(func)
    def decorated_view(*args, **kwargs):
        if not current_user.isAdmin:
            flash("You don't have permission to access this resource.")
            return redirect(url_for('home'))
        return func(*args, **kwargs)
    return decorated_view

@app.route('/')
@app.route('/home', methods=['GET', 'POST'])
@login_required
def home():
    return render_template('home.html', title='Home')

@app.route('/adminhome', methods=['GET', 'POST'])
@login_required
@admin_required
def adminhome():
    return render_template('adminhome.html', title='Admin Home')

@app.route('/login', methods=['GET', 'POST'])
def login():
    form = LoginForm()
    if form.validate_on_submit():
        user = User.query.filter_by(username=form.username.data).first()
        if user is None or not user.check_password(form.password.data):
            flash('Invalid username or password')
            return redirect(url_for('login'))
        login_user(user, remember=form.remember_me.data)
        if current_user.isAdmin:
            return redirect(url_for('adminhome'))
        return redirect(url_for('home'))
    return render_template('login.html', title='Sign In', form=form)

@app.route('/logout')
def logout():
    logout_user()
    return redirect(url_for('login'))

@app.route('/register', methods=['GET', 'POST'])
def register():
    form = RegistrationForm()
    if form.validate_on_submit():
        user = User(username=form.username.data, firstname=form.givenName.data, lastname=form.lastName.data)
        user.set_password(form.password.data)
        db.session.add(user)
        db.session.commit()
        flash('Congratulations, you are now a registered user!')
        return redirect(url_for('login'))
    return render_template('register.html', title='Register', form=form)

@app.route('/profile')
@login_required
def profile():
    user = current_user
    return render_template('profile.html', user=user)

@app.route('/quizEdit', methods=["GET", "POST"])
@login_required
@admin_required
def quizEdit():
    types = Type.query.all()
    categories = Category.query.all()
    questions = Quiz_Question.query.filter_by(active=1).all()

    if request.method == "POST":
        selected_question_id = request.form.get('id')
        if selected_question_id:
            selected_question = Quiz_Question.query.get(selected_question_id)
            if request.form.get('updateQst'):
                form = AnswerForm()  # Create an instance of the AnswerForm
                if form.validate_on_submit():
                    selected_question.definition = form.definition.data
                    selected_question.alt1 = form.alt1.data
                    selected_question.alt2 = form.alt2.data
                    selected_question.alt3 = form.alt3.data
                    selected_question.alt4 = form.alt4.data
                    selected_question.alt5 = form.alt5.data
                    db.session.commit()  # Commit the changes to the database
                    flash('Question updated successfully')
                else:
                    flash('Failed to update question')
            else:
                flash('Invalid form submission')
        else:
            flash('Selected question not found')

    selected_question = None

    return render_template(
        'quiz_edit.html',
        questions=questions,
        selected_question=selected_question,
        types=types,
        categories=categories
    )

@app.route('/addQuestion', methods=["POST"])
@login_required
@admin_required
def addQuestion():
    form = AnswerForm()  # Create an instance of the AnswerForm

    if form.validate_on_submit():
        definition = form.definition.data
        type_id = form.type_id.data
        category_id = form.category_id.data
        alt1 = form.alt1.data
        alt2 = form.alt2.data
        alt3 = form.alt3.data
        alt4 = form.alt4.data
        alt5 = form.alt5.data

        new_question = Quiz_Question(
            definition=definition,
            type_id=type_id,
            category_id=category_id,
            alt1=alt1,
            alt2=alt2,
            alt3=alt3,
            alt4=alt4,
            alt5=alt5
        )

        try:
            db.session.add(new_question)
            db.session.commit()
            flash('Question has been added!', 'success')

            id_qst = new_question.id
            id_user = current_user.id
            quiz_answer = ""  # Set the initial quiz answer to an empty string or provide a default value
            quiz_response = Quiz_Response(id_qst=id_qst, id_user=id_user, quiz_answer=quiz_answer)
            db.session.add(quiz_response)
            db.session.commit()

            return redirect(url_for('quizEdit'))

        except Exception as e:
            db.session.rollback()  # Rollback the database transaction in case of an exception
            flash('Failed to add question')
            

    flash('Invalid form submission')
    return redirect(url_for('quizEdit'))


@app.route('/updateQst/<int:question_id>', methods=["GET", "POST"])
@login_required
@admin_required
def updateQst(question_id):
    question = Quiz_Question.query.get(question_id)
    if question is None:
        flash("No question")

    form = AnswerForm(obj=question)

    # Get the available types and categories
    types = Type.query.all()
    categories = Category.query.all()

    # Set the choices for the type_id and category_id fields
    form.type_id.choices = [(t.id, t.category) for t in types]
    form.category_id.choices = [(c.id, c.definition) for c in categories]

    if form.validate_on_submit():
        print("Form validation successful")
        form.populate_obj(question)  # Update the question object with form data
        question.type = Type.query.get(form.type_id.data)  # Update the type attribute
        question.category = Category.query.get(form.category_id.data)  # Update the category attribute
        db.session.commit()
        flash('Question updated successfully')
        return redirect(url_for('quizEdit'))
    else:
        print("Form validation failed")
        print(form.errors)

    return render_template(
        'quiz_edit.html',
        form=form,
        selected_question=question,
        types=types,
        categories=categories
    )


@app.route('/deleteQstConfirm', methods=["GET", "POST"])
@login_required
@admin_required
def delete_qst_confirm():
    ids = request.form['id']
    if not ids:
        flash("No id.")
    else:
        question = Quiz_Question.query.filter_by(id=ids).first()
        if question is None:
            flash("No question")
        else:
            return render_template('delete_question.html', question=question)


@app.route('/delete_qst', methods=["POST"])
@login_required
@admin_required
def delete_qst():
    ids = request.form['id']
    question = Quiz_Question.query.filter_by(id=ids).first()
    if question:
        question.active = 0
        db.session.commit()
        flash('Question moved to recycle bin')
    return redirect(url_for('quizEdit'))


@app.route('/recyclebin', methods=["GET", "POST"])
@login_required
@admin_required
def recyclebin():
    ids = request.args.get('id')
    if not ids:
        recyclebin = Quiz_Question.query.filter_by(active=0).all()
        return render_template('recyclebin.html', recyclebin=recyclebin)
    else:
        return redirect(url_for('moveBackConfirm', id=ids))


@app.route('/moveFromRecyclebin', methods=["GET", "POST"])
@login_required
@admin_required
def move_from_recyclebin():
    ids = request.args.get('id')
    if not ids:
        return redirect(url_for('quizEdit'))
    else:
        question = Quiz_Question.query.filter_by(id=ids, active=0).first()
        if question:
            question.active = 1
            db.session.commit()
            flash('Question moved back from recycle bin')
        return redirect(url_for('recyclebin'))


@app.route('/moveBackConfirm')
@login_required
@admin_required
def move_back_confirm():
    ids = request.args.get('id')
    if not ids:
        flash("No id.")
    else:
        question = Quiz_Question.query.filter_by(id=ids, active=0).first()
        if question is None:
            flash("No question.")
        else:
            return render_template('move_back_confirm.html', question=question)


@app.route('/recoverQuestion', methods=["POST"])
@login_required
@admin_required
def recover_question():
    id = request.form['id']
    question = Quiz_Question.query.filter_by(id=id, active=0).first()
    if question:
        question.active = 1
        db.session.commit()
        flash('Question moved back from recycle bin')
    return redirect(url_for('recyclebin'))


#Route to answer the Quiz.
@app.route('/quizView', methods=['GET', 'POST'])
@login_required
def quizView():
    form = AnswerForm()
    
    if request.method == 'POST':
        # Process the submitted answer
        current_question_id = form.current_question_id.data
        answer = form.answer.data
        save_answer(current_user.id, current_question_id, answer)
        
        # Get the next question
        next_question_id = get_next_question_id(current_question_id)
        
        if next_question_id:
            # Redirect to the next question
            return redirect(url_for('quizView', current_question_id=next_question_id))
        else:
            # Quiz completed, redirect to the review page
            return redirect(url_for('review'))
    
    else:
        current_question_id = request.args.get('current_question_id')
        
        if current_question_id:
            # Retrieve the current question
            current_question = Quiz_Question.query.get(current_question_id)
        else:
            # No specific question requested, get the first question
            current_question = get_first_question()
        
        return render_template('quiz_answer.html', current_question=current_question, form=form)

# Route for reviewing users Quiz responses.
@app.route('/review')
@login_required
def review():
    quiz_responses = Quiz_Response.query.filter_by(id_user=current_user.id).all()
    return render_template('review_quiz.html', answered_questions=quiz_responses)

def get_first_question():
    return Quiz_Question.query.order_by(Quiz_Question.id).first()

#Functions to get the first, next and last question. 
def get_next_question_id(current_question_id):
    current_question = Quiz_Question.query.get(current_question_id)
    next_question = Quiz_Question.query.filter(Quiz_Question.id > current_question_id, Quiz_Question.active == 1).order_by(Quiz_Question.id).first()
    
    if next_question:
        return next_question.id
    else:
        return None

#Function to save the user response.
def save_answer(user_id, question_id, answer):
    question = Quiz_Question.query.get(question_id)
    user = User.query.get(user_id)
    
    quiz_response = Quiz_Response.query.filter_by(id_qst=question_id, id_user=user_id).first()
    
    if quiz_response:
        # Update existing quiz response
        quiz_response.quiz_answer = answer
    else:
        # Create a new quiz response
        quiz_response = Quiz_Response(id_qst=question_id, id_user=user_id, quiz_answer=answer)
    
    db.session.add(quiz_response)
    db.session.commit()


@app.route('/quiz_users')
@login_required
@admin_required
def quiz_responses_users():
    quiz_responses_users = db.session.query(
        
        User
        
    ).all()
    
    return render_template('quiz_responses_users.html', quiz_responses_users=quiz_responses_users)

@login_required
@admin_required
@app.route('/deleteAllAnswersConfirm')
def deleteAllAnswersConfirm():
    id = request.args.get('id')
    if not id:
        flash("No id")
    else:
        return render_template('answers_del_confirm.html', id=id, deleteConfirmation=True)
        

@login_required
@admin_required
@app.route('/allAnswersDelete', methods=["GET", "POST"])
def allAnswersDelete():
    ids = request.form['id']
    
    
    answers = Quiz_Response.query.filter_by(id_user=ids).all()
    for answer in answers:
            response = Quiz_Response.query.filter_by(id_user=ids)

            save_answer(ids, answer.id_qst, "Your answer is deleted by the administrator")
                
    db.session.commit()
    flash("Answers successfully deleted") 
    return redirect('/answerCheck?id='+str(ids))
        
@login_required
@admin_required
@app.route('/deleteOneResponseConfirm', methods=['GET', 'POST'])
def deleteOneResponseConfirm():
    id = request.form['id']
    id_qst = request.form['id_qst']
    
    if not id or not id_qst:
        flash("No id")
    else:
        response, user, question= db.session.query(
            Quiz_Response,
            User,
            Quiz_Question
        ).join(
            User,
            Quiz_Response.id_user == User.id
        ).join(
            Quiz_Question,
            Quiz_Response.id_qst == Quiz_Question.id
        ).filter(
            Quiz_Response.id_user == id,
            Quiz_Response.id_qst == id_qst
        ).first()
        print(response)
        print(question)
        if response is None or question is None:
            flash("No response")
        else:
            return render_template('del_one_resp_confirm.html', id=id, id_qst=id_qst, response=response, question=question, deleteConfirmation=True)

@login_required
@admin_required   
@app.route('/deleteOneResponse', methods=['POST'])
def deleteOneResponse():
    
    if request.method == 'POST':
        id = request.form['id']
        id_qst = request.form['id_qst']
        save_answer(id, id_qst, "Your answer is deleted by the administrator")
            
        flash("Answer deleted.")        
        return redirect('/answerCheck?id='+str(id))


@login_required
@admin_required
@app.route('/approveQst', methods=["GET", "POST"])
def approveQst():
    
    id = request.form['id']
    answers = Quiz_Response.query.filter_by(id_user=id).all()
    for answer in answers:
            response = Quiz_Response.query.filter_by(id_user=id)
            save_status(id, answer.id_qst, 2)
                
    db.session.commit() 
    return redirect('/answerCheck?id='+str(id))
    

@login_required
@admin_required
@app.route('/answerCheck', methods=["GET", "POST"])
def answerCheck():
    
    if request.method=="GET":
        id_qst = request.args.get('id_qst')
        
        id_user = request.args.get('id')
        
        if not id_qst:
            quiz_responses = db.session.query(
            Quiz_Response,
            User,
            Quiz_Question
            ).join(
            User,
            Quiz_Response.id_user == User.id
            ).join(
            Quiz_Question,
            Quiz_Response.id_qst == Quiz_Question.id
            ).filter(
            Quiz_Response.id_user == id_user 
            ).all()
            
            return render_template('quiz_responses.html',
                               quiz_responses=quiz_responses)
        else:
            quiz_response = db.session.query(
            Quiz_Response,
            User,
            Quiz_Question
            ).join(
            User,
            Quiz_Response.id_user == User.id
            ).join(
            Quiz_Question,
            Quiz_Response.id_qst == Quiz_Question.id
            ).filter(
            Quiz_Response.id_user == id_user,
            Quiz_Response.id_qst == id_qst).all()
            
            return render_template('quiz_responses.html', quiz_response = quiz_response, id_user=id_user, id_qst=id_qst)
    elif request.method=="POST" or request.method=="GET":
        id_user=request.form['id']
        id_qst=request.form['id_qst']
        try:
            quiz_responses = db.session.query(
            Quiz_Response,
            User,
            Quiz_Question
            ).join(
            User,
            Quiz_Response.id_user == User.id
            ).join(
            Quiz_Question,
            Quiz_Response.id_qst == Quiz_Question.id
            ).filter(
            Quiz_Response.id_user == id_user 
            ).all()
            
            return render_template('quiz_responses.html',
                               quiz_responses=quiz_responses) 
        except KeyError:
            quiz_response = db.session.query(
            Quiz_Response,
            User,
            Quiz_Question
            ).join(
            User,
            Quiz_Response.id_user == User.id
            ).join(
            Quiz_Question,
            Quiz_Response.id_qst == Quiz_Question.id
            ).filter(
            Quiz_Response.id_user == id_user,
            Quiz_Response.id_qst == id_qst).all()
            
            return render_template('quiz_responses.html',
                                       quiz_response = quiz_response, id_user=id_user, id_qst=id_qst)
    else:
        return redirect(url_for('answerCheck'))

@login_required
@admin_required
@app.route('/saveCheck', methods=["GET", "POST"])
def saveCheck() :
    user_id = request.form['id']
    qst_id = request.form['id_qst']
    if not qst_id:
        quiz_responses = db.session.query(
        Quiz_Response,
        User,
        Quiz_Question
        ).join(
        User,
        Quiz_Response.id_user == User.id
        ).join(
        Quiz_Question,
        Quiz_Response.id_qst == Quiz_Question.id
        ).filter(
        Quiz_Response.id_user == user_id 
        ).all()
        db.session.commit()   
        return render_template('quiz_responses.html',
                               quiz_responses=quiz_responses)
    else:
        form = AnswerForm()
        if request.method == 'POST':
            # Process the submitted data
            current_question_id = form.current_question_id.data
            quiz_status = request.form["quiz_status"]
            
            
            quiz_comment = request.form["quiz_comment"]
            
            save_status(user_id, qst_id, quiz_status)
            save_comment(user_id, qst_id, quiz_comment)
        
            # Get the next question
            next_question_id = get_next_question_id(qst_id)
            print(user_id, next_question_id )
            if next_question_id:
                # Redirect to the next question
                return redirect(url_for('answerCheck', id=user_id, id_qst=next_question_id))
            else:
                # Quiz completed, redirect to the review page
                return redirect(url_for('answerCheck', id=user_id))
    

#Starts the Quiz. 
@app.route('/start', methods=["GET", "POST"])
@login_required
def start():
    if request.method == "POST":
        answerIds = [a.id_qst for a in Quiz_Response.query.filter_by(id_user=current_user.id).all()]
        if answerIds:
            id = int(answerIds[0])
            return redirect(url_for('quizView', current_question_id=id))
        else:
            first_question_id = Quiz_Question.query.order_by(Quiz_Question.id).first().id
            return redirect(url_for('quizView', current_question_id=first_question_id))
    return redirect(url_for('login'))

# #Submits the Quiz.
@app.route('/submitQuiz', methods=['GET', 'POST'])
@login_required
def submitQuiz():
    form = AnswerForm()
    
    if form.validate_on_submit():
        id_user = current_user.get_id()

        # Update the quiz responses for the current user.
        quiz_questions = Quiz_Question.query.order_by(Quiz_Question.id).all()

        for question in quiz_questions:
            response = Quiz_Response.query.filter_by(id_qst=question.id, id_user=id_user).first()

            if response is None:
                response = Quiz_Response(id_qst=question.id, id_user=id_user, quiz_answer='')
                db.session.add(response)

        db.session.commit()

        # Retrieve the users quiz responses
        result = db.session.query(
            Quiz_Question, Quiz_Response.quiz_answer
        ).join(
            Quiz_Response, Quiz_Question.id == Quiz_Response.id_qst
        ).filter(
            Quiz_Response.id_user == id_user
        ).order_by(Quiz_Question.id).all()

        return render_template('submit_quiz.html', result=result)

    if request.method == 'POST':
        flash('Please fill in all the required fields.')
        
    flash('Quiz submitted successfully. E.T phone home!')
    if current_user.isAdmin:
        return redirect(url_for('adminhome'))
    else:
        return redirect(url_for('home'))

#Handle answers when traversing the Quiz.
@app.route('/handle_answer', methods=['POST'])
@login_required

def handleAnswer():
    selected_options = request.form.getlist('answer')
    id=current_user.id
    id_qst=request.form["id_qst"]
    selected_options.pop(0)
    answer=', '.join([str(elem) for elem in selected_options])
    
    save_answer(id, id_qst, answer)
    next_question_id = get_next_question_id(id_qst)
    if next_question_id!=None:
        return redirect(url_for('quizView', current_question_id=next_question_id))
    else:
        return redirect(url_for('quizView', current_question_id=False))

    
def save_status(id_user, id_question, status):
    question = Quiz_Question.query.get(id_question)
    user = User.query.get(id_user)
    response = Quiz_Response.query.filter_by(id_user=id_user, id_qst=id_question).first()
    if response:
        response.quiz_status = status
        
    else:
        response = Quiz_Response(id_user=id_user, id_qst=id_question, quiz_status=status)
        db.session.add(response)
    db.session.commit()

def save_comment(id_user, id_question, comment):
    question = Quiz_Question.query.get(id_question)
    user = User.query.get(id_user)
    response = Quiz_Response.query.filter_by(id_user=id_user, id_qst=id_question).first()
    if response:
        response.quiz_comment = comment
    else:
        response = Quiz_Response(id_user=id_user, id_qst=id_question, quiz_comment = comment)
        db.session.add(response)
    db.session.commit()

@app.route('/quiz_details/<int:user_id>')
@login_required
def quiz_details(user_id):
    selected_user = User.query.get(user_id)
    if selected_user:
        selected_user_responses = Quiz_Response.query.filter_by(user=selected_user).all()
        return render_template('quiz_details.html', selected_user=selected_user, selected_user_responses=selected_user_responses)
    flash('Selected user not found')
    return redirect(url_for('approved_quizzes'))

@app.route('/approvedQuizzes')
@login_required
def approved_quizzes():
    approved_responses = Quiz_Response.query.filter_by(quiz_status=2).all()
    users = User.query.all()
    return render_template('approved_quizzes.html', approved_responses=approved_responses, users=users)








