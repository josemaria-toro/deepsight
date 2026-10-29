# Tecnologías admitidas: React

Guía que incluye las tecnologías admitidas en aplicaciones basadas en React.

Cualquier tecnología que no aparezca en esta lista requiere aprobación previa antes de incorporarla a un proyecto.

## Lenguaje y entorno

| Tecnología | Uso |
|---|---|
| Node.js 22 LTS o superior | Entorno de ejecución y de desarrollo. |
| TypeScript | Lenguaje base, en modo `strict`. |
| `pnpm` | Gestión de dependencias y fichero de bloqueo. |
| `Vite` | Herramienta de desarrollo y empaquetado. |

## Interfaz

| Tecnología | Uso |
|---|---|
| React | Biblioteca de construcción de interfaces. |
| `react-router` | Enrutado y navegación entre vistas de la aplicación. |

## Especificaciones

| Tecnología | Uso |
|---|---|
| `OpenSpec` | Escritura y gestión de especificaciones de cambios, según [la guía de especificaciones](../development/specifications.md). Es una herramienta de desarrollo. No es una dependencia de ejecución del proyecto. |

## Calidad de código

| Tecnología | Uso |
|---|---|
| `prettier` | Formateo de código. |
| `eslint` | Linter. |
| `pre-commit` | Ejecución de comprobaciones antes de cada commit. |

## Pruebas

| Tecnología | Uso |
|---|---|
| `vitest` | Framework de pruebas unitarias y de componentes. |
| `@testing-library/react` | Pruebas de componentes centradas en el comportamiento visible. |
| `playwright` | Pruebas de integración y end-to-end. |
| `Docker` | Ejecución efímera de las pruebas y de los servicios que necesitan. Es una herramienta de desarrollo, no una dependencia de ejecución del proyecto. |

## Seguridad

| Tecnología | Uso |
|---|---|
| `pnpm audit` | Detección de vulnerabilidades en dependencias. |
| `eslint-plugin-security` | Análisis estático de seguridad. |
| `DOMPurify` | Sanitización de HTML antes de usar `dangerouslySetInnerHTML`. |
| Web Crypto API | Cifrado, descifrado y generación de valores aleatorios en el navegador. |

## Datos y validación

| Tecnología | Uso |
|---|---|
| `zod` | Validación de datos tipada. |

## Documentación

| Tecnología | Uso |
|---|---|
| TSDoc | Documentación de componentes, hooks y funciones exportadas. |
