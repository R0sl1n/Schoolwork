from flask import flash, request, redirect, render_template, url_for, Blueprint
from flask_login import current_user, login_required
from forms import AddFriendForm, DeleteFriendForm, RedeemRewardForm
from userregistry import UserReg

profile_bp = Blueprint("profile",__name__,template_folder="templates")

@profile_bp.route('/profile', methods = ["GET", "POST"])
@login_required
def profile():
    user = current_user
    user.check_for_achievements()
    achievements = user.get_achievements()
    rewards_info = user.get_user_rewards()
    user_rewards_count = user.count_user_rewards()
    form = RedeemRewardForm()
    user_has_visited_attractions = user.has_visited_attractions()

    
    return render_template('profile.html', user=user, achievements=achievements, rewards_info=rewards_info, user_rewards_count = user_rewards_count, form=form, user_has_visited_attractions=user_has_visited_attractions)

@profile_bp.route('/redeem-reward', methods=["POST"])
@login_required
def redeem_reward():
    form = RedeemRewardForm()
    if form.validate_on_submit():
        reward_id = request.form.get('id')
        if reward_id:
            try:
                current_user.redeem_reward(reward_id)
                flash('Reward redeemed successfully.', 'success')
            except Exception as e:
                flash(f'An error occurred: {e}', 'error')
        else:
            flash('Error: No reward ID found.', 'error')
    else:
        flash('Error processing your request.', 'error')
    return redirect(url_for('profile.profile'))

@profile_bp.route('/friends', methods=['GET', 'POST'])
@login_required
def friends():
    form_add_friend = AddFriendForm()
    form_delete_friend = DeleteFriendForm()
    
    user_id = current_user.get_id()  

    if request.method == 'POST':
        if 'add_friend' in request.form:
            if form_add_friend.validate_on_submit():
                friend_email = form_add_friend.friend_email.data
                with UserReg() as db:
                    friend_user = db.fetch_email(friend_email)
                    if friend_user:
                        db.add_friend(user_id, friend_user[0])  
                        flash('Friend added successfully.')
                    else:
                        flash('No user with that email.')
        
        elif 'delete_friend' in request.form:
            if form_delete_friend.validate_on_submit():
                friend_id = form_delete_friend.friend_id.data
                with UserReg() as db:
                    db.remove_friend(user_id, friend_id)  
                    flash('Friend removed successfully.')

    with UserReg() as db:
        all_friends_data = db.get_friends(user_id)
        friends = [{'id': data[0], 'username': data[1], 'email': data[2], 'points': data[3]} for data in all_friends_data]

    return render_template('friends.html', friends=friends, form_add_friend=form_add_friend, form_delete_friend=form_delete_friend)


@profile_bp.route('/add-friend', methods=['POST'])
@login_required
def add_friend():
    form = AddFriendForm()
    if form.validate_on_submit():
        friend_email = form.friend_email.data
        with UserReg() as db:
            friend_user = db.fetch_email(friend_email)
            if friend_user:
                if db.is_friend(current_user.get_id(), friend_user[0]):
                    flash('This user is already your friend.', 'error')
                else:
                    db.add_friend(current_user.get_id(), friend_user[0])
                    flash('Friend added successfully.', 'success')
            else:
                flash('No user with that email found.', 'error')
    return redirect(url_for('profile.friends'))


@profile_bp.route('/delete-friend/<int:friend_id>', methods=['POST'])
@login_required
def delete_friend(friend_id):
    try:
        with UserReg() as db:
            db.remove_friend(current_user.get_id(), friend_id)
            flash('Friend removed successfully.', 'success')
    except Exception as e:
        flash(f'An error occurred: {e}', 'error')
    return redirect(url_for('profile.friends'))
