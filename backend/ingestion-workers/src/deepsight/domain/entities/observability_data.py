"""Entidad base de los datos de observabilidad."""

from abc import ABC, abstractmethod
from dataclasses import dataclass, field
from datetime import datetime
from typing import Any, ClassVar
from uuid import UUID, uuid4

from deepsight.domain.exceptions import DomainError


@dataclass(kw_only=True)
class ObservabilityData(ABC):
    """Propiedades base comunes a todos los tipos de dato de observabilidad.

    Es abstracta: solo se instancian los tipos concretos que heredan de ella.

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
        id: Identidad interna generada por el sistema.
    """

    MAX_APP_NAME_LENGTH: ClassVar[int] = 255
    MAX_APP_VERSION_LENGTH: ClassVar[int] = 64
    MAX_CLIENT_IP_ADDRESS_LENGTH: ClassVar[int] = 45
    MAX_CLIENT_VERSION_LENGTH: ClassVar[int] = 64
    MAX_HOST_NAME_LENGTH: ClassVar[int] = 255
    MAX_SPAN_ID_LENGTH: ClassVar[int] = 64
    MAX_TRACE_ID_LENGTH: ClassVar[int] = 64

    app_name: str
    app_version: str
    span_id: str
    timespan: datetime
    trace_id: str
    client_ip_address: str | None = None
    client_version: str | None = None
    extra_data: dict[str, Any] | None = None
    host_name: str | None = None
    id: UUID = field(default_factory=uuid4)

    def __post_init__(self) -> None:
        for name in ("app_name", "app_version", "span_id", "trace_id"):
            if not getattr(self, name):
                raise DomainError(f"Property {name} is required and cannot be empty")
        if self.timespan.tzinfo is None:
            raise DomainError("Property timespan must include a time zone")
        limits = {
            "app_name": self.MAX_APP_NAME_LENGTH,
            "app_version": self.MAX_APP_VERSION_LENGTH,
            "client_ip_address": self.MAX_CLIENT_IP_ADDRESS_LENGTH,
            "client_version": self.MAX_CLIENT_VERSION_LENGTH,
            "host_name": self.MAX_HOST_NAME_LENGTH,
            "span_id": self.MAX_SPAN_ID_LENGTH,
            "trace_id": self.MAX_TRACE_ID_LENGTH,
        }
        for name, limit in limits.items():
            value = getattr(self, name)
            if value is not None and len(value) > limit:
                raise DomainError(f"Property {name} exceeds {limit} characters")

    @classmethod
    @abstractmethod
    def type_name(cls) -> str:
        """Nombre del tipo concreto, usado como discriminador de persistencia."""
