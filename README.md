# DeepSight

DeepSight es una plataforma de observabilidad. Se desarrolla con agentes de inteligencia artificial siguiendo el estándar Spec Driven Development y consta de los siguientes proyectos:

| Proyecto | Ubicación | Descripción |
|---|---|---|
| `ingestion-api` | `backend/ingestion-api/` | API pública para la ingesta de datos. |
| `ingestion-workers` | `backend/ingestion-workers/` | Procesa los mensajes publicados por la API de ingesta. |
| `analytics-api` | `backend/analytics-api/` | API pública que actúa como backend del portal analítico. |
| `analytics-portal` | `frontend/analytics-portal/` | Portal web para visualizar los datos de forma estructurada, con gráficos en tiempo real y filtros. |

Tecnologías: PostgreSQL como base de datos, RabbitMQ como broker de mensajería, Python (backend) y React (frontend).

## Herramientas de desarrollo

- **OpenSpec 1.14.3**: versión fijada del proyecto para gestionar las especificaciones de `openspec/`.
- **Node.js**: la guía exige Node.js 22 LTS. El equipo inicial trabaja con Node.js 20.19.2, desviación aceptada hasta que se actualice el entorno.

## Estructura del repositorio

```
.
├── .claude/          # Comandos y skills de OpenSpec para Claude Code
├── .github/          # Plantillas de issues y configuración de Dependabot
├── .harness/         # Guías obligatorias para agentes y desarrolladores
│   ├── architecture/     # Arquitectura de backend (Python) y frontend (React)
│   ├── development/      # Git, especificaciones, despliegues y desarrollo por lenguaje
│   ├── documentation/    # Guía de documentación de proyectos
│   ├── guardrails/       # Guardarraíles por lenguaje
│   ├── licenses/         # Licencias por lenguaje
│   ├── security/         # Seguridad por lenguaje
│   └── technologies/     # Tecnologías admitidas por lenguaje
├── backend/          # Proyectos Python: ingestion-api, ingestion-workers y analytics-api
├── CLAUDE.md         # Instrucciones para Claude Code
├── frontend/         # Proyectos React: analytics-portal
├── LICENSE
├── openspec/         # Especificaciones: specs/ (vigentes) y changes/ (en curso y archivados)
└── README.md
```

## Calidad de código

Los hooks de `pre-commit` de todos los proyectos se definen en `.pre-commit-config.yaml` (raíz). Se instalan con `pre-commit install`.

## Despliegues

Al solicitar el despliegue de un proyecto se genera su versión distribuible en `releases/`, en la raíz del espacio de trabajo (se crea si no existe):

- `<nombre-proyecto>.v<version>.zip`: versión distribuible del proyecto.
- `<nombre-proyecto>.v<version>.txt`: variables de entorno y configuraciones necesarias para desplegarlo, sin valores.

`releases/` no se versiona: está incluida en el `.gitignore`. El proceso completo está en [la guía de despliegues](.harness/development/deployment.md).
