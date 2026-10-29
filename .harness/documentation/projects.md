# Guía de documentación: proyectos

Guía para la documentación de proyectos.

## Idioma

- La documentación de proyectos se redacta en español.
- Los nombres de comandos, rutas, variables de entorno y ficheros se escriben tal como aparecen en el código, en inglés.

## README

- Cada proyecto tiene un `README.md` en la raíz.
- El `README.md` contiene, como mínimo, las siguientes secciones:
  1. **Descripción**: qué hace el proyecto y para quién.
  2. **Requisitos**: versiones de lenguaje, herramientas y dependencias del sistema necesarias.
  3. **Instalación**: pasos exactos para preparar el entorno de desarrollo.
  4. **Ejecución**: comandos para arrancar la aplicación en local.
  5. **Pruebas**: comandos para ejecutar las pruebas unitarias, de integración y end-to-end, y la cobertura.
  6. **Configuración**: variables de entorno y ficheros de configuración, con su finalidad. Nunca se incluyen valores reales de secretos.
  7. **Despliegue**: cómo se publica la aplicación, si aplica, según [la guía de despliegues](../development/deployment.md).
  8. **Contribución**: referencia a la guía de estrategia de ramas (`.harness/development/git.md`) y a los guardarraíles (`.harness/guardrails/`).
  9. **Licencia**: licencia aprobada del proyecto, con referencia al fichero `LICENSE`.
- Los comandos del `README.md` se han probado y funcionan tal como están escritos.

## Documentación técnica obligatoria

Todo proyecto debe disponer de la siguiente documentación técnica en `docs/`:

1. **Diagramas de arquitectura**: muestran los componentes del sistema, sus responsabilidades y cómo se comunican.
2. **Diagramas de clases**: muestran las clases del proyecto y sus relaciones.
3. **Estructura y definición de clases**: documento que describe cada clase, sus métodos, propiedades y parámetros, con su finalidad.
4. **Ejemplos de uso**: fragmentos de código que muestran cómo usar los componentes principales del proyecto.

- Los diagramas se escriben en Mermaid, dentro de ficheros Markdown, para que se versionen junto con el código.
- Los ejemplos de uso son código ejecutable y se mantienen en el repositorio. Deben actualizarse cuando cambie la interfaz que muestran.

## Otros documentos

- Los documentos de arquitectura y decisiones técnicas se guardan en `docs/`.
- Cada decisión técnica relevante se registra en `docs/adr/` como un registro de decisión (ADR), numerado de forma secuencial, con el contexto, la decisión y sus consecuencias.
- Las especificaciones de OpenSpec se guardan en `openspec/` en la raíz del proyecto, según [la guía de especificaciones](../development/specifications.md). El `README.md` indica la versión de OpenSpec que usa el proyecto.
- El historial de cambios se mantiene en `CHANGELOG.md`, agrupado por versión siguiendo [SemVer](https://semver.org/lang/es/) y con las secciones `Añadido`, `Cambiado`, `Corregido` y `Eliminado`.

## Mantenimiento

- La documentación se actualiza en el mismo PR que el cambio de código que la afecta.
- Un PR que modifica el comportamiento de un proyecto sin actualizar su documentación debe corregirse antes del merge.
