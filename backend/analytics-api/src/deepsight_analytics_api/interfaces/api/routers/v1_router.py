"""Router de la versión 1 de la API."""

from fastapi import APIRouter

from deepsight_analytics_api.interfaces.api.schemas.health_response import HealthResponse

v1_router = APIRouter(prefix="/api/v1")


@v1_router.get("/health", response_model=HealthResponse, response_model_by_alias=True)
async def get_health() -> HealthResponse:
    """Devuelve el estado del servicio."""
    return HealthResponse(status="ok")
