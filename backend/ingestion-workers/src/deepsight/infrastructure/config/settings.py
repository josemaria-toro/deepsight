"""Configuración de ingestion-workers leída desde variables de entorno."""

from collections.abc import Mapping
from dataclasses import dataclass


@dataclass(frozen=True)
class Settings:
    """Configuración inmutable de la aplicación.

    Attributes:
        rabbitmq_url: URL de conexión con RabbitMQ.
        database_url: URL de conexión con PostgreSQL (driver `asyncpg`).
    """

    rabbitmq_url: str
    database_url: str

    @classmethod
    def from_env(cls, environ: Mapping[str, str]) -> "Settings":
        """Construye la configuración a partir de las variables de entorno.

        Args:
            environ: Mapa de variables de entorno, normalmente `os.environ`.

        Returns:
            La configuración validada.

        Raises:
            ValueError: Si falta alguna variable obligatoria.
        """
        return cls(
            rabbitmq_url=cls._read(environ, "RABBITMQ_URL"),
            database_url=cls._read(environ, "DATABASE_URL"),
        )

    @staticmethod
    def _read(environ: Mapping[str, str], name: str) -> str:
        value = environ.get(name)
        if not value:
            raise ValueError(f"Missing required environment variable: {name}")
        return value
