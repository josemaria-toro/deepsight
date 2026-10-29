"""Pruebas unitarias de la entidad base de observabilidad."""

from datetime import UTC, datetime
from typing import Any
from uuid import UUID

import pytest

from deepsight.domain.entities.observability_data import ObservabilityData
from deepsight.domain.exceptions import DomainError


class ExampleData(ObservabilityData):
    """Tipo concreto mínimo para las pruebas."""

    @classmethod
    def type_name(cls) -> str:
        return "example"


VALID: dict[str, Any] = {
    "app_name": "shop",
    "app_version": "1.0.0",
    "span_id": "span-1",
    "timespan": datetime(2026, 1, 1, tzinfo=UTC),
    "trace_id": "trace-1",
}


def test_base_entity_cannot_be_instantiated() -> None:
    with pytest.raises(TypeError):
        ObservabilityData(**VALID)  # type: ignore[abstract]


def test_concrete_type_keeps_all_base_properties() -> None:
    entity = ExampleData(
        **VALID,
        client_ip_address="10.0.0.1",
        client_version="2.0",
        extra_data={"a": {"b": 1}},
        host_name="host",
    )

    assert entity.app_name == "shop"
    assert entity.extra_data == {"a": {"b": 1}}
    assert entity.host_name == "host"
    assert entity.type_name() == "example"
    assert isinstance(entity.id, UUID)


def test_optional_properties_default_to_none() -> None:
    entity = ExampleData(**VALID)

    assert entity.client_ip_address is None
    assert entity.client_version is None
    assert entity.extra_data is None
    assert entity.host_name is None


def test_each_entity_gets_its_own_identity() -> None:
    assert ExampleData(**VALID).id != ExampleData(**VALID).id


@pytest.mark.parametrize("name", ["app_name", "app_version", "span_id", "trace_id"])
def test_mandatory_text_cannot_be_empty(name: str) -> None:
    with pytest.raises(DomainError, match=name):
        ExampleData(**{**VALID, name: ""})


def test_timespan_requires_time_zone() -> None:
    with pytest.raises(DomainError, match="timespan"):
        ExampleData(**{**VALID, "timespan": datetime(2026, 1, 1)})


@pytest.mark.parametrize(
    ("name", "limit"),
    [
        ("app_name", 255),
        ("app_version", 64),
        ("span_id", 64),
        ("trace_id", 64),
        ("client_ip_address", 45),
        ("client_version", 64),
        ("host_name", 255),
    ],
)
def test_text_length_limits(name: str, limit: int) -> None:
    assert ExampleData(**{**VALID, name: "a" * limit})

    with pytest.raises(DomainError, match=name):
        ExampleData(**{**VALID, name: "a" * (limit + 1)})
