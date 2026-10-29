"""Fábrica de la aplicación FastAPI."""

from fastapi import FastAPI

from deepsight_analytics_api.interfaces.api.routers.v1_router import v1_router


def create_app() -> FastAPI:
    """Crea la aplicación FastAPI con sus routers versionados.

    Returns:
        La aplicación configurada.
    """
    app = FastAPI(title="analytics-api")
    app.include_router(v1_router)
    return app
