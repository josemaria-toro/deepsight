"""Conversión entre modelos de persistencia y entidades de dominio."""

from typing import Any

from deepsight.domain.entities.observability_data import ObservabilityData
from deepsight.infrastructure.persistence.models import ObservabilityDataModel


def observability_data_to_model_fields(entity: ObservabilityData) -> dict[str, Any]:
    """Obtiene las propiedades base de una entidad para construir su modelo.

    Los mappers de los tipos concretos combinan este resultado con sus propias
    propiedades. El discriminador `type` lo fija el modelo concreto.

    Args:
        entity: Entidad de dominio.

    Returns:
        Propiedades base con los nombres de las columnas.
    """
    return {
        "id": entity.id,
        "app_name": entity.app_name,
        "app_version": entity.app_version,
        "span_id": entity.span_id,
        "timespan": entity.timespan,
        "trace_id": entity.trace_id,
        "client_ip_address": entity.client_ip_address,
        "client_version": entity.client_version,
        "extra_data": entity.extra_data,
        "host_name": entity.host_name,
    }


def observability_data_from_model_fields(model: ObservabilityDataModel) -> dict[str, Any]:
    """Obtiene las propiedades base de un modelo para construir su entidad.

    Args:
        model: Modelo de persistencia.

    Returns:
        Propiedades base con los nombres de los atributos de la entidad.
    """
    return {
        "id": model.id,
        "app_name": model.app_name,
        "app_version": model.app_version,
        "span_id": model.span_id,
        "timespan": model.timespan,
        "trace_id": model.trace_id,
        "client_ip_address": model.client_ip_address,
        "client_version": model.client_version,
        "extra_data": model.extra_data,
        "host_name": model.host_name,
    }
