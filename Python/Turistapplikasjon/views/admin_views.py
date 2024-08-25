from werkzeug.utils import secure_filename
from werkzeug.security import generate_password_hash
from flask import flash, request, redirect, render_template, url_for, Blueprint, current_app
from flask_login import current_user, login_required
from forms import EditUserForm, RegisterForm
from userregistry import UserReg
from turistdb import TuristDb
import os.path

admin_bp = Blueprint("admin",__name__,template_folder="templates")


@admin_bp.route('/admin/dashboard')
@login_required
def admin_dashboard():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    
    with UserReg() as user_db:
        all_users_data = user_db.fetch_all_users()
        users = []
        for user_data in all_users_data:
            user_record = list(user_data)
            user_record[5] = bool(user_record[5])
            user_record[6] = bool(user_record[6])
            users.append(user_record)

    with TuristDb() as attraction_db:
        attractions = attraction_db.fetch_all_attractions()
        cities = attraction_db.fetch_all_cities()
        categories = attraction_db.get_all_categories()
        quiz_questions = attraction_db.get_all_quiz_questions()

    return render_template('admin_dashboard.html', users=users, attractions=attractions, cities=cities, categories=categories, quiz_questions=quiz_questions)

@admin_bp.route('/admin/dashboard/add_filter',methods=["POST"])
@login_required
def add_filter():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == "POST":
        category = request.form["name"]
        type_id = request.form["categorytype"]
        icon = request.form["icon"]
        with TuristDb() as attraction_db:
            exists = attraction_db.add_filter(category,type_id,icon)
            if exists == -1:
                flash(f"Adding Filter: {category} Failed! It already exists.","error")
            elif exists == 0:
                flash(f"Adding Filter: {category} Failed! Unknown Error","error")
            else:
                flash(f"Adding Filter: {category} was successful.","success")
            return redirect(url_for("admin.admin_dashboard"))
    else:
        return redirect(url_for("miscellaneous.index"))
    
@admin_bp.route('/admin/dashboard/remove_filter',methods=["POST"])
@login_required
def remove_filter():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == "POST":
        category_id = request.args.get('category_id')

        with TuristDb() as attraction_db:
            return_code = attraction_db.remove_filter(category_id)
            if return_code == 1:
                flash("Removing filter was successful.","success")
            else:
                flash("Removing filter failed! Unknown reason","error")
            return redirect(url_for("admin.admin_dashboard"))
    else:
        return redirect(url_for("miscellaneous.index"))
    
@admin_bp.route('/admin/dashboard/edit_filter',methods=["POST"])
@login_required
def edit_filter():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == "POST":
        id = request.form["category_id"]
        category = request.form["name"]
        type_id = request.form["categorytype"]
        icon = request.form["icon"]

        with TuristDb() as attraction_db:
            return_code = attraction_db.edit_filter(id,category,type_id,icon)
            if return_code == 1:
                flash("Editing filter was successful.","success")
            else:
                flash("Editing filter failed! Unknown reason","error")
            return redirect(url_for("admin.admin_dashboard"))
    else:
        return redirect(url_for("miscellaneous.index"))
    


@admin_bp.route('/admin/dashboard/add_city',methods=["POST"])
@login_required
def add_city():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == "POST":
        city_name = request.form['name']
        with TuristDb() as attraction_db:
            error_id = attraction_db.add_city(city_name)
            if error_id == 1:
                flash(f"Adding City: {city_name} was successful","success")
            elif error_id == -1:
                flash(f"Adding City: {city_name} failed! It already exists.","error")
            else:
                flash(f"Adding City: {city_name} failed! Unknown Error!","error")

            return redirect(url_for("admin.admin_dashboard"))
    else:
        return redirect(url_for("miscellaneous.index"))
    
@admin_bp.route('/admin/dashboard/remove_city',methods=["POST"])
@login_required
def remove_city():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == "POST":
        city_id = request.args.get('city_id')
        
        with TuristDb() as attraction_db:
            return_code = attraction_db.remove_city(city_id)
            if return_code == 1:
                flash(f"Removing city was successful.","success")
            else:
                flash(f"Removing city failed! Unknown reason.","error")
        return redirect(url_for("admin.admin_dashboard"))
    

@admin_bp.route('/admin/dashboard/add_question',methods=["POST"])
@login_required
def add_question():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == "POST":
        attraction_id = request.form['attraction']
        level = request.form['level']
        question_text = request.form['question_text']
        answer_1 = request.form['answer_1']
        answer_2 = request.form['answer_2']
        answer_3 = request.form['answer_3']
        answer_4 = request.form['answer_4']
        correct_answer = request.form['correct_answer']
        with TuristDb() as attraction_db:
            return_code = attraction_db.add_question(attraction_id,level,question_text,answer_1,answer_2,answer_3,answer_4,correct_answer)
            if return_code == -1:
                flash(f"Adding question: {question_text} failed! It already exists.","error")
            elif return_code == 0:
                flash(f"Adding question: {question_text} failed! Unknown error.","error")
            else:
                flash(f"Adding question: {question_text} was successful.","success")
        return redirect(url_for("admin.admin_dashboard"))
       
        
    else:
        return redirect(url_for("miscellaneous.index"))

@admin_bp.route('/admin/dashboard/remove_question',methods=["POST"])
@login_required
def remove_question():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == "POST":
        question_id = request.args.get('question_id')

        with TuristDb() as attraction_db:
            return_code = attraction_db.remove_question(question_id)
            if return_code== 1:
                flash("Removing question was successful.","success")
            else:
                flash("Removing question failed! Unknown reason","error")
            return redirect(url_for("admin.admin_dashboard"))
        
@admin_bp.route('/admin/dashboard/edit_question',methods=["POST"])
@login_required
def edit_question():
    if not current_user.is_admin: 
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == "POST":
        id = request.form["question_id"]
        attraction_id = request.form['attraction']
        level = request.form['level']
        question_text = request.form['question_text']
        answer_1 = request.form['answer_1']
        answer_2 = request.form['answer_1']
        answer_3 = request.form['answer_1']
        answer_4 = request.form['answer_1']
        correct_answer = request.form['correct_answer']

        with TuristDb() as attraction_db:
            return_code = attraction_db.edit_question(id,attraction_id,level,question_text,answer_1,answer_2,answer_3,answer_4,correct_answer)
            if return_code == 1:
                flash("Editing question was successful.","success")
            else:
                flash("Editing question failed! Unknown reason.","error")
            return redirect(url_for("admin.admin_dashboard"))
    else:
        return redirect(url_for("miscellaneous.index"))

@admin_bp.route('/admin/dashboard/upload_reward', methods=['POST'])
@login_required
def upload_reward():
    if not current_user.is_admin:
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))

    if 'reward_file' not in request.files:
        flash('No file part in the request', 'error')
        return redirect(url_for('admin.admin_dashboard'))
    
    file = request.files['reward_file']
    
    if file.filename == '':
        flash('No file selected for uploading', 'error')
        return redirect(url_for('admin.admin_dashboard'))
    
    if file and allowed_file(file.filename):
        filename = secure_filename(file.filename)
        file_path = os.path.join(current_app.config['REWARD_FOLDER'], filename)
        file.save(file_path)
        flash('File successfully uploaded', 'success')
        return redirect(url_for('admin.admin_dashboard'))
    else:
        flash('Allowed file types are .png, .jpg, .jpeg, .gif', 'error')
        return redirect(url_for('admin.admin_dashboard'))

def allowed_file(filename):
    return '.' in filename and filename.rsplit('.', 1)[1].lower() in {'png', 'jpg', 'jpeg', 'gif', 'pdf'}


@admin_bp.route('/admin/dashboard/create_user', methods=["GET", "POST"])
@login_required
def create_user_from_admin_view():
    if not current_user.is_admin:
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    
    form = RegisterForm(request.form)
    if form.validate_on_submit():
        print("Form validated")
        with UserReg() as db:
            user_data = {
                'username': form.username.data,
                'first_name': form.first_name.data,
                'last_name': form.last_name.data,
                'email': form.email.data,
                'password': form.password.data,
                'is_admin': False,
                'is_blocked': False,
                'verified': True
            }

            print("Attempting to create user from admin view with data: ", user_data)
            
            result = db.create_user_from_admin(user_data)

            print("Result: ", result)

            if result['success']:
                flash("User created successfully.", "success")
                return redirect(url_for('admin.admin_dashboard'))
            else:
                flash("User creation failed! Unknown reason","error")
                return redirect(url_for('admin.create_user_from_admin_view'))

    else:
        print("Form not validated")
        print(form.errors)
        for field, errors in form.errors.items():
            for error in errors:
                flash(f"{field}: {error}", "error")

    return render_template('create_user.html', form=form)    


  



@admin_bp.route('/admin/dashboard/edit-user/<user_id>', methods=["GET", "POST"])
@login_required
def edit_user(user_id):
    if not current_user.is_admin:
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    
    form = EditUserForm()
    with UserReg() as db:
        if request.method == "POST" and form.validate_on_submit():
            return_code = db.update_user_details(
                user_id,
                form.username.data,
                form.first_name.data,
                form.last_name.data,
                form.email.data,
                form.verified.data,
                form.is_admin.data,
                form.is_blocked.data
                )
            if return_code == 1:
                flash("User details updated successfully.", "success")
            else:
                flash("User details update failed! Unknown reason","error")
            return redirect(url_for('admin.admin_dashboard'))
        else:
            user_data = db.fetch_user_id(user_id)
            if user_data:
                form.username.data = user_data[1]
                form.first_name.data = user_data[2]
                form.last_name.data = user_data[3]
                form.email.data = user_data[4]
                form.verified.data = bool(user_data[6])
                form.is_admin.data = bool(user_data[7])
                form.is_blocked.data = bool(user_data[8])

                return render_template('edit_user.html', form=form, user_id=user_id)
            else:
                flash("User not found.", "error")
                return redirect(url_for('admin.admin_dashboard'))
            

@admin_bp.route('/admin/dashboard/delete-user/<int:user_id>', methods=["GET", "POST"])
@login_required
def delete_user(user_id):
    if not current_user.is_admin:
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    
    with UserReg() as db:
        db.delete_user(user_id)
        flash("User deleted successfully.", "success")
        
    return redirect(url_for('admin.admin_dashboard'))


@admin_bp.route('/admin/dashboard/add-attraction', methods=['GET', 'POST'])
@login_required
def add_attraction():
    if not current_user.is_admin:
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method == 'POST':
        name = request.form['name']
        city_id = request.form['city']
        short_description = request.form['short_description']
        long_description = request.form['long_description']
        opening_hours = request.form['opening_hours']
        entry_fee = request.form['entry_fee']
        public_transport_map = request.form['public_transport_map']
        attraction_map = request.form['attraction_map']
        site_link = request.form['site_link']
        image_file = request.files['image_file']
        attractions= request.form.getlist('attractions')
        services = request.form.getlist('services')
        
        practical_information = {'description': long_description,
                                 'opening_hours': opening_hours,
                                 'entry_fee': entry_fee,
                                 'nearest_public_transport_directions': public_transport_map,
                                 'google_maps_location': attraction_map,
                                 'official_website': site_link}
        short_information = {'name': name,
                             'city_id': city_id,
                             'short_description': short_description}
        
       
        with TuristDb() as db:
            attraction_id = db.add_attraction(short_information,practical_information)
            if attraction_id == -1:
                flash(f"Adding Attraction: {name} failed! It already exists.","error")
            elif attraction_id == -2:
                flash(f"Adding Attraction: {name} failed! Unknown Error","error")
            else:
                db.add_categories_to_attraction(attraction_id,attractions,services)
                filename = f"Attraction_{attraction_id}.jpg"
                file_path = os.path.join(current_app.config['ATTRACTION_IMAGE_FOLDER'], filename)
                image_file.save(file_path)
                flash(f"Adding Attraction: {name} was successful.","success")
        
        return redirect(url_for('admin.admin_dashboard'))
    return render_template('add_attraction.html')

@admin_bp.route("/admin/dashboard/edit_attraction/<attraction_id>",methods=['GET','POST'])
@login_required
def edit_attraction(attraction_id):
    if not current_user.is_admin:
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    if request.method== 'GET':
        with TuristDb() as db:
            data = db.get_attraction(attraction_id)
            categories = db.get_attraction_has_category(attraction_id)
            data['categories'] = categories
            return data
    else:
        name = request.form['name']
        city_id = request.form['city']
        short_description = request.form['short_description']
        long_description = request.form['long_description']
        opening_hours = request.form['opening_hours']
        entry_fee = request.form['entry_fee']
        public_transport_map = request.form['public_transport_map']
        attraction_map = request.form['attraction_map']
        site_link = request.form['site_link']
        image_file = request.files['image_file']
        attractions= request.form.getlist('attractions')
        services = request.form.getlist('services')
        
        practical_information = {'description': long_description,
                                 'opening_hours': opening_hours,
                                 'entry_fee': entry_fee,
                                 'nearest_public_transport_directions': public_transport_map,
                                 'google_maps_location': attraction_map,
                                 'official_website': site_link}
        short_information = {'name': name,
                             'city_id': city_id,
                             'short_description': short_description}
        
       
        with TuristDb() as db:
            return_code_att = db.edit_attraction(attraction_id,short_information,practical_information)
            return_code_cat = db.add_categories_to_attraction(attraction_id,attractions,services)
            if(return_code_att == 1 and return_code_cat == 1):
                flash("Editing attraction was successful","success")
            elif return_code_att == 1 and return_code_cat != 1:
                flash("Editing attraction partially failed! Was unable to add filters to edited attraction")
            else:
                flash("Editing attraction failed! Unknown reason","error")
        if(image_file):
            filename = f"Attraction_{attraction_id}.jpg"
            file_path = os.path.join(current_app.config['ATTRACTION_IMAGE_FOLDER'], filename)
            image_file.save(file_path)

        return redirect(url_for("admin.admin_dashboard"))

@admin_bp.route('/admin/dashboard/remove-attraction/<int:attraction_id>', methods=['POST'])
@login_required
def remove_attraction(attraction_id):
    if not current_user.is_admin:
        flash("You are not authorized to view this page.", "warning")
        return redirect(url_for('miscellaneous.index'))
    with TuristDb() as db:
        filename = f"Attraction_{attraction_id}.jpg"
        file_path = os.path.join(current_app.config['ATTRACTION_IMAGE_FOLDER'], filename)
        os.remove(file_path)
        return_code = db.remove_attraction(attraction_id)
        if return_code == 1:
            flash("Removing attraction was successfull.", "success")
        else:
            flash("Removing attraction failed! Unknown reason.","error")
    return redirect(url_for('admin.admin_dashboard'))