# Tecnologías admitidas: Python

Guía que incluye las tecnologías admitidas en aplicaciones basadas en Python.

Cualquier tecnología que no aparezca en esta lista requiere aprobación previa antes de incorporarla a un proyecto.

## Lenguaje y gestión de dependencias

| Tecnología | Uso |
|---|---|
| Python 3.13 o superior | Lenguaje base. |
| `uv` | Gestión de entornos, dependencias y fichero de bloqueo. |
| `pyproject.toml` | Configuración del proyecto y dependencias. |

## Especificaciones

| Tecnología | Uso |
|---|---|
| `OpenSpec` | Escritura y gestión de especificaciones de cambios, según [la guía de especificaciones](../development/specifications.md). Es una herramienta de desarrollo y requiere Node.js 22 LTS. No es una dependencia de ejecución del proyecto. |

## Calidad de código

| Tecnología | Uso |
|---|---|
| `black` | Formateo de código. |
| `ruff` | Linter. |
| `mypy` | Comprobación de tipos en modo estricto. |
| `pre-commit` | Ejecución de comprobaciones antes de cada commit. |

## Pruebas

| Tecnología | Uso |
|---|---|
| `pytest` | Framework de pruebas unitarias y de integración. |
| `pytest-asyncio` | Tecnología por defecto para las pruebas asíncronas. |
| `pytest-cov` | Medición de cobertura de código. |
| `Docker` | Ejecución efímera de las pruebas y de los servicios que necesitan (base de datos, broker). Es una herramienta de desarrollo, no una dependencia de ejecución del proyecto. |

## Seguridad

| Tecnología | Uso |
|---|---|
| `pip-audit` | Detección de vulnerabilidades en dependencias. |
| `bandit` | Análisis estático de seguridad. |
| `argon2-cffi` | Hash de contraseñas con Argon2. |
| `cryptography` | Cifrado y descifrado simétrico (AES-GCM, Fernet) y criptografía asimétrica. |

## Datos y validación

| Tecnología | Uso |
|---|---|
| `pydantic` | Validación de datos y serialización JSON. |
| `PostgreSQL` | Motor de base de datos por defecto. |
| `asyncpg` | Driver por defecto para PostgreSQL. |
| `SQLAlchemy` | Acceso a bases de datos relacionales, con consultas parametrizadas. |
| `Alembic` | Migraciones de esquema de bases de datos. |

## Mensajería

| Tecnología | Uso |
|---|---|
| `RabbitMQ` | Broker de mensajería por defecto. |
| `pika` | Librería por defecto para las comunicaciones con RabbitMQ. |

## Aplicaciones web y APIs

| Tecnología | Uso |
|---|---|
| `FastAPI` | Framework para APIs HTTP. |
| `httpx` | Cliente HTTP con verificación TLS activa. |

## Logging y configuración

| Tecnología | Uso |
|---|---|
| `logging` (biblioteca estándar) | Registro de eventos de ejecución. |
| Variables de entorno | Configuración y secretos. |
