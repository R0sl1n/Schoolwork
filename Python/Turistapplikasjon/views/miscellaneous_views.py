from flask import request, redirect, render_template, url_for, session, Blueprint
from flask_login import login_required
from turistdb import TuristDb


miscellaneous_bp = Blueprint("miscellaneous",__name__,template_folder="templates")

@miscellaneous_bp.route('/', methods = ["GET", "POST"])
@miscellaneous_bp.route('/index', methods = ["GET", "POST"])
def index():
    return render_template('index.html')

@miscellaneous_bp.route('/change_language', methods=['GET'])
def change_language():
    language = request.args.get('language')
    supported_languages = ['en', 'de', 'es', 'nl','fe', 'it'] 
    previous_page = request.referrer or url_for('miscellaneous.index')
    if language in supported_languages:
        session['language'] = language
        return redirect(previous_page)
    else:
        return redirect(previous_page) 

@miscellaneous_bp.route('/about')
def about():
    return render_template('about.html')   


@miscellaneous_bp.route('/leaderboard')
@login_required
def leaderboard():
    leaderboard = None
    with TuristDb() as db:
        leaderboard = db.fetch_leaderboard()
    return render_template('leaderboard.html', leaderboard=leaderboard)



@miscellaneous_bp.route("/privacy-policy")
def privacy_policy():
    return render_template("privacy_policy.html")