"""Pruebas unitarias del DTO base de observabilidad."""

from dataclasses import dataclass
from datetime import UTC, datetime
from typing import Any

import pytest

from deepsight.application.dtos.observability_data_dto import ObservabilityDataDto

VALID: dict[str, Any] = {
    "app_name": "shop",
    "app_version": "1.0.0",
    "span_id": "span-1",
    "timespan": datetime(2026, 1, 1, tzinfo=UTC),
    "trace_id": "trace-1",
}


def test_keeps_all_base_properties() -> None:
    dto = ObservabilityDataDto(
        **VALID,
        client_ip_address="10.0.0.1",
        client_version="2.0",
        extra_data={"a": {"b": 1}},
        host_name="host",
    )

    assert dto.app_name == "shop"
    assert dto.extra_data == {"a": {"b": 1}}
    assert dto.host_name == "host"


def test_optional_properties_default_to_none() -> None:
    dto = ObservabilityDataDto(**VALID)

    assert dto.client_ip_address is None
    assert dto.client_version is None
    assert dto.extra_data is None
    assert dto.host_name is None


@pytest.mark.parametrize("name", ["app_name", "app_version", "span_id", "trace_id"])
def test_mandatory_text_cannot_be_empty(name: str) -> None:
    with pytest.raises(ValueError, match=name):
        ObservabilityDataDto(**{**VALID, name: ""})


def test_timespan_requires_time_zone() -> None:
    with pytest.raises(ValueError, match="timespan"):
        ObservabilityDataDto(**{**VALID, "timespan": datetime(2026, 1, 1)})


def test_text_cannot_exceed_maximum_length() -> None:
    with pytest.raises(ValueError, match="app_name"):
        ObservabilityDataDto(**{**VALID, "app_name": "a" * 256})


def test_optional_text_cannot_exceed_maximum_length() -> None:
    with pytest.raises(ValueError, match="client_ip_address"):
        ObservabilityDataDto(**VALID, client_ip_address="1" * 46)


def test_text_at_the_limit_is_accepted() -> None:
    assert ObservabilityDataDto(**{**VALID, "app_name": "a" * 255}).app_name == "a" * 255


def test_concrete_dto_inherits_the_base() -> None:
    @dataclass(frozen=True, kw_only=True)
    class ExampleDto(ObservabilityDataDto):
        name: str

    dto = ExampleDto(**VALID, name="x")

    assert dto.name == "x"
    with pytest.raises(ValueError, match="span_id"):
        ExampleDto(**{**VALID, "span_id": ""}, name="x")
