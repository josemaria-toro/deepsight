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
- Nombre de las ramas: `<tipo>/<n-ticket>-<descripción-corta>`, en inglés, en minúsculas y separadas por guiones, donde `<n-ticket>` es el número del ticket asociado.
  - Tipos: `feature/`, `fix/`, `docs/`, `refactor/`, `chore/`.
  - Ejemplos: `feature/12-oauth-login`, `fix/27-crash-on-save`.
- Las correcciones urgentes siguen el mismo flujo con una rama `fix/`.
- Las ramas deben ser cortas: se integran en `main` lo antes posible y se eliminan tras el merge.
- Antes de abrir el PR, la rama se actualiza con los últimos cambios de `main`.

## Tickets

- Todo el trabajo parte de un ticket (issue de GitHub) y su contenido es la fuente de la tarea. Si no existe ticket, se crea antes de empezar.
- Cada rama de trabajo se asocia a un único ticket, cuyo número va en el nombre de la rama.
- Cada PR se asocia al ticket de su rama. La descripción del PR incluye una palabra de cierre de GitHub con el ticket (`Closes #<n-ticket>`) para que el ticket quede cerrado automáticamente al hacer merge.
- Un ticket solo se da por completado cuando su PR se ha integrado en `main`. Si al hacer merge el ticket sigue abierto, se cierra manualmente con un comentario que enlace al PR.
- Si el trabajo de un ticket se divide en varios PR, solo el último incluye la palabra de cierre. Los anteriores lo referencian con `Refs #<n-ticket>`.

## Pull Requests

- El título del PR se escribe en inglés y se corresponde con el título de la especificación de OpenSpec que implementa el cambio.
- Cada PR referencia su ticket en la descripción (ver [Tickets](#tickets)).
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
