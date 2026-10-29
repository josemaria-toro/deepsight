"""Modelos de persistencia de SQLAlchemy."""

from datetime import datetime
from typing import Any
from uuid import UUID

from sqlalchemy import DateTime, String, Uuid
from sqlalchemy.dialects.postgresql import JSONB
from sqlalchemy.orm import Mapped, mapped_column

from deepsight.domain.entities.observability_data import ObservabilityData
from deepsight.infrastructure.persistence.base import Base


class ObservabilityDataModel(Base):
    """Tabla base `observability_data` con las propiedades comunes.

    Usa herencia unida: la columna `type` discrimina el tipo concreto, cuyas
    propiedades propias viven en una tabla que apunta a esta. No tiene
    identidad polimórfica propia porque la entidad base es abstracta.
    """

    __tablename__ = "observability_data"
    __mapper_args__ = {"polymorphic_on": "type"}

    id: Mapped[UUID] = mapped_column(Uuid, primary_key=True)
    type: Mapped[str] = mapped_column(String(64), nullable=False, index=True)
    app_name: Mapped[str] = mapped_column(String(ObservabilityData.MAX_APP_NAME_LENGTH))
    app_version: Mapped[str] = mapped_column(String(ObservabilityData.MAX_APP_VERSION_LENGTH))
    span_id: Mapped[str] = mapped_column(String(ObservabilityData.MAX_SPAN_ID_LENGTH))
    timespan: Mapped[datetime] = mapped_column(DateTime(timezone=True))
    trace_id: Mapped[str] = mapped_column(String(ObservabilityData.MAX_TRACE_ID_LENGTH), index=True)
    client_ip_address: Mapped[str | None] = mapped_column(
        String(ObservabilityData.MAX_CLIENT_IP_ADDRESS_LENGTH)
    )
    client_version: Mapped[str | None] = mapped_column(
        String(ObservabilityData.MAX_CLIENT_VERSION_LENGTH)
    )
    extra_data: Mapped[dict[str, Any] | None] = mapped_column(JSONB)
    host_name: Mapped[str | None] = mapped_column(String(ObservabilityData.MAX_HOST_NAME_LENGTH))
