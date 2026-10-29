# Proposal

## Why

La plataforma DeepSight recibirá datos de observabilidad de orígenes muy distintos (aplicaciones, servicios, agentes de inteligencia artificial, etc.). Antes de definir cada tipo concreto de dato (ticket #4 al #12) hace falta una estructura base común, para que todos se identifiquen, se correlacionen y se procesen de la misma forma. Ticket de origen: #3.

## What Changes

- Se define la estructura base que debe cumplir cualquier objeto de observabilidad recibido: `app_name`, `app_version`, `client_ip_address`, `client_version`, `extra_data`, `host_name`, `span_id`, `timespan` y `trace_id`.
- Se fija qué propiedades son obligatorias y cuáles admiten nulos, y la validación que se aplica a cada una.
- Se fija la representación JSON de la base (`camelCase`) y de `timespan` (fecha y hora con zona horaria).
- La base queda como punto de extensión: los tipos concretos de los tickets #4 a #12 la heredan y añaden sus propias propiedades.
- No se definen endpoints ni tipos concretos de dato. Eso corresponde a los tickets siguientes.

## Capabilities

### New Capabilities

- `observability-base-data`: estructura, obligatoriedad y validación de las propiedades comunes a todos los objetos de observabilidad que se ingieren.

### Modified Capabilities

Ninguna.

## Impact

- `backend/ingestion-api`: esquema de entrada y DTO con la estructura base y su validación.
- `backend/ingestion-workers`: entidad de dominio base y su persistencia (modelo, mapper y contrato de repositorio), más la migración de Alembic de la tabla base.
- `docs/` de ambos proyectos: diagramas de clases y definición de clases.
- Sin nuevas dependencias.
