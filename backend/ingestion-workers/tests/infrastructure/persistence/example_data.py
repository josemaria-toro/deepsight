"""Tipo concreto de ejemplo, solo para las pruebas de persistencia."""

from deepsight.domain.entities.observability_data import ObservabilityData


class ExampleData(ObservabilityData):
    """Entidad concreta mínima."""

    @classmethod
    def type_name(cls) -> str:
        return "example"
