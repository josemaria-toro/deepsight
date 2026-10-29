# Guía de arquitectura: backend

Guía de arquitectura para los proyectos de backend.

## Principio general

- Los proyectos de backend siguen Domain-Driven Design (DDD).
- La arquitectura de referencia es la hexagonal (puertos y adaptadores) combinada con Clean Architecture.
- El código se organiza en capas. Una capa solo depende de las capas indicadas en la regla de dependencias.
- Las dependencias apuntan hacia el dominio. El dominio no depende de ninguna otra capa ni de ningún framework.

### Arquitectura de referencia

La arquitectura hexagonal y la Clean Architecture son complementarias y se aplican a la vez:

- **Hexagonal**: el dominio y la aplicación quedan en el centro y se comunican con el exterior solo a través de **puertos** (interfaces definidas en el núcleo). Los **adaptadores** implementan o invocan esos puertos:
  - Adaptadores de entrada (primarios), en `interfaces/`: HTTP, CLI, runner. Llaman a los casos de uso.
  - Adaptadores de salida (secundarios), en `infrastructure/`: persistencia, configuración, servicios externos. Implementan los puertos de salida.
- **Clean Architecture**: las capas se organizan en círculos concéntricos y la regla de dependencias apunta hacia dentro, hacia `domain`. Es la regla que se detalla a continuación.

Correspondencia con la estructura del proyecto:

| Concepto | Ubicación |
|---|---|
| Núcleo (entidades, reglas de negocio) | `domain/` |
| Casos de uso | `application/use_cases/` |
| Puertos de salida | `domain/ports/` |
| Adaptadores de entrada | `interfaces/` |
| Adaptadores de salida | `infrastructure/` |

Un proyecto nuevo se diseña según este modelo. Si una funcionalidad necesita un acceso externo, se define primero el puerto en `domain/ports/` y después el adaptador en `infrastructure/`.

### Regla de dependencias

```
interfaces → application → domain ← infrastructure
```

- `domain` no importa nada de `application`, `infrastructure` ni `interfaces`, ni de FastAPI, SQLAlchemy ni otros frameworks.
- `application` importa de `domain`, pero no de `infrastructure` ni de `interfaces`.
- `infrastructure` implementa los puertos definidos en `domain` y puede importar de `domain` y `application`.
- `interfaces` expone los casos de uso de `application` al exterior (HTTP, CLI, runner).

## Estructura de carpetas

Cada proyecto de backend sigue esta estructura dentro de `src/<nombre_paquete>/`:

```
src/<nombre_paquete>/
├── domain/                      # Núcleo
│   ├── entities/
│   ├── value_objects/
│   ├── ports/                   # Puertos de salida
│   │   ├── repositories/
│   │   └── gateways/
│   ├── services/
│   └── exceptions.py
├── application/                 # Casos de uso
│   ├── use_cases/
│   └── dtos/
├── infrastructure/              # Adaptadores de salida
│   ├── persistence/
│   │   ├── models.py
│   │   ├── mappers.py
│   │   └── repositories/
│   └── config/
└── interfaces/                  # Adaptadores de entrada
    ├── api/
    │   ├── routers/
    │   └── schemas/
    └── cli/
```

Reglas de la estructura:

- `domain/ports/` agrupa todos los puertos de salida del dominio. Un puerto de persistencia va en `ports/repositories/` y un puerto hacia un sistema externo (correo, almacenamiento, APIs de terceros) va en `ports/gateways/`.
- `domain/services/` contiene solo lógica de dominio que no es un puerto.
- `infrastructure/` y `interfaces/` son los únicos lugares que pueden importar frameworks y bibliotecas externas.
- Los nombres de carpetas de la estructura son fijos. Un proyecto no crea carpetas de nivel superior adicionales sin actualizar esta guía.

## Descripción de las capas

- **`domain/`**: el núcleo del negocio. No depende de ninguna tecnología.
  - `entities/`: entidades con identidad y reglas de negocio. Ejemplo: `project.py` con la clase `Project`.
  - `value_objects/`: objetos inmutables definidos por sus valores. Ejemplo: una versión SemVer.
  - `ports/`: puertos de salida, es decir, interfaces (`typing.Protocol`) que el dominio necesita del exterior.
    - `ports/repositories/`: cómo se persisten las entidades. Ejemplo: `project_repository.py` con `ProjectRepository`.
    - `ports/gateways/`: acceso a sistemas externos. Ejemplo: `notification_gateway.py` con `NotificationGateway`.
  - `services/`: lógica de dominio que no pertenece a una sola entidad y no necesita un puerto.
  - `exceptions.py`: excepciones propias del dominio, como `ProjectNotFoundError`.
- **`application/`**: orquesta los casos de uso. Coordina entidades y puertos, pero no contiene reglas de negocio.
  - `use_cases/`: un caso de uso por fichero, con un único método público de ejecución. Ejemplo: `create_project.py`.
  - `dtos/`: objetos de entrada y salida de los casos de uso, independientes de HTTP y de la base de datos.
- **`infrastructure/`**: detalles técnicos. Implementa los puertos del dominio.
  - `persistence/models.py`: modelos ORM de SQLAlchemy, como `ProjectModel`. No salen de esta capa.
  - `persistence/mappers.py`: conversión entre modelos ORM y entidades de dominio.
  - `persistence/repositories/`: implementaciones de los puertos, como `SqlAlchemyProjectRepository`.
  - `config/`: lectura de configuración desde variables de entorno.
- **`interfaces/`**: adaptadores de entrada, sin lógica de negocio.
  - `api/routers/`: rutas de FastAPI que llaman a los casos de uso.
  - `api/schemas/`: modelos de entrada y salida de la API. Se exponen en `camelCase`.
  - `cli/`: comandos de línea de comandos, si el proyecto los necesita.

## Rutas de la API

- Todos los endpoints de la API siguen el formato `/api/v<num_version>/<path>`, donde `<num_version>` es el número entero de la versión mayor de la API. Ejemplo: `/api/v1/projects`.
- La versión forma parte de la ruta, nunca de una cabecera ni de un parámetro de consulta.
- Cada versión se agrupa en su propio router dentro de `interfaces/api/routers/`, con el prefijo `/api/v<num_version>`.
- Un cambio incompatible en el contrato de un endpoint exige publicar una nueva versión. Los cambios compatibles se mantienen en la versión vigente.
- Los endpoints de salud o de métricas, si existen, siguen el mismo formato.

## Reglas de nombrado

- Las entidades de dominio no llevan sufijo: `Project`.
- Los modelos ORM llevan el sufijo `Model`: `ProjectModel`.
- Los puertos de repositorio se nombran por la entidad: `ProjectRepository`.
- Las implementaciones indican la tecnología: `SqlAlchemyProjectRepository`.
- Los casos de uso se nombran con un verbo: `CreateProject`, `GetProject`.

## Creación de entidades del dominio

Cada vez que se crea una entidad del dominio, se crean también los siguientes elementos en el mismo cambio:

1. **Contrato del repositorio** (`domain/ports/repositories/`): puerto `typing.Protocol` con las operaciones de persistencia de la entidad. Ejemplo: `ProjectRepository`.
2. **Modelo de persistencia** (`infrastructure/persistence/models.py`): clase ORM de SQLAlchemy que representa la tabla. Ejemplo: `ProjectModel`. Los tamaños de las columnas coinciden con las longitudes máximas definidas en la entidad.
3. **Repositorio de persistencia** (`infrastructure/persistence/repositories/`): implementación del contrato con SQLAlchemy. Ejemplo: `SqlAlchemyProjectRepository`. Recibe la sesión por inyección y devuelve entidades de dominio, nunca modelos ORM.
4. **Mapper** (`infrastructure/persistence/mappers.py`): conversión bidireccional entre el modelo ORM y la entidad de dominio.

- Una entidad no se considera completa hasta que tenga estos cuatro elementos y sus pruebas.
- Las pruebas de la entidad son unitarias. Las del repositorio y el mapper son de integración contra una base de datos real.

## Pruebas por capa

- Las pruebas de `domain` y `application` son unitarias y no necesitan base de datos ni red.
- Las pruebas de `infrastructure` son de integración y usan una base de datos real.
- Las pruebas de `interfaces` comprueban los contratos HTTP o de CLI.
- La estructura de `tests/` refleja la de `src/`.
