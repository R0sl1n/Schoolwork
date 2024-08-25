import mysql.connector
from mysql.connector import errorcode
import json

class TuristDb:
    
    def __init__(self) -> None:
        dbconfig = {'host': '127.0.0.1',
                    'user': 'user',
                    'password': 'test',
                    'database': 'turistdb',}
        self.configuration = dbconfig

    def __enter__(self) -> 'TuristDb':
        self.conn = mysql.connector.connect(**self.configuration)
        self.cursor = self.conn.cursor(prepared=True)
        return self

    def __exit__(self, exc_type, exc_val, exc_trace) -> None:
        self.conn.commit()
        self.cursor.close()
        self.conn.close()

    def fetch_attractions(self):
        try:
            self.cursor.execute("SELECT id, name, city_id, short_description FROM attraction")
            columns = [col[0] for col in self.cursor.description]
            result = [dict(zip(columns, row)) for row in self.cursor.fetchall()]
            return result
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None
    
    def fetch_filtered_attractions(self,attraction_list,service_list,selected_city):

        if len(attraction_list) == 0 or len(service_list) == 0:
            statement_attractions = "SELECT * FROM attraction WHERE city_id = %s AND id IN (SELECT attraction_id from attraction_has_category attraction_id WHERE category_id in ("
            data = attraction_list if len(service_list) == 0 else service_list
            for i in range(len(data)):
                statement_attractions += "%s"
                if i == len(data)-1:
                    statement_attractions += "))"
                else:
                    statement_attractions += ","
        else:
            statement_attractions = "SELECT * FROM attraction WHERE city_id = %s AND id IN (select t1.attraction_id from (SELECT DISTINCT(attraction_id) from attraction_has_category where "
            data = attraction_list+service_list
            for i in range(len(attraction_list)):
                statement_attractions += "category_id =%s"
                if i == len(attraction_list)-1:
                    statement_attractions+= ") as t1 INNER JOIN (select attraction_id from attraction_has_category where category_id in ("
                else:
                    statement_attractions += " or "
            for j in range(len(service_list)):
                statement_attractions+="%s"
                if j == len(service_list)-1:
                    statement_attractions+= ") group by attraction_id having count(attraction_id) ="
                else:
                    statement_attractions+=","
            statement_attractions+= str(len(service_list))+") as t2 on t1.attraction_id = t2.attraction_id)"
        data.append(selected_city)
        try:
            self.cursor.execute(statement_attractions,data)
            columns = [col[0] for col in self.cursor.description]
            result = [dict(zip(columns, row)) for row in self.cursor.fetchall()]
           

        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            result = None
        return result

    def fetch_all_cities(self):
        try:
            self.cursor.execute("SELECT id, name FROM city")
            columns = [col[0] for col in self.cursor.description]
            result = [dict(zip(columns, row)) for row in self.cursor.fetchall()]
            return result
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None


    def get_category_attractions(self) -> list[tuple]:
        try:
            self.cursor.execute("SELECT id,definition,icon FROM `category` WHERE category_type_id = 1")
            result = self.cursor.fetchall()
            result = [(row[0],row[1],row[2]) for row in result]
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            result = None
        return result
    
    def add_attraction(self, short_information,practical_information):
        try:
            self.cursor.execute("INSERT INTO attraction (name, city_id,short_description, practical_information) VALUES (%s, %s, %s, %s)", 
                                (short_information['name'], short_information['city_id'], short_information['short_description'],json.dumps(practical_information)))
            self.conn.commit()
            # Add entires in levels for all users on for this attraction
            attraction_id = self.cursor.lastrowid
            self.cursor.execute("SELECT id FROM user")
            result = self.cursor.fetchall()
            user_ids = [row[0] for row in result]
            for user_id in user_ids:
                self.cursor.execute("INSERT INTO user_has_level (user_id, attraction_id, level) VALUES (%s, %s, 1)", (user_id, attraction_id,))
                self.conn.commit()
            return attraction_id
        except mysql.connector.Error as err:
            if err.errno == -1:
                print(f"Failed to add attraction: {err}")
                return -1
            else:
                print(f"Failed to add attraction: {err}")
                return -2
        

    def remove_attraction(self, attraction_id) -> int:
        try:
            # Entries for this attraction in levels must be removed first
            self.cursor.execute("DELETE FROM user_has_level WHERE attraction_id = %s", (attraction_id,))
            self.conn.commit()
            # Entries for this attraction in category must be removed first
            self.cursor.execute("DELETE FROM attraction_has_category WHERE attraction_id = %s", (attraction_id,))
            self.conn.commit()
            # Entries for this attraction in questions must also be removed
            self.cursor.execute("DELETE FROM question WHERE attraction_id = %s", (attraction_id,))
            self.conn.commit()
            # Remove the attraction
            self.cursor.execute("DELETE FROM attraction WHERE id = %s", (attraction_id,))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            print(f"Failed to remove attraction: {err}")
            return -1

    def edit_attraction(self,attraction_id,short_information,practical_information):
        query = """UPDATE attraction SET city_id = (%s), name = (%s), practical_information = (%s),
                                short_description = (%s) WHERE id = (%s)"""
        try:
            self.cursor.execute(query, 
                                (short_information['city_id'],
                                short_information['name'],
                                json.dumps(practical_information),
                                short_information['short_description'],
                                attraction_id))
            self.conn.commit()
            #Removes the old filter category for this attraction
            self.cursor.execute("DELETE FROM attraction_has_category WHERE attraction_id = %s", (attraction_id,))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            print(f"Failed to edit attraction: {err}")
            return -1


    def get_attraction(self,attraction_id):
        try:
            self.cursor.execute("SELECT * from attraction WHERE id=(%s)", (attraction_id,))
            result = list(self.cursor.fetchone())
            if result:
                columns = [col[0] for col in self.cursor.description]
                result[3] = json.loads(result[3])
                result[1] = str(result[1])
                return dict(zip(columns, result))
            return None
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None
        
    def get_edit_attraction(self,attraction_id):
        try:
            self.cursor.execute("SELECT t1.id ,t2.name as city_name, t1.name, practical_information, short_description, level from attraction as t1 join city as t2 WHERE t1.id=%s and t2.id = city_id", (attraction_id,))
            result = list(self.cursor.fetchone())
            if result:
                columns = [col[0] for col in self.cursor.description]
                result[3] = json.loads(result[3])
                return dict(zip(columns, result))
            return None
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None

    def get_attraction_id_by_name(self,attraction_name):
        try:
            self.cursor.execute("SELECT id FROM attraction WHERE name = %s", (attraction_name,))
            result = self.cursor.fetchone()
            return result[0]
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None
        

    def fetch_city_by_name(self, city_name):
        try:
            self.cursor.execute("SELECT id, name FROM city WHERE LOWER(name) = LOWER(%s)", (city_name,))
            result = self.cursor.fetchone()
            if result:
                columns = [col[0] for col in self.cursor.description]
                return dict(zip(columns, result))
            return None
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None


    def add_city(self, city_name):
        try:
            self.cursor.execute("INSERT INTO city (name) VALUES (%s)", (city_name,))
            self.conn.commit()
            return self.cursor.lastrowid  # This returns the id of the newly inserted record
        except mysql.connector.Error as err:
            print(f"Failed to add city: {err}")
            return None

            
    def add_city(self, city_name):
        try:
            self.cursor.execute("INSERT INTO city (name) VALUES (%s)", (city_name,))
            self.conn.commit()
            return self.cursor.lastrowid  # This returns the id of the newly inserted record
        except mysql.connector.Error as err:
            print(f"Failed to add city: {err}")
            return None

    def get_category_services(self):
        query = "SELECT id,definition,icon FROM category WHERE category_type_id = (SELECT id FROM category_type WHERE type = 'Services')"
        try:
            self.cursor.execute(query)
            result = self.cursor.fetchall()
            result = [(row[0],row[1],row[2]) for row in result]
            return result
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None
        

    def get_all_categories(self):
        try:
            self.cursor.execute("SELECT t1.id,definition,t2.type, icon FROM `category`as t1 JOIN category_type as t2 WHERE t1.category_type_id = t2.id ")
            columns = [col[0] for col in self.cursor.description]
            results = [dict(zip(columns, row)) for row in self.cursor.fetchall()]
            return results
            
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None
        
    def get_attraction_has_category(self,attraction_id):
        try:
            self.cursor.execute("SELECT category_id FROM attraction_has_category WHERE attraction_id = %s",(attraction_id,))
            results = [str(row[0]) for row in self.cursor.fetchall()]
            return results
            
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None


        
    def add_filter(self,category,type_id,icon):
        try:
            self.cursor.execute("INSERT INTO `category` (`definition`,`category_type_id`,`icon`) VALUES (%s,%s,%s)", (category,type_id,icon))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            if err.errno == -1:
                print(f"Failed to add filter: {err}")
                return -1
            else:
                print(f"Failed to add filter: {err}")
                return 0
        
    def remove_filter(self,filter_id)-> int:
        try:
            self.cursor.execute("DELETE FROM category WHERE id = (%s)",(filter_id,))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            print(f"Failed to remove filter: {err}")
            return -1
        
    def edit_filter(self,id,category,type_id,icon) -> int:
        try:
            self.cursor.execute("UPDATE category SET category_type_id = (%s), definition = (%s), `icon` = (%s) WHERE id = (%s)", (type_id,category,icon,id))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            print(f"Failed to add filter: {err}")
            return -1



    def add_city(self,name) -> int:
        try:
            self.cursor.execute("INSERT INTO city (name) VALUES (%s)", (name.capitalize(),))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            if err.errno == -1:
                print(f"Failed to add_city: {err}")
                return -1
            else:
                print(f"Failed to add_city: {err}")
                return 0

    def remove_city(self,city_id) -> int:
        try:
            self.cursor.execute("DELETE FROM city WHERE id = %s",(city_id,))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            print(f"Failed to remove city: {err}")
            return -1
        



    def fetch_all_attractions(self):
        try:
            self.cursor.execute("SELECT t1.id, t2.name AS city_name, t1.name, t1.practical_information,t1.short_description FROM attraction AS t1 JOIN city AS t2 ON t1.city_id = t2.id")
            columns = [col[0] for col in self.cursor.description]
            results = [dict(zip(columns, row)) for row in self.cursor.fetchall()]
            return results
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None
    
    def fetch_attractions_by_city(self,selected_city):
        query = "SELECT * FROM attraction where city_id=%s "
        try:
            self.cursor.execute(query, (selected_city,))
            columns = [col[0] for col in self.cursor.description]
            results = [dict(zip(columns, row)) for row in self.cursor.fetchall()]
            return results
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None
        
    def fetch_attraction_name(self, attraction_id):
        query = "SELECT name FROM attraction WHERE id = %s"
        try:
            self.cursor.execute(query, (attraction_id,))
            result = self.cursor.fetchone()
            if result:
                return result[0]
            else:
                return None
        except Exception as e:
            print(f"Error fetching attraction name: {e}")
            return None
        
    def fetch_level_for_attraction(self, attraction_id, user_id):
        try:
            level_query = """
            SELECT level FROM user_has_level
            WHERE user_id = %s AND attraction_id = %s
            """
            self.cursor.execute(level_query, (user_id, attraction_id))
            return self.cursor.fetchone()[0]
        except Exception as e:
            print(f"Error fetching level for attraction {attraction_id}: {e}")
            return []        
        
    def fetch_questions_for_attraction(self, attraction_id, user_level):
        try:
            question_query = """
            SELECT id, question_text, correct_answer, option_2, option_3, option_4, option_5
            FROM question
            WHERE attraction_id = %s AND level = %s
            ORDER BY RAND() LIMIT 5
            """
            self.cursor.execute(question_query, (attraction_id, user_level))
            questions = self.cursor.fetchall()

            return [
                {
                    "id": q[0],
                    "text": q[1],
                    "correct_answer": q[2],
                    "options": [q[2], q[3], q[4], q[5], q[6]]
                } for q in questions
            ]
        except Exception as e:
            print(f"Error fetching questions for attraction {attraction_id}: {e}")
            return []
        
    def update_level(self, attraction_id, user_id):
        try:
            self.cursor.execute("UPDATE user_has_level SET level = level + 1 WHERE user_id = %s AND attraction_id = %s", (user_id, attraction_id,))
            self.conn.commit()
            return self.cursor.lastrowid
        except mysql.connector.Error as err:
            print(f"Failed to update level: {err}")
            return None
        
    def fetch_leaderboard(self):
        try:
            self.cursor.execute("SELECT username, points FROM user WHERE is_blocked = 0 ORDER BY points DESC LIMIT 5")
            result = [(index + 1, row[0], row[1]) for index, row in enumerate(self.cursor.fetchall())]
            return result
        except mysql.connector.Error as err:
            print(f"Failed fetching leaderboard: {err}")
            return None

    def get_attraction_practical_info(self, attraction_id):
        try:
            self.cursor.execute("SELECT name, practical_information FROM attraction WHERE id = %s", (attraction_id,))
            result = self.cursor.fetchone()
            if result:
                practical_info = json.loads(result[1])
                return {'name': result[0], 'practical_information': practical_info}
            else:
                return None
        except mysql.connector.Error as err:
            print(f"Database error: {err}")

    def add_categories_to_attraction(self,attraction_id,attractions,services)-> int:
        try:
            if(len(services)> 0):
                for serv in services:
                    self.cursor.execute("INSERT INTO attraction_has_category (attraction_id,category_id) VALUES (%s,%s)",(attraction_id,serv))
            if(len(attractions)> 0):
                for attr in attractions:
                    self.cursor.execute("INSERT INTO attraction_has_category (attraction_id,category_id) VALUES (%s,%s)",(attraction_id,attr))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return -1
                     

    def get_all_quiz_questions(self):
        try:
            self.cursor.execute("SELECT t1.id,t2.name as attraction, t1.attraction_id,t1.level, question_text, option_2, option_3,option_4, option_5,correct_answer FROM question as t1 JOIN attraction as t2 WHERE t1.attraction_id = t2.id")
            columns = [col[0] for col in self.cursor.description]
            results = [dict(zip(columns, row)) for row in self.cursor.fetchall()]
            return results
            
        except mysql.connector.Error as err:
            print(f"Database error: {err}")
            return None
        
    def remove_question(self,id) -> int:
        try:
            self.cursor.execute("DELETE FROM question WHERE id = (%s)",(id,))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            print(f"Failed to remove question: {err}")
            return -1

    def add_question(self,attraction_id,level,question_text,answer_1,answer_2,answer_3,answer_4,correct_answer) -> int:
        try:
            self.cursor.execute("INSERT INTO question (attraction_id,level,question_text,option_2,option_3,option_4,option_5,correct_answer) VALUES (%s,%s,%s,%s,%s,%s,%s,%s)", (attraction_id,level,question_text,answer_1,answer_2,answer_3,answer_4,correct_answer))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            if err.errno == -1:
                print(f"Failed to add question: {err}")
                return -1
            else:
                print(f"Failed to add question: {err}")
                return 0
        
    def edit_question(self,id,attraction_id,level,question_text,answer_1,answer_2,answer_3,answer_4,correct_answer):
        try:
            query = """
                        UPDATE question SET attraction_id = (%s), level = (%s), 
                        question_text = (%s), option_2 = (%s), option_3 = (%s),option_4 = (%s),
                        option_5 = (%s),correct_answer = (%s) WHERE id = (%s)
                    """
            self.cursor.execute(query,(attraction_id,level,question_text,answer_1,answer_2,answer_3,answer_4,correct_answer,id))
            self.conn.commit()
            return 1
        except mysql.connector.Error as err:
            print(f"Failed to edit question: {err}")
            return -1