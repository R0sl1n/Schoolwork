"""empty message

Revision ID: c7e9af21a8da
Revises: d2e9619ffae1
Create Date: 2023-05-25 17:57:20.607830

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'c7e9af21a8da'
down_revision = 'd2e9619ffae1'
branch_labels = None
depends_on = None


def upgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('quiz__question', schema=None) as batch_op:
        batch_op.add_column(sa.Column('alt7', sa.String(length=200), nullable=True))

    # ### end Alembic commands ###


def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    with op.batch_alter_table('quiz__question', schema=None) as batch_op:
        batch_op.drop_column('alt7')

    # ### end Alembic commands ###
