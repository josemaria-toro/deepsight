"""Pruebas unitarias de las excepciones del dominio."""

import pytest

from ingestion_workers.domain.exceptions import DomainError


def test_domain_error_is_an_exception() -> None:
    with pytest.raises(DomainError, match="boom"):
        raise DomainError("boom")
