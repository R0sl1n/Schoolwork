"""empty message

Revision ID: 621811fce6a5
Revises: 
Create Date: 2023-05-31 09:45:14.008581

"""
from alembic import op
import sqlalchemy as sa
from sqlalchemy.dialects import mysql

# revision identifiers, used by Alembic.
revision = '621811fce6a5'
down_revision = None
branch_labels = None
depends_on = None


def upgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    op.drop_table('quiz')
    with op.batch_alter_table('quiz_question', schema=None) as batch_op:
        batch_op.alter_column('type_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=True)
        batch_op.alter_column('category_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=True)

    with op.batch_alter_table('quiz_response', schema=None) as batch_op:
        batch_op.create_foreign_key(None, 'user', ['id_user'], ['id'])
        batch_op.create_foreign_key(None, 'quiz_question', ['id_qst'], ['id'])
        batch_op.drop_column('id_quiz')

    # ### end Alembic commands ###


def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('quiz_response', schema=None) as batch_op:
        batch_op.add_column(sa.Column('id_quiz', mysql.INTEGER(display_width=11), autoincrement=False, nullable=False))
        batch_op.drop_constraint(None, type_='foreignkey')
        batch_op.drop_constraint(None, type_='foreignkey')

    with op.batch_alter_table('quiz_question', schema=None) as batch_op:
        batch_op.alter_column('category_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=False)
        batch_op.alter_column('type_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=False)

    op.create_table('quiz',
    sa.Column('id', mysql.INTEGER(display_width=11), autoincrement=True, nullable=False),
    sa.Column('id_user', mysql.VARCHAR(length=50), nullable=True),
    sa.PrimaryKeyConstraint('id'),
    mysql_collate='utf8mb4_general_ci',
    mysql_default_charset='utf8mb4',
    mysql_engine='InnoDB'
    )
    # ### end Alembic commands ###
