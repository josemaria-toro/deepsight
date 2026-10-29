# Guía de desarrollo: React

Guía para el desarrollo de aplicaciones basadas en React.

## Versión y entorno

- Usar Node.js 22 LTS o superior.
- Usar TypeScript en todo el código. No se admite JavaScript sin tipar en código nuevo.
- Gestionar las dependencias con `pnpm` y versionar el fichero de bloqueo (`pnpm-lock.yaml`) en Git.
- Las versiones de Node y pnpm se fijan en el proyecto (`engines` en `package.json` y `.nvmrc`).

## Estructura del proyecto

- El código fuente vive en `src/`.
- Los componentes se agrupan por funcionalidad (`src/features/<nombre>/`), no por tipo de fichero.
- Las pruebas se ubican junto al código que prueban (`<nombre>.test.tsx`) o en `tests/` para las pruebas de integración y end-to-end.
- No se incluyen en Git artefactos generados (`node_modules`, `dist/`, cachés de herramientas).

## Idioma

- El código fuente se escribe siempre en inglés: módulos, paquetes, componentes, hooks, funciones, clases, propiedades, variables, tipos y constantes.
- Los docstrings (TSDoc) se escriben en español.
- Los comentarios se escriben en español.
- Los mensajes de log y los errores lanzados desde el código se escriben en inglés, ya que forman parte de la interfaz técnica del código.

## Estilo de código

- El formateo se hace con `prettier` y la comprobación de estilo con `eslint`.
- Los componentes usan `PascalCase`. Hooks, funciones y variables usan `camelCase`. Las constantes globales usan `UPPER_SNAKE_CASE`.
- Los componentes son funciones. No se usan componentes de clase.
- Los nombres de hooks comienzan por `use`.
- Los miembros de las clases y de los objetos se ordenan por tipo: primero los constructores, después las propiedades y por último los métodos.
- Dentro de cada grupo, los miembros se ordenan alfabéticamente: primero los públicos y después los privados.
- Cada fichero contiene un único tipo (componente, clase, enumeración, `interface` o `type`). El nombre del fichero corresponde al del tipo que define. Las `props` de un componente se consideran parte de este y pueden declararse en su mismo fichero.

## Componentes y estado

- Un componente debe tener una sola responsabilidad. Si crece, se divide en componentes más pequeños.
- Usar hooks para la lógica reutilizable. No duplicar lógica entre componentes.
- Preferir el estado local al estado global. Solo se usa estado global cuando varios componentes distantes lo necesitan.
- No usar `dangerouslySetInnerHTML` sin sanitizar previamente el contenido.

## Tipado

- `tsconfig.json` activa `strict: true`.
- No usar `any`. Si es imprescindible, se justifica con un comentario.
- Las props de los componentes se definen con tipos explícitos.

## Pruebas

- Usar `vitest` y `@testing-library/react` para las pruebas unitarias y de componentes.
- Las pruebas de integración y end-to-end usan `playwright`.
- Las pruebas se centran en el comportamiento visible para el usuario, no en detalles internos de implementación.
- Las pruebas deben ser deterministas: no dependen del orden de ejecución, del reloj del sistema ni de la red, salvo en las pruebas end-to-end.
- Cobertura mínima obligatoria: 80 % de todo el código del proyecto (ver [guardarraíles](../guardrails/react.md)).
- Las pruebas se ejecutan de forma efímera en contenedores de Docker: cada ejecución crea sus contenedores (base de datos, broker y demás servicios) y los elimina al terminar, sin dejar estado entre ejecuciones.
- Si Docker no está instalado en el sistema, se recomienda instalarlo antes de ejecutar las pruebas, siguiendo la [documentación oficial](https://docs.docker.com/engine/install/). La instalación puede requerir permisos elevados.

## Documentación en el código

- Documentar con TSDoc los componentes, hooks y funciones exportadas. Las descripciones se escriben en español.
- Los comentarios explican el *porqué*, no el *qué*.

## Herramientas de calidad

- Configurar `pre-commit` con `prettier`, `eslint` y `tsc --noEmit` para ejecutar las comprobaciones antes de cada commit.
- La integración continua debe ejecutar `prettier --check`, `eslint`, `tsc --noEmit`, `vitest` y `playwright`. Un fallo en cualquiera bloquea el merge.

## Dependencias

- Añadir solo las dependencias necesarias y mantenerlas actualizadas.
- Antes de añadir una dependencia, comprobar que tiene licencia compatible con GPL v3.
