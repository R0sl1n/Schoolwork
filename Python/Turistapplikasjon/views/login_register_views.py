from werkzeug.security import generate_password_hash, check_password_hash
from flask import flash, request, redirect, render_template, url_for,Blueprint, current_app
from flask_login import current_user, login_user, logout_user, login_required
from werkzeug.security import generate_password_hash, check_password_hash
from flask_mail import Message
from itsdangerous import BadTimeSignature, URLSafeTimedSerializer, SignatureExpired

from forms import LoginForm, RegisterForm, ForgotPasswordForm, ResetPasswordForm
from userregistry import UserReg
from user import User

login_reg_bp = Blueprint("login_reg",__name__,template_folder="templates")
s = URLSafeTimedSerializer(current_app.secret_key) # URL-Token for epost-validering

@login_reg_bp.route('/login', methods=["GET", "POST"])
def login():
    if current_user.is_authenticated:
        return render_template('index.html')
    form = LoginForm(request.form)
    if request.method == "POST" and form.validate():
        email = request.form['email']
        password = request.form['password']
        with UserReg() as db:
            user_data = db.fetch_email(email)
            if user_data is None:
                flash("No account with this email exist.","error")
                return redirect(url_for("login_reg.login"))
            elif user_data:
                user = User(*user_data)
                pwd_hash = user.password_hash
                if check_password_hash(pwd_hash, password) == True:
                    if user.is_blocked:
                        flash("User is blocked.","error")
                        return redirect(url_for("login_reg.login"))
                    # Removed functionality until email verification is available.
                   # elif not user.verified:
                   #     flash("Please verify your email before logging in","error")
                   #     return redirect(url_for("login_reg.login"))
                    login_user(user, remember=True)
                else:
                    flash("Incorrect password.","error")
                    return redirect(url_for("login_reg.login"))
            return redirect(url_for('attractions.attractions'))
    return render_template('login.html', form=form)


@login_reg_bp.route('/forgot-password', methods=['GET', "POST"])
def forgot_password():
    form = ForgotPasswordForm(request.form)
    if request.method == "POST" and form.validate():
        email = request.form['email']
        with UserReg() as db:
            user = db.fetch_email(email)
            if user:
                token = s.dumps(email, salt='password-reset-salt')
                reset_link = url_for('login_reg.reset_password', token=token, _external=True)
                msg = Message("Password Reset Request", sender='kst195@uit.no', recipients=[email])
                msg.body = f'Please click on the link to reset your password: {reset_link}'
                print(msg) # Midlertidig løsning for få opp linken uten mailserver
                #mail.send(msg)
                flash('Please check your email for a password reset link.',"success")
            else:
                flash('This email is not registered.', 'error')
    return render_template('forgot_password.html', form=form)


@login_reg_bp.route('/reset-password/<token>', methods=['GET', 'POST'])
def reset_password(token):
    try:
        email = s.loads(token, salt='password-reset-salt', max_age=3600)
    except:
        flash('The password reset link is invalid or has expired.', 'error')
        return redirect(url_for('login_reg.forgot_password'))
    form = ResetPasswordForm(request.form)
    if form.validate_on_submit(): 
        new_password = form.password.data
        with UserReg() as db:
            user = db.fetch_email(email)
            if user:
                user_id = user[0]
                new_password_hash = generate_password_hash(new_password)
                db.update_password_hash(user_id, new_password_hash)
                flash('Your password has been updated!', 'success')
                return redirect(url_for('login_reg.login'))
            else:
                flash('User not found.', 'error')
                return redirect(url_for('login_reg.forgot_password'))
    return render_template('reset_password.html', form=form, token=token)


@login_reg_bp.route('/signup', methods=["GET", "POST"])
def signup():
    if current_user.is_authenticated:
        return render_template('index.html')
    form = RegisterForm(request.form)
    if request.method == "POST" and form.validate():
        username = request.form['username']
        first_name = request.form['first_name']
        last_name = request.form['last_name']
        email = request.form['email']
        password = request.form['password']
        hashed_pw = generate_password_hash(password)
        new_user = (username, first_name, last_name, email, hashed_pw)
        with UserReg() as db:
            check_username = db.fetch_user(username)
            check_username_blocked = db.fetch_blocked_username(username)
            check_email = db.fetch_email(email)
            if check_username is not None:
                flash("Username is already in use.","error")
                return render_template('register.html',form=form)
            elif check_username_blocked is not None:
                flash("The username is not valid.","error")
                return render_template('register.html',form=form)    
            elif check_email is not None:
                flash("Email is already in use.","error")
                return render_template('register.html',form=form)
            else:
                db.create_user(new_user)
                # Uncomment and update the following lines as needed for email verification
                # token = s.dumps(email) 
                # msg = Message('Confirm your email', sender='your-email@example.com', recipients=[email])
                # link = url_for('verify', token=token, _external=True) 
                # msg.body = 'Follow this link to confirm your email: {}'.format(link)
                # mail.send(msg)
                # return render_template('verified.html')
                flash("Account registration successful.","success")
                return redirect("login")
    return render_template('register.html', form=form)


@login_reg_bp.route('/verify/<token>')
def verify(token):
    try:
        email = s.loads(token, max_age=3600)  # Must be verified within 1 hour
    except SignatureExpired:
        msg = "The verification email has expired. Please register your email again."
        return render_template("error.html", msg=msg)
    except BadTimeSignature:
        msg = "Invalid verification token"
        return render_template("error.html", msg=msg)

    with UserReg() as db:
        if db.fetch_email(email) is not None:
            db.verify_email(email)
    return render_template('verified.html', email=email)


@login_reg_bp.route('/logout', methods = ["GET", "POST"])
@login_required
def logout():
    logout_user()
    flash("You have been logged out.")
    return redirect(url_for('miscellaneous.index'))