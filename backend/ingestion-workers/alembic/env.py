"""Entorno de migraciones de Alembic, en modo asíncrono."""

import asyncio
import os

from alembic import context
from sqlalchemy import Connection
from sqlalchemy.ext.asyncio import create_async_engine

from deepsight.infrastructure.persistence import models  # noqa: F401
from deepsight.infrastructure.persistence.base import Base

config = context.config
target_metadata = Base.metadata


def _database_url() -> str:
    url = config.get_main_option("sqlalchemy.url") or os.environ.get("DATABASE_URL")
    if not url:
        raise RuntimeError("Missing database URL: set DATABASE_URL")
    return url


def _run_migrations(connection: Connection) -> None:
    context.configure(connection=connection, target_metadata=target_metadata)
    with context.begin_transaction():
        context.run_migrations()


async def _run_async_migrations() -> None:
    engine = create_async_engine(_database_url())
    async with engine.connect() as connection:
        await connection.run_sync(_run_migrations)
    await engine.dispose()


def run_migrations_online() -> None:
    """Ejecuta las migraciones contra la base de datos."""
    asyncio.run(_run_async_migrations())


run_migrations_online()
