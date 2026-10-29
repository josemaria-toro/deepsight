"""Modelo concreto de ejemplo, solo para las pruebas de persistencia."""

from deepsight.infrastructure.persistence.models import ObservabilityDataModel


class ExampleDataModel(ObservabilityDataModel):
    """Modelo concreto mínimo, con herencia de tabla única."""

    __mapper_args__ = {"polymorphic_identity": "example"}
