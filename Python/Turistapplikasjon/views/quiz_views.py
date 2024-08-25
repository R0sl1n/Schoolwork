from flask import flash, request, redirect, render_template, url_for, session, Blueprint
from flask_login import current_user, login_required
from forms import QuizForm
from userregistry import UserReg

from turistdb import TuristDb
import random


quiz_bp = Blueprint("quiz",__name__,template_folder="templates")

@quiz_bp.route('/complete_quiz/<quiz_id>', methods=['POST'])
@login_required
def complete_quiz(quiz_id):
    with UserReg() as db: 
        quiz = db.fetch_quiz_by_id(quiz_id)
    if quiz is None:
        flash('Quiz not found.', 'error')
        return redirect(url_for('miscellaneous.index')) 
    points_awarded = calculate_points(quiz)
    current_user.add_points(points_awarded)
    current_user.check_for_achievements(current_user.user_id)
    achievements = db.get_all_achievements()
    for achievement in achievements:
        threshold_points = achievement['points_required']
        if current_user.points >= threshold_points:
            flash(f"Congratulations! You've reached the point threshold for the '{achievement['definition']}' achievement.")
    flash(f'Congratulations, you have earned {points_awarded} points!', 'success')

    return redirect(url_for('profile.profile'))

def calculate_points(quiz):
    if quiz['level'] == 1:
        return 5 
    elif quiz['level'] == 2:
        return 10
    else:
        return 0


@quiz_bp.route('/take-quiz/<int:attraction_id>', methods=["GET"])
@login_required
def take_quiz(attraction_id):
    with UserReg() as db:
        # FEATURE: Lives
        lives = db.fetch_lives_for_user(current_user.user_id)
        if lives < 1:
            return redirect(url_for('attractions.attractions'))
    with TuristDb() as db:
        attraction_name = db.fetch_attraction_name(attraction_id)
        user_level = db.fetch_level_for_attraction(attraction_id, current_user.user_id)
        questions = db.fetch_questions_for_attraction(attraction_id, user_level)
        if questions:
            answers = dict()
            for question in questions:
                    answers['question' + str(question['id'])] = question['correct_answer']
                    random.shuffle(question['options'])
            session['answers'] = answers
            if not attraction_name:
                flash("attraction not found.", "error")
                return redirect(url_for('attractions.attractions'))
            form = QuizForm()
        else:
            flash("No quiz available for this attraction at this level", 'quiz_response')
            return redirect(url_for('attractions.attractions'))
    return render_template('take_quiz.html', form=form, user_level=user_level, attraction_name=attraction_name, questions=questions, attraction_id=attraction_id)


@quiz_bp.route('/submit-quiz-answers/<int:attraction_id>', methods=["POST"])
@login_required
def submit_quiz_answers(attraction_id):
    form = QuizForm()
    QUIZ_POINTS = 50
    if request.method == "POST" and form.validate_on_submit():
        user_level = request.form['level']
        with TuristDb() as db:
            attraction_level = db.fetch_level_for_attraction(attraction_id, current_user.user_id)
        # Bugfix for resubmit if going back to question page
        if int(attraction_level) != int(user_level):
            return redirect(url_for('attractions.attractions'))
        user_answers = {key: request.form[key] for key in request.form.keys() if key.startswith('question')}
        correct_answers: dict = session.get('answers', None)
        if correct_answers == user_answers:
            with TuristDb() as db:
                db.update_level(attraction_id, current_user.user_id)
            with UserReg() as db:
                db.add_points(user_level, current_user.user_id)
            flash("Quiz Successs: New level unlocked", 'quiz_response')
            flash(f"Points awarded: {QUIZ_POINTS * int(user_level)}", 'quiz_response')
        else:
            # FEATURE: Lives
            with UserReg() as db:
                lives = db.fetch_lives_for_user(current_user.user_id)
                db.update_lives(current_user.user_id, lives - 1)
                if lives <= 1:
                    from datetime import datetime
                    ts_now = datetime.now()
                    db.update_locked_timestamp(current_user.user_id, ts_now)
            flash("Quiz Failed: Incorrect answers", 'quiz_response')
            flash("Quiz Failed: You've lost a heart", 'lives_update')
        achievement_count = current_user.check_count_achievements()
        # check if user has 5 achievements to be eligible for reward
        if achievement_count == 5:
            with UserReg() as db: 
                reward_id = db.get_reward_id(1)
                # check if user has already received the reward
                if not current_user.has_received_reward(reward_id):
                    current_user.award_user_reward(reward_id)
    return redirect(url_for('attractions.attractions'))