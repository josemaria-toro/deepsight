"""Base declarativa de los modelos de SQLAlchemy."""

from sqlalchemy.orm import DeclarativeBase


class Base(DeclarativeBase):
    """Clase base de todos los modelos de persistencia."""
