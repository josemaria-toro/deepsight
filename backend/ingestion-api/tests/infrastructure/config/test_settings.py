"""Pruebas unitarias de `Settings`."""

import pytest

from deepsight_ingestion_api.infrastructure.config.settings import Settings

VALID_ENV = {"RABBITMQ_URL": "value-rabbitmq_url"}


def test_from_env_reads_all_variables() -> None:
    settings = Settings.from_env(VALID_ENV)

    assert settings.rabbitmq_url == VALID_ENV["RABBITMQ_URL"]


@pytest.mark.parametrize("missing", list(VALID_ENV))
def test_from_env_fails_when_variable_is_missing(missing: str) -> None:
    environ = {key: value for key, value in VALID_ENV.items() if key != missing}

    with pytest.raises(ValueError, match=missing):
        Settings.from_env(environ)


def test_from_env_fails_when_variable_is_empty() -> None:
    environ = {**VALID_ENV, "RABBITMQ_URL": ""}

    with pytest.raises(ValueError, match="RABBITMQ_URL"):
        Settings.from_env(environ)
