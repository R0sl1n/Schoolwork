#This script is mostly the works of the author of this webpage: https://blog.miguelgrinberg.com/post/the-flask-mega-tutorial-part-i-hello-world
#The addition to remove FKs with upgrade/downgrade has been added to it in regards of functionality.

import logging
from logging.config import fileConfig
from flask import current_app
from alembic import context, op
import sqlalchemy as sa

# this is the Alembic Config object, which provides
# access to the values within the .ini file in use.
config = context.config

# Interpret the config file for Python logging.
# This line sets up loggers basically.
fileConfig(config.config_file_name)
logger = logging.getLogger('alembic.env')


def get_engine():
    try:
        # this works with Flask-SQLAlchemy<3 and Alchemical
        return current_app.extensions['migrate'].db.get_engine()
    except TypeError:
        # this works with Flask-SQLAlchemy>=3
        return current_app.extensions['migrate'].db.engine


def get_engine_url():
    try:
        return get_engine().url.render_as_string(hide_password=False).replace(
            '%', '%%')
    except AttributeError:
        return str(get_engine().url).replace('%', '%%')



config.set_main_option('sqlalchemy.url', get_engine_url())
target_db = current_app.extensions['migrate'].db


#Removing foreign keys so that the upgrade made be done without errors, and reapplying afterwards.
def upgrade():
    with op.batch_alter_table('user') as batch_op:
        batch_op.drop_constraint('fk_quiz_response_id_user', type_='foreignkey')
    
    with op.batch_alter_table('quiz_response') as batch_op:
        batch_op.drop_constraint('fk_quiz_response_id_user', type_='foreignkey')
        batch_op.create_foreign_key('fk_quiz_response_id_user', 'user', ['id_user'], ['id'])


def downgrade():
    with op.batch_alter_table('user') as batch_op:
        batch_op.create_foreign_key('fk_quiz_response_id_user', 'user', 'quiz_response', ['id_user'], ['id'])
    
    with op.batch_alter_table('quiz_response') as batch_op:
        batch_op.create_foreign_key('fk_quiz_response_id_user', 'user', ['id_user'], ['id'])


def get_metadata():
    if hasattr(target_db, 'metadatas'):
        return target_db.metadatas[None]
    return target_db.metadata


def run_migrations_offline():
    """Run migrations in 'offline' mode.

    This configures the context with just a URL
    and not an Engine, though an Engine is acceptable
    here as well.  By skipping the Engine creation
    we don't even need a DBAPI to be available.

    Calls to context.execute() here emit the given string to the
    script output.

    """
    url = config.get_main_option("sqlalchemy.url")
    context.configure(
        url=url, target_metadata=get_metadata(), literal_binds=True
    )

    with context.begin_transaction():
        context.run_migrations()


def run_migrations_online():
    """Run migrations in 'online' mode.

    In this scenario we need to create an Engine
    and associate a connection with the context.

    """

    # this callback is used to prevent an auto-migration from being generated
    # when there are no changes to the schema
    # reference: http://alembic.zzzcomputing.com/en/latest/cookbook.html
    def process_revision_directives(context, revision, directives):
        if getattr(config.cmd_opts, 'autogenerate', False):
            script = directives[0]
            if script.upgrade_ops.is_empty():
                directives[:] = []
                logger.info('No changes in schema detected.')

    connectable = get_engine()

    with connectable.connect() as connection:
        context.configure(
            connection=connection,
            target_metadata=get_metadata(),
            process_revision_directives=process_revision_directives,
            **current_app.extensions['migrate'].configure_args
        )

        with context.begin_transaction():
            context.run_migrations()


if context.is_offline_mode():
    run_migrations_offline()
else:
    run_migrations_online()
