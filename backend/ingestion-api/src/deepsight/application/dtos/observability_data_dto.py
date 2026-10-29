"""DTO base de los objetos de observabilidad."""

from dataclasses import dataclass
from datetime import datetime
from typing import Any

MAX_LENGTHS: dict[str, int] = {
    "app_name": 255,
    "app_version": 64,
    "client_ip_address": 45,
    "client_version": 64,
    "host_name": 255,
    "span_id": 64,
    "trace_id": 64,
}


@dataclass(frozen=True, kw_only=True)
class ObservabilityDataDto:
    """Propiedades base comunes a todos los objetos de observabilidad.

    Es independiente de HTTP y de la base de datos. Los DTO de los tipos
    concretos heredan de esta clase.

    Attributes:
        app_name: Nombre de la aplicación origen.
        app_version: Versión de la aplicación origen.
        span_id: Identificador del tramo.
        timespan: Instante del dato, con zona horaria.
        trace_id: Identificador de la traza a la que pertenece.
        client_ip_address: Dirección IP del cliente, si se conoce.
        client_version: Versión del cliente, si se conoce.
        extra_data: Datos adicionales de formato libre.
        host_name: Nombre del equipo origen.
    """

    app_name: str
    app_version: str
    span_id: str
    timespan: datetime
    trace_id: str
    client_ip_address: str | None = None
    client_version: str | None = None
    extra_data: dict[str, Any] | None = None
    host_name: str | None = None

    def __post_init__(self) -> None:
        for name in ("app_name", "app_version", "span_id", "trace_id"):
            if not getattr(self, name):
                raise ValueError(f"Property {name} is required and cannot be empty")
        if self.timespan.tzinfo is None:
            raise ValueError("Property timespan must include a time zone")
        for name, max_length in MAX_LENGTHS.items():
            value = getattr(self, name)
            if value is not None and len(value) > max_length:
                raise ValueError(f"Property {name} exceeds {max_length} characters")
