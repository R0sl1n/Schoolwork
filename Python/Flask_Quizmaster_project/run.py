from app import app
from app.models import User, db

@app.shell_context_processor
def make_shell_context():
    return {'db': db, 'User': User}

if __name__ == "__main__":
    app.run(debug=True)