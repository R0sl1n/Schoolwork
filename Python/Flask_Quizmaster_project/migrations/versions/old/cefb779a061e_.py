"""empty message

Revision ID: cefb779a061e
Revises: 9822e0c627a2
Create Date: 2023-05-30 20:07:38.940735

"""
from alembic import op
import sqlalchemy as sa
from sqlalchemy.dialects import mysql

# revision identifiers, used by Alembic.
revision = 'cefb779a061e'
down_revision = '9822e0c627a2'
branch_labels = None
depends_on = None


def upgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('quiz_question', schema=None) as batch_op:
        batch_op.alter_column('type_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=True)
        batch_op.alter_column('category_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=True)

    with op.batch_alter_table('quiz_response', schema=None) as batch_op:
        batch_op.add_column(sa.Column('id', sa.Integer(), autoincrement=True, nullable=False))

    # ### end Alembic commands ###


def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('quiz_response', schema=None) as batch_op:
        batch_op.drop_column('id')

    with op.batch_alter_table('quiz_question', schema=None) as batch_op:
        batch_op.alter_column('category_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=False)
        batch_op.alter_column('type_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=False)

    # ### end Alembic commands ###
