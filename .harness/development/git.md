# Guía de desarrollo: Git

Guía con la estrategia de ramas.

## Rama permanente

- `main`: es la única rama permanente. Contiene el código integrado y es la rama de la que salen todas las ramas de trabajo.

No se hacen commits directos sobre `main`. Todo cambio entra mediante un Pull Request (PR).

## Flujo de integración

```
feature/fix/docs/refactor/chore → main
```

1. Las ramas de trabajo salen de `main`.
2. Cuando el trabajo está listo, se abre un PR de la rama de trabajo a `main`.
3. Tras el merge, la rama de trabajo se elimina.

## Ramas de trabajo

- Cada tarea se desarrolla en una rama corta creada a partir de `main`.
- Nombre de las ramas: `<tipo>/<descripción-corta>`, en inglés, en minúsculas y separadas por guiones.
  - Tipos: `feature/`, `fix/`, `docs/`, `refactor/`, `chore/`.
  - Ejemplos: `feature/oauth-login`, `fix/crash-on-save`.
- Las correcciones urgentes siguen el mismo flujo con una rama `fix/`.
- Las ramas deben ser cortas: se integran en `main` lo antes posible y se eliminan tras el merge.
- Antes de abrir el PR, la rama se actualiza con los últimos cambios de `main`.

## Pull Requests

- El título del PR se escribe en inglés y se corresponde con el título de la especificación de OpenSpec que implementa el cambio.
- Cada PR debe tener una descripción clara del cambio y su motivo, escrita en inglés.
- La descripción del PR no incluye datos del agente de IA utilizado: ni `Co-Authored-By`, ni `Generated with`, ni enlaces o menciones a herramientas o modelos.
- El listado de cambios de la descripción es coherente, claro y conciso. Se redacta como una única lista de viñetas sobre el resultado global del PR, sin separarla por commit.
- Antes de hacer merge, el PR debe tener una revisión aprobada y los checks en verde.
- Todos los merges en `main` se hacen por "squash" para mantener un historial limpio.

## Commits

- Mensajes en imperativo y en una sola línea descriptiva (por ejemplo, `Añade validación del formulario de registro`).
- Cada commit debe dejar el proyecto en un estado que compile y pase las pruebas.

## Versiones

- Las versiones se etiquetan en `main` con tags anotados siguiendo [SemVer](https://semver.org/lang/es/) (`vMAJOR.MINOR.PATCH`).
- Los tags se crean sobre `main` tras el merge de los cambios que forman parte de la versión.
