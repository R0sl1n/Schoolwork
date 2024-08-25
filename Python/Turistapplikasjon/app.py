from flask import Flask, session
from flask_login import LoginManager
from flask_wtf.csrf import CSRFProtect
from flask_mail import Mail
import secrets
from userregistry import UserReg
from user import User
from flask_babel import Babel
import os.path
from views.miscellaneous_views import miscellaneous_bp
from views.attractions_views import attractions_bp
from views.admin_views import admin_bp
from views.quiz_views import quiz_bp
from views.profile_views import profile_bp

app = Flask(__name__)
#app.config['MAIL_SERVER'] = 'smtpserver.uit.no'
#app.config['MAIL_PORT'] = 587
mail = Mail(app)

app.config['SESSION_COOKIE_SECURE'] = False

APP_ROOT = os.path.dirname(os.path.abspath(__file__))
ATTRACTION_IMAGE_FOLDER = os.path.join(APP_ROOT, 'static', 'img/attraction-images')
REWARD_FOLDER = os.path.join(APP_ROOT, 'static', 'rewards')
app.config['ATTRACTION_IMAGE_FOLDER'] = ATTRACTION_IMAGE_FOLDER
app.config['REWARD_FOLDER'] = REWARD_FOLDER

csrf = CSRFProtect(app)
csrf.init_app(app)
app.secret_key = secrets.token_urlsafe(16)


login_manager = LoginManager()
login_manager.init_app(app)

babel = Babel(app)

with app.app_context():
    from views.login_register_views import login_reg_bp

app.register_blueprint(miscellaneous_bp)
app.register_blueprint(attractions_bp)
app.register_blueprint(admin_bp)
app.register_blueprint(login_reg_bp)
app.register_blueprint(quiz_bp)
app.register_blueprint(profile_bp)

# Set the default language to English
def get_locale():
    return session.get('language', 'en')

babel.init_app(app, locale_selector=get_locale)

# Translation directory
app.config['BABEL_TRANSLATION_DIRECTORIES'] = 'translations'


def get_all_users():
    with UserReg() as db:
        return db.fetch_all_users()


@login_manager.user_loader
def load_user(user_id):
    return User.get(user_id)


