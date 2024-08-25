from flask_wtf import FlaskForm
from wtforms import Form, BooleanField,StringField, SubmitField, validators, PasswordField, ValidationError
from wtforms.validators import DataRequired, Email, Length, EqualTo, InputRequired
from wtforms.fields import TextAreaField, HiddenField, EmailField, SelectField
from flask_wtf.file import FileField, FileRequired
from flask_babel import lazy_gettext

class RegisterForm(FlaskForm):
    username = StringField('Username', validators=[DataRequired(message=lazy_gettext("The username field is empty."))],render_kw={"placeholder": lazy_gettext("Username")})
    first_name = StringField('First Name', validators=[DataRequired(message=lazy_gettext("The first name field is empty."))],render_kw={"placeholder": lazy_gettext("First Name")})
    last_name = StringField('Last Name', validators=[DataRequired(message=lazy_gettext("The last name field is empty."))],render_kw={"placeholder": lazy_gettext("Last Name")})
    email = EmailField('Email Address', validators=[DataRequired(), Email(), Length(max=120)],render_kw={"placeholder": lazy_gettext("Email")})
    password = PasswordField("Password:", validators=[DataRequired(), Length(min=6,  message=lazy_gettext("The password must be at least 6 characters long."))],render_kw={"placeholder": lazy_gettext("Password")})
    confirm_password = PasswordField('Confirm Password: ', validators=[DataRequired(), EqualTo('password', message =lazy_gettext("Confirm password does not match the password."))],render_kw={"placeholder": lazy_gettext("Confirm Password")})
    agree_to_terms = BooleanField(validators=[DataRequired()])
    id = HiddenField()


class LoginForm(FlaskForm):
    email = EmailField('Email Address', validators=[DataRequired(message=lazy_gettext("The email field is empty.")), Email(), Length(max=120)])
    password = PasswordField("Password:", validators=[DataRequired(message=lazy_gettext("Password is missing")), Length(min=6)])

class ForgotPasswordForm(FlaskForm):
    email = EmailField('Email Address', validators=[DataRequired(), Email(), Length(max=120)],render_kw={"placeholder": lazy_gettext("Email")})

class ResetPasswordForm(FlaskForm):
    password = PasswordField('New Password', validators=[DataRequired(), Length(min=6,  message=lazy_gettext("The password must be at least 6 characters long."))])
    confirm = PasswordField('Repeat Password', validators=[DataRequired(), EqualTo('password', message=lazy_gettext('Passwords must match.'))])
    submit = SubmitField('Reset Password')


class EditUserForm(FlaskForm):
    username = StringField('Username', validators=[DataRequired(message=lazy_gettext("The username field is empty."))],render_kw={"placeholder": lazy_gettext("Username")})
    first_name = StringField('First Name', validators=[DataRequired(message=lazy_gettext("The first name field is empty."))],render_kw={"placeholder": lazy_gettext("First Name")})
    last_name = StringField('Last Name', validators=[DataRequired(message=lazy_gettext("The last name field is empty."))],render_kw={"placeholder": lazy_gettext("Last Name")})
    email = EmailField('Email Address', validators=[DataRequired(), Email(), Length(max=120)],render_kw={"placeholder": lazy_gettext("Email")})
    verified = BooleanField('Verified')
    is_admin = BooleanField('Is Admin')
    is_blocked = BooleanField('Is Blocked')
    
class QuizForm(FlaskForm):
    submit = SubmitField(lazy_gettext('Submit'))
        
class AddFriendForm(FlaskForm):
    friend_email = EmailField(lazy_gettext('Friend Email'), validators=[DataRequired(), Email()])
    submit_add = SubmitField(lazy_gettext('Add Friend'))

class DeleteFriendForm(FlaskForm):
    friend_id = HiddenField(validators=[DataRequired()])
    submit_delete = SubmitField(lazy_gettext('Remove Friend'))

class RedeemRewardForm(FlaskForm):
    submit = SubmitField(lazy_gettext('Redeem'))