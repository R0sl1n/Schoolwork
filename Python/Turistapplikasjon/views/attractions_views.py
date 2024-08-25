
from flask import flash, request, redirect, render_template, url_for, session,Blueprint
from flask_login import current_user, login_required
from userregistry import UserReg
from turistdb import TuristDb

attractions_bp = Blueprint("attractions",__name__,template_folder="templates")

@attractions_bp.route('/attractions',methods=["GET", "POST"])
@login_required
def attractions():
    with UserReg() as db:
        # FEATURE: Lives 
        LOCK_OUT_HOURS = 7
        MAX_LIVES = 3
        from datetime import datetime, timedelta
        lives = db.fetch_lives_for_user(current_user.user_id)
        ts_now = datetime.now()
        ts_locked: datetime = db.fetch_locked_timestamp(current_user.user_id)
        duration: timedelta = (ts_now - ts_locked)
        ts_unlocked = ts_locked + timedelta(hours=LOCK_OUT_HOURS)
        # check to see if enough time has passed since lockout
        if lives < 1:
            if (duration.total_seconds() > LOCK_OUT_HOURS * 3600):
                # reset lives
                db.update_lives(current_user.user_id, MAX_LIVES)
                lives = MAX_LIVES
            else:
                flash("Quiz timeout: You've lost all your lives", 'lives_update')
                flash(f"Timeout until: {str(ts_unlocked)}", 'lives_update')
    with TuristDb() as db:
        # FEATURE: Categories/Services
        attraction_category = db.get_category_attractions()
        attractions_services = db.get_category_services()
        cities = db.fetch_all_cities()
        
        # Default city Oslo
        selected_city = 1
        
        if request.method == "POST":
            list_attractions = request.form.getlist("attractions")
            list_services = request.form.getlist("services")
            
            # Check if the selected city is present in the form data
            selected_city_form = request.form.get("selected_city")
            if selected_city_form:
                selected_city = int(selected_city_form)
            else:
                # Use the last chosen city if not present in the form data
                selected_city = session.get('selected_city', 1)

            # Apply filtering
            if len(list_attractions) == 0 and len(list_services) == 0:
                # Fetch attractions for the selected city if no filtration applied
                attractions = db.fetch_attractions_by_city(selected_city)
            else:
                attractions = db.fetch_filtered_attractions(list_attractions, list_services, selected_city)

            # Update selected city in session
            session['selected_city'] = selected_city

        else:
            # Check if selected city is in session
            selected_city = session.get('selected_city', 1)

            # Fetch attractions for the selected city
            attractions = db.fetch_attractions_by_city(selected_city)
        
        # Fetch levels and add them to the result
        for attraction in attractions:
            attraction['level'] = db.fetch_level_for_attraction(attraction['id'], current_user.user_id)

        return render_template('attractions.html', lives=lives, attractions=attractions, categories=attraction_category, services=attractions_services, cities=cities, selected_city=selected_city)
       

@attractions_bp.route('/attraction/<int:attraction_id>/info')
def attraction_info(attraction_id):
    with TuristDb() as db:
        attraction_info = db.get_attraction_practical_info(attraction_id)
        if attraction_info:
            return render_template('attraction_info.html', info=attraction_info)
        else:
            flash('attraction not found.', 'error')
            return redirect(url_for('attractions.attractions'))