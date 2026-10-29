"""Pruebas de integración del modelo y el mapper base contra PostgreSQL."""

from collections.abc import AsyncIterator
from datetime import UTC, datetime, timedelta, timezone
from typing import Any

import pytest
from sqlalchemy import inspect
from sqlalchemy.ext.asyncio import AsyncEngine, async_sessionmaker, create_async_engine

from deepsight.infrastructure.persistence.base import Base
from deepsight.infrastructure.persistence.mappers import (
    observability_data_from_model_fields,
    observability_data_to_model_fields,
)
from deepsight.infrastructure.persistence.models import ObservabilityDataModel
from tests.infrastructure.persistence.example_data import ExampleData
from tests.infrastructure.persistence.example_data_model import ExampleDataModel

pytestmark = pytest.mark.integration

VALID: dict[str, Any] = {
    "app_name": "shop",
    "app_version": "1.0.0",
    "span_id": "span-1",
    "timespan": datetime(2026, 1, 1, 10, tzinfo=timezone(timedelta(hours=1))),
    "trace_id": "trace-1",
}


@pytest.fixture
async def engine(database_url: str) -> AsyncIterator[AsyncEngine]:
    engine = create_async_engine(database_url)
    async with engine.begin() as connection:
        await connection.run_sync(Base.metadata.create_all)
    yield engine
    async with engine.begin() as connection:
        await connection.run_sync(Base.metadata.drop_all)
    await engine.dispose()


async def test_schema_is_created_with_expected_columns(engine: AsyncEngine) -> None:
    async with engine.connect() as connection:
        columns = await connection.run_sync(
            lambda sync: {c["name"]: c for c in inspect(sync).get_columns("observability_data")}
        )

    assert set(columns) == {
        "id",
        "type",
        "app_name",
        "app_version",
        "span_id",
        "timespan",
        "trace_id",
        "client_ip_address",
        "client_version",
        "extra_data",
        "host_name",
    }
    assert columns["app_name"]["type"].length == 255
    assert columns["trace_id"]["type"].length == 64
    assert columns["client_ip_address"]["type"].length == 45
    assert columns["timespan"]["type"].timezone is True
    assert columns["app_name"]["nullable"] is False
    assert columns["host_name"]["nullable"] is True


async def test_persisted_object_is_recovered_with_all_base_properties(
    engine: AsyncEngine,
) -> None:
    entity = ExampleData(
        **VALID,
        client_ip_address="10.0.0.1",
        client_version="2.0",
        extra_data={"nested": {"a": [1, 2]}, "flag": True},
        host_name="host",
    )
    sessions = async_sessionmaker(engine, expire_on_commit=False)

    async with sessions.begin() as session:
        session.add(ExampleDataModel(**observability_data_to_model_fields(entity)))
    async with sessions() as session:
        model = await session.get(ObservabilityDataModel, entity.id)

    assert isinstance(model, ExampleDataModel)
    assert model.type == "example"
    recovered = ExampleData(**observability_data_from_model_fields(model))
    assert recovered == entity
    assert recovered.timespan == datetime(2026, 1, 1, 9, tzinfo=UTC)


async def test_optional_properties_are_stored_as_null(engine: AsyncEngine) -> None:
    entity = ExampleData(**VALID)
    sessions = async_sessionmaker(engine, expire_on_commit=False)

    async with sessions.begin() as session:
        session.add(ExampleDataModel(**observability_data_to_model_fields(entity)))
    async with sessions() as session:
        model = await session.get(ObservabilityDataModel, entity.id)

    assert model is not None
    assert ExampleData(**observability_data_from_model_fields(model)) == entity
