# Instrucciones para Claude

Este repositorio es Spec Driven Development, un repositorio que sirve como plantilla para crear aplicaciones utilizando agentes de inteligencia artificial, siguiendo el estandar Spec Driven Development.

## Guías obligatorias

Antes de escribir, modificar o revisar código, lee las guías correspondientes en `.harness/` y aplícalas. Las guías son la fuente de verdad. Si una instrucción de este fichero contradice una guía, avisa al usuario y sigue la guía.

- Código Python: `.harness/development/python.md`, `.harness/security/python.md`, `.harness/technologies/python.md`, `.harness/guardrails/python.md`.
- Código React: `.harness/development/react.md`, `.harness/security/react.md`, `.harness/technologies/react.md`, `.harness/guardrails/react.md`.
- Documentación de proyectos: `.harness/documentation/projects.md`.
- Arquitectura de backend: `.harness/architecture/python.md`. Los proyectos de backend siguen DDD como enfoque de diseño, implementado con arquitectura hexagonal y Clean Architecture.
- Arquitectura de frontend: `.harness/architecture/react.md`. Los proyectos de frontend se organizan por funcionalidad.
- Ramas, commits y PR: `.harness/development/git.md`.
- Despliegues: `.harness/development/deployment.md`.
- Especificaciones: `.harness/development/specifications.md`.
- Licencias: `.harness/licenses/`.

Si una tecnología no aparece en la guía de tecnologías admitidas, no la uses. Pregunta al usuario antes de incorporarla.

## Reglas clave

- **Guardarraíles**: cualquier cambio debe cumplir los guardarraíles de su lenguaje (`.harness/guardrails/`). Si incumple uno, el merge queda bloqueado. Debes comprobarlo antes de proponer un cambio como listo.
- **Idioma**: el código fuente, incluidos nombres de módulos, clases, métodos, propiedades, variables y constantes, va en inglés. Los docstrings, los comentarios y la documentación van en español. Los mensajes de log y las excepciones van en inglés.
- **Serialización JSON**: las propiedades expuestas en JSON van en `camelCase`, nunca en `snake_case`. Los atributos internos de Python siguen en `snake_case`; la conversión se hace solo en la capa de serialización (por ejemplo, con alias de `pydantic`).
- **Rutas de la API**: los endpoints siguen el formato `/api/v<num_version>/<path>`, como indica `.harness/architecture/python.md`.
- **Pruebas**: el código nuevo o modificado necesita pruebas unitarias, de integración y end-to-end según corresponda. La cobertura del proyecto debe ser al menos del 80 %. Las pruebas se ejecutan de forma efímera en Docker; si no está instalado, recomienda al usuario instalarlo.
- **Arquitectura de backend**: los proyectos de backend siguen DDD y aplican la arquitectura hexagonal con Clean Architecture, con las capas y la regla de dependencias de `.harness/architecture/python.md`. Al crear una entidad del dominio, crea en el mismo cambio su contrato de repositorio, su modelo de persistencia, su repositorio de SQLAlchemy y su mapper, con sus pruebas.
- **Especificaciones**: las funcionalidades nuevas y los cambios de comportamiento se especifican con OpenSpec antes de implementarlos. Las especificaciones viven en `openspec/` de cada proyecto. Cada especificación sigue obligatoriamente el flujo de OpenSpec, en orden y sin omitir fases: `propose` (`/opsx:propose`), `apply` (`/opsx:apply`) y `archive` (`/opsx:archive`), como indica `.harness/development/specifications.md`. Al implementar un cambio, sigue sus `tasks.md` y comprueba que cada escenario tiene su prueba. Archiva el cambio en la rama de trabajo antes del merge.
- **Documentación**: los cambios que afecten a un proyecto deben actualizar su documentación en el mismo PR.
- **Despliegues**: cuando se solicite desplegar un proyecto, genera su versión distribuible, comprímela como `<nombre-proyecto>.v<version>.zip` en `releases/` (en la raíz del espacio de trabajo; créala si no existe) y crea junto a ella un `.txt` equivalente con las variables de entorno y configuraciones necesarias, sin valores. `releases/` no se versiona y está en el `.gitignore`. Sigue `.harness/development/deployment.md`.
- **Secretos**: nunca escribas contraseñas, claves, tokens ni otra información sensible en el código, en la documentación ni en los ejemplos.

## Git

- `main` es la única rama permanente. No hagas commits directos en `main`.
- Crea ramas de trabajo desde `main` con el formato `<tipo>/<descripción-corta>`, con la descripción en inglés (`feature/`, `fix/`, `docs/`, `refactor/`, `chore/`).
- Los cambios entran en `main` mediante PR, con merge por squash. El título del PR va en inglés y se corresponde con el título de la especificación de OpenSpec. La descripción del PR también va en inglés, sin datos del agente utilizado (`Co-Authored-By`, `Generated with`, etc.), con el listado de cambios coherente, claro y conciso, en una única lista sin separar por commit.
- Los commits usan la identidad configurada en git. No cambies el autor de los commits.

## Primera ejecución en el repositorio

Antes de cualquier otra tarea, cuenta los commits del repositorio en todas sus ramas, locales y remotas, con `git rev-list --count --all`. Si el repositorio no tiene commits (el comando devuelve `0`) o solo tiene el commit inicial (devuelve `1`), es la primera vez que se ejecuta Claude en este repositorio. En ese caso, sigue este procedimiento en orden antes de cualquier otra tarea.

Si hay más de un commit, el onboarding ya se ha hecho y no se ejecuta.

### 1. Saludo

Saluda al usuario, explica en pocas líneas los pasos que vas a seguir y pregunta lo que necesites para continuar.

### 2. Descripción del repositorio

- Pide al usuario una descripción del repositorio: qué es, para quién es y qué objetivo tiene.
- Espera su respuesta antes de continuar.
- Con esa descripción, actualiza `README.md` (la descripción y la estructura del repositorio) y `CLAUDE.md` (la primera línea y cualquier dato de contexto que haya cambiado). No añadas contenido que el usuario no haya indicado.

### 3. Identidad de git

- Pide el nombre y el correo del usuario. No los supongas a partir del contexto de la sesión.
- Configúralos en el repositorio, de forma local: `git config --local user.name` y `git config --local user.email`.
- Verifica los valores con `git config --local --get`.
- Si ya hay valores locales, muéstralos y confirma antes de cambiarlos.

### 4. OpenSpec

- Comprueba si OpenSpec está instalado (`openspec --version`) y si el proyecto está inicializado (existe la carpeta `openspec/`).
- Si no está instalado, instálalo. Requiere Node.js 22 LTS. Si Node.js no está disponible, indícalo y pide al usuario que lo instale o autoriza su instalación (ver "Permisos y credenciales").
- Si no está inicializado, inicialízalo siguiendo la documentación oficial de la versión instalada y [la guía de especificaciones](.harness/development/specifications.md).
- Fija la versión de OpenSpec en el proyecto, como indica esa guía.

### 5. Estructura de ramas

- Lee [la guía de estrategia de ramas](.harness/development/git.md) y comprueba las ramas locales y remotas.
- Si falta `main`, indícalo al usuario y créala desde el HEAD actual.
- Si existen otras ramas permanentes de estrategias anteriores (por ejemplo, `develop` o `pre-release`), indícalo al usuario. No las elimines sin su confirmación.
- Cambia a `main`. Es el punto de partida de todo el trabajo.
- Si hay cambios sin commitear que impedirían el cambio de rama, para y pregunta al usuario qué hacer con ellos. No los descartes ni los incluyas en un commit sin confirmación.
- No hagas push de ninguna rama sin confirmación del usuario. Las reglas de protección de ramas en el remoto no se pueden configurar desde el repositorio local. Indícalas en el resumen como pendientes si no están configuradas.

### 6. Contexto del proyecto

Pregunta qué contendrá el repositorio. Puede ser más de un tipo.

- **Backend**: pide las tecnologías que quiere usar. Crea el scaffolding de backend según [la guía de arquitectura de Python](.harness/architecture/python.md), [las tecnologías admitidas de Python](.harness/technologies/python.md) y [los guardarraíles de Python](.harness/guardrails/python.md). Se crea en `backend/`.
- **Frontend**: pide las tecnologías que quiere usar. Crea el scaffolding de frontend según [la guía de arquitectura de React](.harness/architecture/react.md), [las tecnologías admitidas de React](.harness/technologies/react.md) y [los guardarraíles de React](.harness/guardrails/react.md). Se crea en `frontend/`.
- **Otro tipo de proyecto**: pide el tipo, el nombre y las tecnologías que quiere usar. No hay scaffolding automático para este tipo todavía.

Para todos los proyectos, pide las tecnologías al usuario, aunque el scaffolding automático solo admita Python (backend) y React (frontend). Compáralas con la guía de tecnologías admitidas del lenguaje correspondiente. Si una tecnología no está admitida, no la incorpores hasta que el usuario lo apruebe. Si no hay scaffolding automático para el tipo de proyecto, no crees la estructura hasta tener las tecnologías definidas y validadas.

### 7. Scaffolding

- Trabaja en una rama `chore/scaffolding-inicial` creada desde `main`. No hagas commits directos en `main`.
- Crea la estructura de cada proyecto siguiendo sus guías. Incluye la configuración de herramientas (formateo, linter, tipos, pruebas con cobertura mínima del 80 %, pre-commit), el `README.md` según [la guía de documentación de proyectos](.harness/documentation/projects.md) y un `.gitignore` acorde a la tecnología.
- Incluye al menos una prueba de humo por proyecto, para que las comprobaciones se puedan ejecutar desde el primer momento.
- No crees código de negocio. El scaffolding solo contiene la estructura, la configuración y los ejemplos mínimos necesarios.
- Haz commits con la identidad configurada en el paso 3, siguiendo [la guía de commits](.harness/development/git.md).

### 8. Resumen del scaffolding

Muestra al usuario un resumen con:
- Los proyectos creados y su ubicación.
- Las tecnologías de cada proyecto.
- La estructura de carpetas creada.
- Las ramas creadas o corregidas y la rama actual.
- Los pasos pendientes, como el push o la protección de ramas.

### 9. Instalación de tecnologías

- Instala las tecnologías de cada proyecto: intérpretes, herramientas de gestión de dependencias, dependencias del proyecto, entornos virtuales y hooks de pre-commit. Por ejemplo, `uv` y `.venv` en backend, y `pnpm` en frontend.
- Ejecuta las comprobaciones del proyecto (formateo, linter, tipos, pruebas y cobertura) y confirma que pasan.
- Si una instalación falla, indica el error exacto y no lo ocultes.

### 10. Permisos y credenciales

- Si una instalación necesita permisos elevados, como `sudo`, pide al usuario que ejecute el comando con el prefijo `!` para que la contraseña no pase por el chat. Si no es posible, pídela directamente.
- Usa las credenciales solo para ese comando concreto. Olvídalas inmediatamente después.
- Nunca las guardes ni las recuerdes: no las escribas en ficheros, variables de entorno, configuración, memoria, logs, commits, documentación ni resúmenes.
- Si necesitas permisos de nuevo, pídelos otra vez.

### 11. Cierre

- Muestra al usuario un resumen final: identidad configurada, estado de OpenSpec, ramas, scaffolding creado, tecnologías instaladas, comprobaciones ejecutadas y pendientes.

## Antes de terminar una tarea

1. Ejecuta las comprobaciones del proyecto: formateo, linter, tipos, pruebas y cobertura.
2. Revisa los guardarraíles aplicables y confirma que el cambio los cumple. Si hay un cambio de OpenSpec asociado, confirma que ha pasado por `propose` y `apply`, que está validado, que sus tareas y escenarios están cubiertos y que está archivado con `archive`.
3. Actualiza la documentación afectada.
4. Resume al usuario qué has cambiado y qué comprobaciones has ejecutado. Si alguna no se ha podido ejecutar, dilo.
