# Clases de ingestion-api

## Diagrama de clases

```mermaid
classDiagram
    class Settings
    class DomainError
    class HealthResponse
    Exception <|-- DomainError
```

## Definición de clases

### `Settings` (`infrastructure/config/settings.py`)

Configuración inmutable leída de variables de entorno.

- Propiedades: `rabbitmq_url`.
- `from_env(environ)`: construye la configuración. Lanza `ValueError` si falta una variable obligatoria.

### `DomainError` (`domain/exceptions.py`)

Excepción base del dominio.

### `HealthResponse` (`interfaces/api/schemas/health_response.py`)

Respuesta del endpoint `GET /api/v1/health`, serializada en `camelCase`. Propiedad: `status`.
