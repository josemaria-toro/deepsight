# Guía de desarrollo: especificaciones

Guía para el desarrollo de especificaciones con OpenSpec. Se aplica a todos los proyectos, con independencia de su lenguaje.

## Herramienta

- Las especificaciones se escriben y gestionan con OpenSpec.
- Cada proyecto mantiene su carpeta `openspec/` en la raíz del repositorio, versionada junto con el código.
- La versión de OpenSpec se fija en el proyecto. Los comandos se ejecutan con esa versión.

## Estructura

```
openspec/
├── specs/              # Especificaciones vigentes. Fuente de verdad del comportamiento del proyecto.
└── changes/            # Cambios en curso, pendientes de implementar o de archivar.
    └── archive/        # Cambios ya implementados y archivados, con fecha.
```

Cada cambio vive en `openspec/changes/<id-cambio>/` con los siguientes ficheros:

- `proposal.md`: por qué se hace el cambio y qué alcance tiene.
- `specs/<capacidad>/spec.md`: requisitos del cambio en formato delta (`ADDED`, `MODIFIED`, `REMOVED`) con escenarios.
- `design.md`: enfoque técnico, si el cambio lo necesita.
- `tasks.md`: lista de tareas de implementación con casillas.

## Cuándo se usa

- Toda funcionalidad nueva, cambio de comportamiento o cambio de contrato (API, eventos, esquemas de datos) requiere un cambio de OpenSpec antes de escribir el código.
- Las correcciones de errores se especifican solo si cambian el comportamiento esperado. Una corrección que restaura el comportamiento ya especificado no requiere cambio.
- Los cambios puramente técnicos sin efecto en el comportamiento (formateo, renombrados, actualizaciones de dependencias) no requieren especificación.

## Flujo

Toda especificación sigue el flujo de OpenSpec en tres fases obligatorias y en este orden: **propose**, **apply** y **archive**. No se puede omitir ninguna fase ni cambiar su orden.

1. **Propose** (`/opsx:propose`): crea el cambio en `openspec/changes/<id-cambio>/` con `proposal.md`, las especificaciones delta, `design.md` si aplica y `tasks.md`.
   - Los artefactos se generan con el flujo de OpenSpec, no a mano.
   - El cambio se valida con `openspec validate <id-cambio>` antes de pasar a la siguiente fase.
   - No se escribe código de implementación hasta que la propuesta está completa y validada.
2. **Apply** (`/opsx:apply`): implementa el cambio siguiendo las tareas de `tasks.md` en orden.
   - Cada tarea se marca como completada solo cuando cumple sus escenarios y tiene sus pruebas.
   - Si durante la implementación cambia el plan, los artefactos se actualizan con `/opsx:update` antes de continuar. El código y la especificación no pueden quedar en desacuerdo.
3. **Archive** (`/opsx:archive`): con todas las tareas completadas, archiva el cambio. Las especificaciones delta se incorporan a `openspec/specs/` y el cambio pasa a `openspec/changes/archive/` con fecha.
   - El archivado se hace en la rama de trabajo, antes del merge, para que el PR incluya la especificación, el código y el archivado.

La fase de exploración (`/opsx:explore`) es opcional y se puede usar antes de `propose` para aclarar requisitos. No sustituye a ninguna de las tres fases obligatorias.

### Revisión

- El PR contiene el cambio completo: propuesta, implementación y archivado.
- Si la especificación requiere aprobación antes de implementarla, se abre un PR previo solo con la fase `propose`. La implementación y el archivado van después en otro PR.
- No se hace merge en `main` de un PR que implementa un cambio sin archivarlo, salvo en el caso anterior de PR previo de propuesta.

## Reglas

- `openspec/specs/` solo se modifica a través de un cambio archivado. No se editan especificaciones vigentes directamente.
- Cada escenario de una especificación tiene al menos una prueba que lo verifica. Las pruebas siguen las guías de pruebas del lenguaje correspondiente.
- Un PR que implementa un cambio referencia su identificador en la descripción.
- Las especificaciones se escriben en español, como el resto de la documentación. Los identificadores de cambio y capacidad se escriben en minúsculas y separados por guiones, en inglés.
- Si la implementación se aparta de la especificación, el cambio se actualiza antes del merge. El código y la especificación no pueden quedar en desacuerdo.

## Relación con otras guías

- Ramas y PR: [git.md](git.md). Las ramas de un cambio siguen el formato `feature/<id-cambio>` o `fix/<id-cambio>`.
- Documentación de proyectos: [../documentation/projects.md](../documentation/projects.md). La carpeta `openspec/` se documenta en el `README.md` del proyecto.
- Tecnologías admitidas: [../technologies/python.md](../technologies/python.md) y [../technologies/react.md](../technologies/react.md).
