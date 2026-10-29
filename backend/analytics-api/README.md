# analytics-api

## Descripción

API pública que actúa como backend del portal analítico: expone los datos almacenados en PostgreSQL con filtros y datos en tiempo real. Forma parte de la plataforma DeepSight.

## Requisitos

- Python 3.13 o superior.
- [`uv`](https://docs.astral.sh/uv/).
- Docker, para ejecutar las pruebas de forma efímera.

## Instalación

```bash
cd backend/analytics-api
uv sync
uv run pre-commit install --config ../../.pre-commit-config.yaml
```

## Ejecución

```bash
export DATABASE_URL=<valor>
uv run uvicorn --factory deepsight.interfaces.api.app:create_app --reload
```

La API queda disponible en `http://localhost:8000/api/v1/health`.

## Pruebas

```bash
# Pruebas en un contenedor efímero de Docker (recomendado)
./scripts/test.sh

# Pruebas en local, con cobertura mínima del 80 %
uv run pytest

# Solo pruebas de integración
uv run pytest -m integration

# Comprobaciones de calidad
uv run black --check .
uv run ruff check .
uv run mypy
```

Todavía no hay pruebas end-to-end en este proyecto.

## Configuración

| Variable de entorno | Finalidad |
|---|---|
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
