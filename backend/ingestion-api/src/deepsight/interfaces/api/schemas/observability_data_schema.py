"""Esquema base de entrada de los objetos de observabilidad."""

from typing import Annotated, Any

from pydantic import AwareDatetime, BaseModel, ConfigDict, Field, StrictStr
from pydantic.alias_generators import to_camel

RequiredText = Annotated[StrictStr, Field(min_length=1)]


class ObservabilityDataSchema(BaseModel):
    """Propiedades base de todo objeto de observabilidad, en `camelCase`.

    Los esquemas de los tipos concretos heredan de esta clase. Solo se aceptan
    los nombres en `camelCase`: `snake_case` se rechaza.

    Attributes:
        app_name: Nombre de la aplicación origen.
        app_version: Versión de la aplicación origen.
        span_id: Identificador del tramo.
        timespan: Instante del dato, con zona horaria (ISO 8601).
        trace_id: Identificador de la traza.
        client_ip_address: Dirección IP del cliente.
        client_version: Versión del cliente.
        extra_data: Datos adicionales de formato libre.
        host_name: Nombre del equipo origen.
    """

    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=False)

    app_name: Annotated[RequiredText, Field(max_length=255)]
    app_version: Annotated[RequiredText, Field(max_length=64)]
    span_id: Annotated[RequiredText, Field(max_length=64)]
    timespan: AwareDatetime
    trace_id: Annotated[RequiredText, Field(max_length=64)]
    client_ip_address: Annotated[StrictStr, Field(max_length=45)] | None = None
    client_version: Annotated[StrictStr, Field(max_length=64)] | None = None
    extra_data: dict[str, Any] | None = None
    host_name: Annotated[StrictStr, Field(max_length=255)] | None = None
