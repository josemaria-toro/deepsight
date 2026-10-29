"""Contrato de persistencia de los datos de observabilidad."""

from typing import Protocol
from uuid import UUID

from deepsight.domain.entities.observability_data import ObservabilityData


class ObservabilityDataRepository[T: ObservabilityData](Protocol):
    """Operaciones de persistencia de un tipo concreto de dato de observabilidad."""

    async def add(self, entity: T) -> None:
        """Persiste una entidad nueva.

        Args:
            entity: Entidad que se persiste.
        """
        ...

    async def get(self, entity_id: UUID) -> T | None:
        """Recupera una entidad por su identidad.

        Args:
            entity_id: Identidad de la entidad.

        Returns:
            La entidad o `None` si no existe.
        """
        ...
