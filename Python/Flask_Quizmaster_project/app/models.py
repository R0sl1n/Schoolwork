from werkzeug.security import generate_password_hash, check_password_hash
from flask_login import UserMixin
from app import db, login, app

class User(UserMixin, db.Model):
    __tablename__ = 'user'
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    username = db.Column(db.String(64), index=True, unique=True)
    firstname = db.Column(db.String(64))
    lastname = db.Column(db.String(64))
    password_hash = db.Column(db.String(128))
    isAdmin = db.Column(db.SmallInteger, default=0, nullable=False)
    quiz_responses = db.relationship('Quiz_Response', back_populates='user', cascade='all, delete')

    def __repr__(self):
        return self.username

    def get_fullName(self):
        return self.firstname, self.lastname

    def set_password(self, password):
        self.password_hash = generate_password_hash(password)

    def check_password(self, password):
        return check_password_hash(self.password_hash, password)

    def check_isAdmin(self):
        return self.isAdmin


class Type(db.Model):
    __tablename__ = 'type'
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    category = db.Column(db.String(45), index=True, unique=True)


class Category(db.Model):
    __tablename__ = 'category'
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    definition = db.Column(db.String(45))


class Quiz_Question(db.Model):
    __tablename__ = 'quiz_question'
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    definition = db.Column(db.String(200))
    type_id = db.Column(db.Integer, db.ForeignKey('type.id'))
    active = db.Column(db.Integer,default=1 )
    category_id = db.Column(db.Integer, db.ForeignKey('category.id'))
    alt1 = db.Column(db.String(200), nullable=False)
    alt2 = db.Column(db.String(200))
    alt3 = db.Column(db.String(200))
    alt4 = db.Column(db.String(200))
    alt5 = db.Column(db.String(200))
    

    type = db.relationship('Type', backref='questions')
    category = db.relationship('Category', backref='questions')
    quiz_responses = db.relationship('Quiz_Response', back_populates='question', cascade='all, delete')


class Quiz_Response(db.Model):
    __tablename__ = 'quiz_response'
    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    id_qst = db.Column(db.Integer, db.ForeignKey('quiz_question.id'),nullable=False)
    id_user = db.Column(db.Integer, db.ForeignKey('user.id'), nullable=False)
    quiz_answer = db.Column(db.String(200))
    quiz_status = db.Column(db.Integer, default=0)  # New field to determine if the quiz response is approved
    quiz_comment = db.Column(db.String(200))  # New field to hold the admin's comment on this quiz response

    question = db.relationship('Quiz_Question', back_populates='quiz_responses')
    user = db.relationship('User', back_populates='quiz_responses')



with app.app_context():
    db.create_all()
    
@login.user_loader
def load_user(id):
    return User.query.get(int(id))