"""empty message

Revision ID: d2e9619ffae1
Revises: 
Create Date: 2023-05-25 16:29:05.401335

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'd2e9619ffae1'
down_revision = None
branch_labels = None
depends_on = None


def upgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('quiz__question', schema=None) as batch_op:
        batch_op.create_foreign_key(None, 'type', ['type_id'], ['id'])
        batch_op.create_foreign_key(None, 'category', ['category_id'], ['id'])

    with op.batch_alter_table('quiz__response', schema=None) as batch_op:
        batch_op.create_foreign_key(None, 'quiz__question', ['id_qst'], ['id'])
        batch_op.create_foreign_key(None, 'user', ['id_user'], ['id'])

    # ### end Alembic commands ###


def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('quiz__response', schema=None) as batch_op:
        batch_op.drop_constraint(None, type_='foreignkey')
        batch_op.drop_constraint(None, type_='foreignkey')

    with op.batch_alter_table('quiz__question', schema=None) as batch_op:
        batch_op.drop_constraint(None, type_='foreignkey')
        batch_op.drop_constraint(None, type_='foreignkey')

    # ### end Alembic commands ###