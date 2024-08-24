"""empty message

Revision ID: 9a25c52ecddc
Revises: 
Create Date: 2023-05-31 11:00:01.679310

"""
from alembic import op
import sqlalchemy as sa
from sqlalchemy.dialects import mysql

# revision identifiers, used by Alembic.
revision = '9a25c52ecddc'
down_revision = None
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

    # ### end Alembic commands ###


def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('quiz_question', schema=None) as batch_op:
        batch_op.alter_column('category_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=False)
        batch_op.alter_column('type_id',
               existing_type=mysql.INTEGER(display_width=11),
               nullable=False)

    # ### end Alembic commands ###
