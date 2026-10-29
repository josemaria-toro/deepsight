"""Pruebas del contrato HTTP del endpoint de salud."""

from fastapi.testclient import TestClient

from deepsight.interfaces.api.app import create_app


def test_health_returns_ok() -> None:
    client = TestClient(create_app())

    response = client.get("/api/v1/health")

    assert response.status_code == 200
    assert response.json() == {"status": "ok"}


def test_unversioned_health_is_not_exposed() -> None:
    client = TestClient(create_app())

    assert client.get("/health").status_code == 404
