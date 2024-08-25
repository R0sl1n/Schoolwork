import mysql.connector
from mysql.connector import errorcode
import bcrypt

class UserReg:

    def __init__(self) -> None:

        dbconfig = {'host': '127.0.0.1',
                    'user': 'user',
                    'password': 'test',
                    'database': 'turistdb', }

        self.configuration = dbconfig

    def __enter__(self) -> 'UserReg':
        self.conn = mysql.connector.connect(**self.configuration)
        self.cursor = self.conn.cursor(prepared=True)
        return self

    def __exit__(self, exc_type, exc_val, exc_trace) -> None:
        self.conn.commit()
        self.cursor.close()
        self.conn.close()

    def fetch_email(self, email):
        try:
            self.cursor.execute("SELECT * FROM user WHERE email=%s", (email,))
            result = self.cursor.fetchone()
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            result = None
        return result

    def fetch_user(self, username):
        query = "SELECT * FROM user WHERE username=%s"
        print(f"Executing query: {query} with username: {username}")
        try:
            self.cursor.execute(query, (username,))
            result = self.cursor.fetchone()
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            result = None
        return result

    def fetch_all_users(self):
        try:
            self.cursor.execute("SELECT id, username, first_name, last_name, email, Verified, is_admin, is_blocked FROM user")
            result = self.cursor.fetchall()
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            result = []
        return result


    def fetch_user_id(self, user_id):
        try:
            self.cursor.execute("SELECT * FROM user WHERE id=%s", (user_id,))
            result = self.cursor.fetchone()
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            result = None

        return result

    def fetch_blocked_username(self, blocked_username):
        query = "SELECT * FROM blocked_username WHERE blocked_username=LOWER(%s)"
        print(f"Executing query: {query} with blocked_username: {blocked_username}")
        try:
            self.cursor.execute(query, (blocked_username,))
            result = self.cursor.fetchone()
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            result = None
        return result

    def create_user(self, user):
        try:
            sql = """INSERT INTO user (username, first_name, last_name, email, password) 
                    VALUES (%s, %s, %s, %s, %s)"""
            self.cursor.execute(sql, user)
            # Add entries in levels on all attractions for this user
            user_id = self.cursor.lastrowid
            self.cursor.execute("SELECT id FROM attraction")
            result = self.cursor.fetchall()
            attraction_ids = [row[0] for row in result]
            for attraction_id in attraction_ids:
                self.cursor.execute("INSERT INTO user_has_level (user_id, attraction_id, level) VALUES (%s, %s, 1)", (user_id, attraction_id,))
                self.conn.commit();
        except mysql.connector.Error as err:
            print(f"Database error: {err}")

    def create_user_from_admin(self, user):
        try:
            sql = """INSERT INTO user (username, first_name, last_name, email, password_hash)
                    VALUES (%s, %s, %s, %s, %s)"""
            self.cursor.execute(sql, (
                user['username'],
                user['first_name'],
                user['last_name'],
                user['email'],
                user['password']
            ))

            user_id = self.cursor.lastrowid
            print(f"User ID: {user_id}")
            self.conn.commit()

            if user_id:
                return {"success": True, "user_id": user_id}
            else:
                return {"success": False, "error": "Failed to create user"}

        except mysql.connector.Error as err:
            self.conn.rollback()
            print(f"Database error: {err}")
            return {"success": False}

    def update_password_hash(self, user_id, new_password_hash):
        try:
            sql = "UPDATE user SET password_hash=%s WHERE id=%s"
            self.cursor.execute(sql, (new_password_hash, user_id))
        except mysql.connector.Error as err:
            print(f"Database error: {err}")

    def verify_email(self, email):
        try:
            self.cursor.execute("UPDATE user SET Verified = 1 WHERE email = (%s)", (email,))
        except mysql.connector.Error as err:
            print(f"Database error: {err}")

    def validate_login(self, username, password):
        user = self.fetch_user(username)
        if user:
            hashed = user[5]
            return bcrypt.checkpw(password.encode('utf-8'), hashed)
        return False
    
    def update_user_details(self, user_id, username, first_name, last_name, email, verified, is_admin, is_blocked) -> int:
        sql = """UPDATE user SET username=%s, first_name=%s, last_name=%s, email=%s, verified=%s, is_admin=%s, is_blocked=%s WHERE id=%s"""
        try:
            self.cursor.execute(sql, (username, first_name, last_name, email, verified, is_admin, is_blocked, user_id))
            return 1
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return -1
        
    def delete_user(self, user_id):
        try:
            # Entries in levels for this user must be removed first
            self.cursor.execute("DELETE FROM user_has_level WHERE user_id = %s", (user_id,))
            self.conn.commit()
            # Remove user
            self.cursor.execute("DELETE FROM user WHERE id=%s", (user_id,))
            self.conn.commit()
        except Exception as e:
            print(f"Failed to delete user {user_id}: {e}")
            self.conn.rollback()
    
    def get_user_achievements(self, user_id):
        self.cursor.execute("""
        SELECT a.definition, a.points_required, a.data, a.icon, uha.achieved_at
        FROM achievement a
        JOIN user_has_achievement uha ON a.id = uha.id_achievement
        WHERE uha.id_user = %s
        """, (user_id,))
        columns = [col[0] for col in self.cursor.description]
        return [dict(zip(columns, row)) for row in self.cursor.fetchall()]
    
    def update_user_points(self, user_id, points):
        query = "UPDATE user SET points = %s WHERE id = %s"
        try:
            self.cursor.execute(query, (points, user_id))
        except mysql.connector.Error as err:
            print(f"Error in update_user_points: {err}")
        
    # get all achievements
    def get_all_achievements(self):
        query = "SELECT id, definition, points_required FROM achievement"
        try:
            self.cursor.execute(query)
            columns = [col[0] for col in self.cursor.description]
            return [dict(zip(columns, row)) for row in self.cursor.fetchall()]
        except mysql.connector.Error as err:
            print(f"Error in get_all_achievements: {err}")
            return []
            
    # check if user has an achievement
    def has_achievement(self, user_id, achievement_id):
        query = "SELECT COUNT(1) FROM user_has_achievement WHERE id_user = %s AND id_achievement = %s"
        try:
            self.cursor.execute(query, (user_id, achievement_id))
            result = self.cursor.fetchone()
            return result[0] > 0
        except mysql.connector.Error as err:
            print(f"Error in has_achievement: {err}")
            return False

    # award an achievement to a user
    def award_achievement(self, user_id, achievement_id):
        query = "INSERT INTO user_has_achievement (id_user, id_achievement) VALUES (%s, %s)"
        try:
            self.cursor.execute(query, (user_id, achievement_id))
        except mysql.connector.Error as err:
            print(f"Error in award_achievement: {err}")

    # count the number of achievements a user has
    def count_user_achievements(self, user_id):
        query = "SELECT COUNT(*) FROM user_has_achievement WHERE id_user = %s"
        try:
            self.cursor.execute(query, (user_id,))
            count = self.cursor.fetchone()[0]
            return count
        except mysql.connector.Error as err:
            print(f"Error counting user achievements: {err}")
            return None
        
    # award a user a reward
    def award_user_reward(self, user_id, reward_id):
        query = "INSERT INTO user_has_reward (id_user, id_reward) VALUES (%s, %s)"
        try:
            self.cursor.execute(query, (user_id, reward_id))
        except mysql.connector.Error as err:
            print(f"Error inserting user reward: {err}")
    
    # check if user has received a reward so they can't receive it again
    def has_user_received_reward(self, user_id, reward_id):
        query = "SELECT COUNT(*) FROM user_has_reward WHERE id_user = %s AND id_reward = %s"
        try:
            self.cursor.execute(query, (user_id, reward_id))
            count = self.cursor.fetchone()[0]
            return count > 0
        except mysql.connector.Error as err:
            print(f"Error checking user reward: {err}")
            return False

    # get a specific reward by id
    def get_reward_id(self, reward_id):
        query = "SELECT id FROM rewards WHERE id = %s"
        try:
            self.cursor.execute(query, (reward_id,))
            result = self.cursor.fetchone()
            if result:
                return result[0] 
            else:
                return None
        except mysql.connector.Error as err:
            print(f"Error retrieving reward id: {err}")
            return None
    
    def get_user_rewards(self, user_id):
        query = """
        SELECT uhr.id, r.reward_type, r.details
        FROM user_has_reward uhr
        JOIN rewards r ON uhr.id_reward = r.id
        WHERE uhr.id_user = %s AND uhr.has_redeemed = 0
        """
        try:
            self.cursor.execute(query, (user_id,))
            rewards = self.cursor.fetchall()
            return rewards
        except mysql.connector.Error as err:
            print(f"Error retrieving user rewards: {err}")
            return None

    
    def count_user_rewards(self, user_id):
        query = "SELECT COUNT(*) FROM user_has_reward WHERE id_user = %s AND has_redeemed = 0"
        try:
            self.cursor.execute(query, (user_id,))
            count = self.cursor.fetchone()[0]
            return count
        except mysql.connector.Error as err:
            print(f"Error counting user rewards: {err}")
            return 0
    
    def redeem_reward(self, id):
        query = """UPDATE user_has_reward SET has_redeemed = 1 WHERE id= %s"""
        try:
            self.cursor.execute(query, (id,))
        except mysql.connector.Error as err:
            print(f"Error redeeming reward: {err}")


    # remove an achievement from a user (to make sure it gets removed if you were to lower points)
    def remove_achievement(self, user_id, achievement_id):
        query = "DELETE FROM user_has_achievement WHERE id_user = %s AND id_achievement = %s"
        try:
            self.cursor.execute(query, (user_id, achievement_id))
        except mysql.connector.Error as err:
            print(f"Error in remove_achievement: {err}")

    def fetch_quiz_by_id(self, quiz_id):
        query = "SELECT * FROM quiz WHERE id = %s"
        try:
            self.cursor.execute(query, (quiz_id,))
            return self.cursor.fetchone()
        except mysql.connector.Error as err:
            print(f"Error fetching quiz by ID: {err}")
            return None
    def add_points(self, user_level, user_id):
        try:
            # points given is 500 * level per quiz
            self.cursor.execute(""" UPDATE user SET points = points + (50 * %s)  WHERE id = %s """, (user_level, user_id,))
        except mysql.connector.Error as err:
            print(f"Database error: {err}")

    def get_friends(self, user_id):
        try:
            self.cursor.execute("""
                SELECT u.id, u.username, u.email, COALESCE(u.points, 0) AS points
                FROM user AS u
                JOIN user_has_friend AS uhf ON u.id = uhf.id_user_friend
                WHERE uhf.id_user = %s
                ORDER BY u.points DESC  
            """, (user_id,))
            friends = self.cursor.fetchall()
            return friends
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return []

    def add_friend(self, user_id, friend_id):
        sql = "INSERT INTO user_has_friend (id_user, id_user_friend) VALUES (%s, %s)"
        self.cursor.execute(sql, (user_id, friend_id))

    def remove_friend(self, user_id, friend_id):
        sql = "DELETE FROM user_has_friend WHERE id_user = %s AND id_user_friend = %s"
        self.cursor.execute(sql, (user_id, friend_id))
        
    def is_friend(self, user_id, friend_id):
        query = "SELECT EXISTS(SELECT 1 FROM user_has_friend WHERE id_user=%s AND id_user_friend=%s)"
        self.cursor.execute(query, (user_id, friend_id))
        return self.cursor.fetchone()[0]


    # DATABASE OPERATIONS FOR LIVES FEATURE
    ##############################################
    def fetch_lives_for_user(self, user_id):
        try:
            self.cursor.execute("SELECT lives FROM user WHERE id = %s", (user_id,))
            result = self.cursor.fetchone()
            if result:
                return result[0]
            return None
        except Exception as e:
            print(f"Error fetching lives for user: {e}")
            return None
        
    def fetch_locked_timestamp(self, user_id):
        try:
            self.cursor.execute("SELECT timestamp_locked FROM user WHERE id = %s", (user_id,))
            result = self.cursor.fetchone()
            if result:
                return result[0]
            return None
        except Exception as e:
            print(f"Error fetching locked timestamp for user: {e}")
            return None
    
    def update_locked_timestamp(self, user_id, locked_timestamp):
        try:
            self.cursor.execute("UPDATE user SET timestamp_locked = %s WHERE id = %s", (locked_timestamp, user_id,))
            self.conn.commit()
        except Exception as e:
            print(f"Error setting locked timestamp for user: {e}")
            return None 
        
    def update_lives(self, user_id, lives):
        try:
            self.cursor.execute("UPDATE user SET lives = %s WHERE id = %s", (lives, user_id, ))
            self.conn.commit()
        except mysql.connector.Error as err:
            print(f"Failed updating lives for user: {err}")
            return None
