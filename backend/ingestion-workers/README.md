# ingestion-workers

## Descripción

Procesos que consumen los mensajes publicados por la API de ingesta y los persisten en PostgreSQL. Forma parte de la plataforma DeepSight.

## Requisitos

- Python 3.13 o superior.
- [`uv`](https://docs.astral.sh/uv/).
- Docker, para ejecutar las pruebas de forma efímera.

## Instalación

```bash
cd backend/ingestion-workers
uv sync
uv run pre-commit install --config ../../.pre-commit-config.yaml
```

## Ejecución

```bash
export RABBITMQ_URL=<valor> DATABASE_URL=<valor>
uv run python examples/load_settings.py
```

El proceso consumidor de mensajes se añadirá con las primeras especificaciones.

## Pruebas

```bash
# Pruebas en un contenedor efímero de Docker (recomendado)
./scripts/test.sh

# Pruebas en local, con cobertura mínima del 80 %
uv run pytest

# Solo pruebas de integración (arrancan un PostgreSQL efímero con Docker)
uv run pytest -m integration

# Comprobaciones de calidad
uv run black --check .
uv run ruff check .
uv run mypy
```

Todavía no hay pruebas end-to-end en este proyecto.

## Migraciones

```bash
uv run alembic upgrade head
uv run alembic downgrade base
```

Requieren la variable de entorno `DATABASE_URL`.

## Configuración

| Variable de entorno | Finalidad |
|---|---|
| `RABBITMQ_URL` | URL de conexión con RabbitMQ. |
| `DATABASE_URL` | URL de conexión con PostgreSQL (driver `asyncpg`). |

Nunca se versionan valores reales. Los ficheros `.env` están en el `.gitignore`.

## Despliegue

Se sigue [la guía de despliegues](../../.harness/development/deployment.md).

## Documentación

- [Arquitectura](docs/architecture.md)
- [Clases](docs/classes.md)
- [Ejemplos de uso](docs/examples.md)
- Decisiones técnicas en [`docs/adr/`](docs/adr/).

## Contribución

Consulta [la guía de ramas](../../.harness/development/git.md) y los [guardarraíles de Python](../../.harness/guardrails/python.md). Las especificaciones viven en [`openspec/`](../../openspec/) (OpenSpec 1.14.3).

## Licencia

GNU GPL versión 3. Consulta el fichero [`LICENSE`](LICENSE).
