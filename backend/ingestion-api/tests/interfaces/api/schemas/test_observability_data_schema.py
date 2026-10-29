"""Pruebas del contrato JSON del esquema base de observabilidad."""

from datetime import UTC, datetime
from typing import Any

import pytest
from pydantic import ValidationError

from deepsight.interfaces.api.schemas.observability_data_schema import ObservabilityDataSchema

VALID: dict[str, Any] = {
    "appName": "shop",
    "appVersion": "1.0.0",
    "spanId": "span-1",
    "timespan": "2026-01-01T10:00:00+01:00",
    "traceId": "trace-1",
}

FULL: dict[str, Any] = {
    **VALID,
    "clientIpAddress": "10.0.0.1",
    "clientVersion": "2.0",
    "extraData": {"nested": {"a": [1, 2]}},
    "hostName": "host",
}


def test_accepts_all_base_properties() -> None:
    schema = ObservabilityDataSchema.model_validate(FULL)

    assert schema.app_name == "shop"
    assert schema.client_ip_address == "10.0.0.1"
    assert schema.extra_data == {"nested": {"a": [1, 2]}}
    assert schema.host_name == "host"


def test_optional_properties_may_be_omitted_or_null() -> None:
    omitted = ObservabilityDataSchema.model_validate(VALID)
    nulls = ObservabilityDataSchema.model_validate(
        {
            **VALID,
            "clientIpAddress": None,
            "clientVersion": None,
            "extraData": None,
            "hostName": None,
        }
    )

    for schema in (omitted, nulls):
        assert schema.client_ip_address is None
        assert schema.client_version is None
        assert schema.extra_data is None
        assert schema.host_name is None


@pytest.mark.parametrize("name", ["appName", "appVersion", "spanId", "timespan", "traceId"])
def test_mandatory_property_is_required(name: str) -> None:
    payload = {key: value for key, value in VALID.items() if key != name}

    with pytest.raises(ValidationError, match=name):
        ObservabilityDataSchema.model_validate(payload)


@pytest.mark.parametrize("name", ["appName", "appVersion", "spanId", "timespan", "traceId"])
def test_mandatory_property_cannot_be_null(name: str) -> None:
    with pytest.raises(ValidationError, match=name):
        ObservabilityDataSchema.model_validate({**VALID, name: None})


@pytest.mark.parametrize("name", ["appName", "appVersion", "spanId", "traceId"])
def test_mandatory_text_cannot_be_empty(name: str) -> None:
    with pytest.raises(ValidationError, match=name):
        ObservabilityDataSchema.model_validate({**VALID, name: ""})


def test_timespan_keeps_the_instant() -> None:
    schema = ObservabilityDataSchema.model_validate(VALID)

    assert schema.timespan == datetime(2026, 1, 1, 9, 0, tzinfo=UTC)


@pytest.mark.parametrize("value", ["2026-01-01T10:00:00", "yesterday", "01/01/2026"])
def test_timespan_requires_iso_8601_with_time_zone(value: str) -> None:
    with pytest.raises(ValidationError, match="timespan"):
        ObservabilityDataSchema.model_validate({**VALID, "timespan": value})


@pytest.mark.parametrize("value", ["text", 1, ["a"]])
def test_extra_data_must_be_a_dictionary(value: Any) -> None:
    with pytest.raises(ValidationError, match="extraData"):
        ObservabilityDataSchema.model_validate({**VALID, "extraData": value})


@pytest.mark.parametrize(
    ("name", "limit"),
    [
        ("appName", 255),
        ("appVersion", 64),
        ("spanId", 64),
        ("traceId", 64),
        ("clientIpAddress", 45),
        ("clientVersion", 64),
        ("hostName", 255),
    ],
)
def test_text_length_limits(name: str, limit: int) -> None:
    assert ObservabilityDataSchema.model_validate({**VALID, name: "a" * limit})

    with pytest.raises(ValidationError, match=name):
        ObservabilityDataSchema.model_validate({**VALID, name: "a" * (limit + 1)})


def test_serializes_in_camel_case() -> None:
    dumped = ObservabilityDataSchema.model_validate(FULL).model_dump(by_alias=True, mode="json")

    assert set(dumped) == set(FULL)
    assert datetime.fromisoformat(dumped["timespan"]) == datetime(2026, 1, 1, 9, tzinfo=UTC)


def test_snake_case_is_rejected() -> None:
    snake = {
        "app_name": "shop",
        "app_version": "1.0.0",
        "span_id": "span-1",
        "timespan": "2026-01-01T10:00:00+01:00",
        "trace_id": "trace-1",
    }

    with pytest.raises(ValidationError, match="appName"):
        ObservabilityDataSchema.model_validate(snake)


def test_concrete_schema_inherits_the_base() -> None:
    class ExampleSchema(ObservabilityDataSchema):
        name: str

    schema = ExampleSchema.model_validate({**VALID, "name": "x"})

    assert schema.name == "x"
    assert schema.app_name == "shop"
    with pytest.raises(ValidationError, match="spanId"):
        ExampleSchema.model_validate(
            {"name": "x", **{k: v for k, v in VALID.items() if k != "spanId"}}
        )
