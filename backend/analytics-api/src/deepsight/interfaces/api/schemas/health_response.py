"""Esquema de respuesta del endpoint de salud."""

from pydantic import BaseModel, ConfigDict
from pydantic.alias_generators import to_camel


class HealthResponse(BaseModel):
    """Respuesta del endpoint de salud, serializada en `camelCase`.

    Attributes:
        status: Estado del servicio.
    """

    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)

    status: str
