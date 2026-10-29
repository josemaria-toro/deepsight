"""Crea la tabla base observability_data.

Revision ID: 0001
Revises:
"""

import sqlalchemy as sa
from alembic import op
from sqlalchemy.dialects import postgresql

revision = "0001"
down_revision = None
branch_labels = None
depends_on = None


def upgrade() -> None:
    op.create_table(
        "observability_data",
        sa.Column("id", sa.Uuid(), nullable=False),
        sa.Column("type", sa.String(length=64), nullable=False),
        sa.Column("app_name", sa.String(length=255), nullable=False),
        sa.Column("app_version", sa.String(length=64), nullable=False),
        sa.Column("span_id", sa.String(length=64), nullable=False),
        sa.Column("timespan", sa.DateTime(timezone=True), nullable=False),
        sa.Column("trace_id", sa.String(length=64), nullable=False),
        sa.Column("client_ip_address", sa.String(length=45), nullable=True),
        sa.Column("client_version", sa.String(length=64), nullable=True),
        sa.Column("extra_data", postgresql.JSONB(), nullable=True),
        sa.Column("host_name", sa.String(length=255), nullable=True),
        sa.PrimaryKeyConstraint("id"),
    )
    op.create_index("ix_observability_data_type", "observability_data", ["type"])
    op.create_index("ix_observability_data_trace_id", "observability_data", ["trace_id"])


def downgrade() -> None:
    op.drop_index("ix_observability_data_trace_id", table_name="observability_data")
    op.drop_index("ix_observability_data_type", table_name="observability_data")
    op.drop_table("observability_data")
