# analytics-portal

## Descripción

Portal analítico de DeepSight: web donde visualizar los datos de forma estructurada, con gráficos en tiempo real y posibilidad de filtrar la información. Consume `analytics-api`.

## Requisitos

- Node.js 22 LTS (la guía lo exige; el entorno inicial usa 20.19.x, desviación aceptada, ver `.nvmrc`).
- `pnpm` 10 o superior.
- Docker, para ejecutar las pruebas de forma efímera.

## Instalación

```bash
cd frontend/analytics-portal
pnpm install
pnpm exec playwright install chromium
```

Los hooks de `pre-commit` se instalan desde la raíz del repositorio con `pre-commit install`.

## Ejecución

```bash
pnpm dev
```

La aplicación queda disponible en `http://localhost:5173`.

## Pruebas

```bash
# Pruebas en un contenedor efímero de Docker (recomendado)
./scripts/test.sh

# Unitarias, de componentes e integración, con cobertura mínima del 80 %
pnpm test

# End-to-end con Playwright
pnpm test:e2e

# Comprobaciones de calidad
pnpm format:check
pnpm lint
pnpm typecheck
pnpm audit
```

## Configuración

Sin variables de entorno por ahora. Cuando se añadan, se documentarán aquí sin valores reales.

## Despliegue

Se sigue [la guía de despliegues](../../.harness/development/deployment.md). `pnpm build` genera `dist/`.

## Documentación

- [Arquitectura](docs/architecture.md)
- [Componentes](docs/classes.md)
- [Ejemplos de uso](docs/examples.md)
- Decisiones técnicas en [`docs/adr/`](docs/adr/).

## Contribución

Consulta [la guía de ramas](../../.harness/development/git.md) y los [guardarraíles de React](../../.harness/guardrails/react.md). Las especificaciones viven en [`openspec/`](../../openspec/) (OpenSpec 1.14.3).

## Licencia

GNU GPL versión 3. Consulta el fichero [`LICENSE`](LICENSE).
