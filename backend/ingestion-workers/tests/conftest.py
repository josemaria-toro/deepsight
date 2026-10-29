"""Fixtures compartidas: un PostgreSQL efímero en Docker para las pruebas de integración."""

import subprocess
import time
from collections.abc import Iterator

import pytest

POSTGRES_IMAGE = "postgres:17-alpine"


def _docker(*args: str) -> str:
    result = subprocess.run(  # noqa: S603
        ["docker", *args],  # noqa: S607
        check=True,
        capture_output=True,
        text=True,
    )
    return result.stdout.strip()


@pytest.fixture(scope="session")
def database_url() -> Iterator[str]:
    """Arranca un contenedor de PostgreSQL que se elimina al terminar la sesión."""
    container = _docker(
        "run",
        "--rm",
        "--detach",
        "--env",
        "POSTGRES_HOST_AUTH_METHOD=trust",
        "--publish",
        "127.0.0.1::5432",
        POSTGRES_IMAGE,
    )
    try:
        port = _docker("port", container, "5432/tcp").splitlines()[0].rsplit(":", 1)[1]
        for _ in range(60):
            ready = subprocess.run(  # noqa: S603
                ["docker", "exec", container, "pg_isready", "-U", "postgres"],  # noqa: S607
                capture_output=True,
                check=False,
            )
            if ready.returncode == 0:
                break
            time.sleep(1)
        else:
            raise RuntimeError("PostgreSQL did not become ready in time")
        # `pg_isready` responde antes de acabar el arranque inicial: se espera un poco más.
        time.sleep(2)
        yield f"postgresql+asyncpg://postgres@127.0.0.1:{port}/postgres"
    finally:
        _docker("stop", container)
