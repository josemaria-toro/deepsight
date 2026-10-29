"""Ejemplo: carga de la configuración desde variables de entorno."""

import logging
import os

from ingestion_workers.infrastructure.config.settings import Settings


def main() -> None:
    """Carga y muestra la configuración (sin valores sensibles)."""
    settings = Settings.from_env(os.environ)
    logging.basicConfig(level=logging.INFO)
    logging.getLogger(__name__).info("Configuration loaded: %s", sorted(vars(settings)))


if __name__ == "__main__":
    main()
