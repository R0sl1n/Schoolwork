from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, BooleanField, SubmitField, EmailField, TextAreaField, HiddenField
from wtforms.validators import DataRequired, ValidationError, EqualTo
from app.models import User

class LoginForm(FlaskForm):
	username = StringField('Username', validators=[DataRequired()])
	password = PasswordField('Password', validators=[DataRequired()])
	remember_me = BooleanField('Remember me')
	submit = SubmitField('Sign in')
    
class RegistrationForm(FlaskForm):
    username = StringField('Username', validators=[DataRequired()])
    givenName = StringField('First name', validators=[DataRequired()])
    lastName = StringField('Last name', validators=[DataRequired()])
    password = PasswordField('Password', validators=[DataRequired()])
    password2 = PasswordField(
        'Repeat Password', validators=[DataRequired(), EqualTo('password')])
    submit = SubmitField('Register')

    def validate_username(self, username):
        user = User.query.filter_by(username=username.data).first()
        if user is not None:
            raise ValidationError('Please use a different username. Username already registered.')

class AnswerForm(FlaskForm):
    answer = TextAreaField('Answer', validators=[DataRequired()])
    submit = SubmitField('Submit')
    quiz_comment = StringField('quiz_comment')
    quiz_status = StringField('quiz_status', default=0)
    id = StringField('ID')
    id_qst = StringField('ID')
    definition = TextAreaField('Question', validators=[DataRequired()])
    alt1 = StringField('Option 1', validators=[DataRequired()])
    alt2 = StringField('Option 2')
    alt3 = StringField('Option 3')
    alt4 = StringField('Option 4')
    alt5 = StringField('Option 5')
    type_id = StringField('Type ID', validators=[DataRequired()])
    category_id = StringField('Category ID', validators=[DataRequired()])
    current_question_id = HiddenField()
    answer = StringField('Answer')
    submit = SubmitField('Submit')
    

    
    