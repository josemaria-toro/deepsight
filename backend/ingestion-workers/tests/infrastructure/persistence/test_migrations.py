"""Pruebas de integración de las migraciones de Alembic contra PostgreSQL."""

import asyncio
from collections.abc import Iterator
from pathlib import Path

import pytest
from alembic import command
from alembic.config import Config
from sqlalchemy import inspect
from sqlalchemy.ext.asyncio import create_async_engine

from deepsight.infrastructure.persistence.base import Base

pytestmark = pytest.mark.integration

PROJECT_ROOT = Path(__file__).resolve().parents[3]


@pytest.fixture
def alembic_config(database_url: str) -> Iterator[Config]:
    config = Config(str(PROJECT_ROOT / "alembic.ini"))
    config.set_main_option("script_location", str(PROJECT_ROOT / "alembic"))
    config.set_main_option("sqlalchemy.url", database_url.replace("%", "%%"))
    yield config


async def _table_names(database_url: str) -> set[str]:
    engine = create_async_engine(database_url)
    async with engine.connect() as connection:
        names = await connection.run_sync(lambda sync: set(inspect(sync).get_table_names()))
    await engine.dispose()
    return names


async def test_upgrade_and_downgrade(alembic_config: Config, database_url: str) -> None:
    await asyncio.to_thread(command.upgrade, alembic_config, "head")
    assert "observability_data" in await _table_names(database_url)

    await asyncio.to_thread(command.downgrade, alembic_config, "base")
    assert "observability_data" not in await _table_names(database_url)


async def test_migration_matches_the_model(alembic_config: Config, database_url: str) -> None:
    await asyncio.to_thread(command.upgrade, alembic_config, "head")
    engine = create_async_engine(database_url)
    async with engine.connect() as connection:
        columns = await connection.run_sync(
            lambda sync: {c["name"] for c in inspect(sync).get_columns("observability_data")}
        )
    await engine.dispose()
    await asyncio.to_thread(command.downgrade, alembic_config, "base")

    assert columns == set(Base.metadata.tables["observability_data"].columns.keys())
