from userregistry import UserReg
from werkzeug.security import generate_password_hash, check_password_hash
from flask_mail import Message
from flask_login import LoginManager, UserMixin
import hashlib



class User(UserMixin):
    # constructor / attributes
    def __init__(self, user_id, username, first_name, last_name, email, password_hash, verified, is_admin, is_blocked, points, lives, timestamp_locked):
        self.user_id = user_id
        self.username = username
        self.first_name = first_name
        self.last_name = last_name
        self.email = email
        self.password_hash = password_hash
        self.verified = bool(verified)
        self.is_admin = bool(is_admin)
        self.is_blocked = bool(is_blocked)
        self.points = points
        self.lives = lives
        self.timestamp_locked = timestamp_locked
        self.user_has_visited_attractions = self.has_visited_attractions()
    

    def set_password(self, password_hash):
        self.password_hash = generate_password_hash(password_hash)

        
    def check_password(self, password_hash):
        return check_password_hash(self.password_hash, password_hash)
    

    def __str__(self):
        return f'Id: {self.user_id}\n' + \
            f'Username: {self.username}\n' + \
            f'Email: {self.email}\n' + \
            f'Password Hash: {self.password_hash}'
    
    def check_isAdmin(self):
        return self.is_admin
    
    
    # def is_authenticated(self):
    #     return self.is_authenticated

    
    # def is_active(self):
    #     return True

   
    # def is_anonymous(self):
    #     return False

    def get_id(self):
        return str(self.user_id)
    
    # def get(self, user_id):
    #     with UserReg() as db:
    #         user = User(*db.fetch_user_id(user_id))
    #         if user:
    #             return user
    #         else:
    #             return False
    
    #Denne funksjonen genererer unike avatars for hver bruker basert på en hash av deres e-post.
    def gravatar(self, size=100, default='identicon', rating='g'):
        digest = hashlib.md5(self.email.lower().encode('utf-8')).hexdigest()
        return f'https://www.gravatar.com/avatar/{digest}?d={default}&s={size}&r={rating}'
    
    def get_achievements(self):
        with UserReg() as db:
            return db.get_user_achievements(self.user_id)
    
    def add_points(self, points):
        self.points += points
        with UserReg() as db:
            db.update_user_points(self.user_id, self.points)
        self.check_for_achievements()

    def check_for_achievements(self):
        with UserReg() as db:
            all_achievements = db.get_all_achievements()
            for achievement in all_achievements:
                if self.points >= int(achievement['points_required']) and not db.has_achievement(self.user_id, achievement['id']):
                    db.award_achievement(self.user_id, achievement['id'])
                elif self.points < int(achievement['points_required']) and db.has_achievement(self.user_id, achievement['id']):
                    db.remove_achievement(self.user_id, achievement['id'])

    def check_count_achievements(self):
        with UserReg() as db:
            return db.count_user_achievements(self.user_id)
        
    def award_user_reward(self, reward_id):
        with UserReg() as db:
            db.award_user_reward(self.user_id, reward_id)    

    def has_received_reward(self, reward_id):
        with UserReg() as db:
            return db.has_user_received_reward(self.user_id, reward_id)
    
    def get_user_rewards(self):
        with UserReg() as db:
            return db.get_user_rewards(self.user_id)
    
    def count_user_rewards(self):
        with UserReg() as db:
            return db.count_user_rewards(self.user_id)

    def redeem_reward(self, id):
        with UserReg() as db:
            db.redeem_reward(id)
        
    @staticmethod
    def get(user_id):
        with UserReg() as db:
            user_data = db.fetch_user_id(user_id)
            if user_data:
                return User(*user_data)
            else:
                return None
            
# Find out if a user has visited an attraction by checking if the user has level 2 or higher in the attraction

    def has_visited_attractions(self):
        visited_attractions = []
        query = """
            SELECT attraction.id, attraction.name, user_has_level.level
            FROM attraction
            INNER JOIN user_has_level ON attraction.id = user_has_level.attraction_id
            WHERE user_has_level.user_id = %s AND user_has_level.level >= 2
        """
        try:
            with UserReg() as db:
                db.cursor.execute(query, (self.user_id,))
                visited_attractions = db.cursor.fetchall()
                print(visited_attractions) # Debugging
        except Exception as e:
            print(f"Error fetching visited attractions: {e}")

        return visited_attractions        

       