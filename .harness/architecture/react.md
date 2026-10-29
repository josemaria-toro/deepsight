# Guía de arquitectura: React

Guía de arquitectura para los proyectos de frontend basados en React.

## Principio general

- El código se organiza por funcionalidad (`features`), no por tipo de fichero.
- Cada funcionalidad es autocontenida: agrupa sus componentes, hooks, llamadas a la API y tipos.
- Las funcionalidades se comunican solo a través de su API pública, nunca importando ficheros internos de otra funcionalidad.
- Los componentes presentan datos y delegan la lógica en hooks y en la capa de API. No contienen reglas de negocio.

### Regla de dependencias

```
app → features → shared
```

- `shared` no importa de `features` ni de `app`.
- `features` importa de `shared`, pero no de `app`.
- Una funcionalidad no importa de otra funcionalidad salvo a través de su `index.ts`.
- `app` compone las funcionalidades, las rutas y los proveedores globales.

## Estructura de carpetas

```
src/
├── app/
│   ├── App.tsx
│   ├── router.tsx
│   ├── providers/
│   └── main.tsx
├── features/
│   └── <nombre_funcionalidad>/
│       ├── api/
│       ├── components/
│       ├── hooks/
│       ├── model/
│       ├── pages/
│       ├── index.ts
│       └── <nombre_funcionalidad>.test.tsx
└── shared/
    ├── api/
    ├── components/
    ├── hooks/
    ├── lib/
    ├── types/
    └── ui/
tests/
├── e2e/
└── integration/
```

## Descripción de las capas

- **`app/`**: arranque de la aplicación.
  - `main.tsx`: punto de entrada que monta la aplicación.
  - `App.tsx`: composición de proveedores y rutas.
  - `router.tsx`: definición de rutas de `react-router`. Cada ruta apunta a una página de una funcionalidad.
  - `providers/`: proveedores globales, como el de tema o el de sesión.
- **`features/<nombre>/`**: una funcionalidad de negocio. Ejemplo: `features/projects/`.
  - `api/`: llamadas al backend, validadas con `zod`. Ejemplo: `projects.api.ts`.
  - `components/`: componentes de presentación de la funcionalidad.
  - `hooks/`: hooks que conectan los componentes con la API y el estado. Ejemplo: `useProjects.ts`.
  - `model/`: tipos, esquemas `zod` y funciones puras de la funcionalidad.
  - `pages/`: componentes de ruta que componen componentes y hooks.
  - `index.ts`: API pública de la funcionalidad. Es lo único que importan otras capas.
- **`shared/`**: código reutilizable sin conocimiento de negocio.
  - `api/`: cliente HTTP base, con configuración común y manejo de errores.
  - `components/`: componentes compartidos con lógica, como un formulario genérico.
  - `hooks/`: hooks genéricos, como `useDebounce`.
  - `lib/`: utilidades puras, como formateo de fechas.
  - `types/`: tipos globales.
  - `ui/`: componentes visuales sin lógica, como botones o campos de texto.

## Reglas de nombrado

- Los componentes usan `PascalCase` y su fichero tiene el mismo nombre: `ProjectCard.tsx`.
- Los hooks empiezan por `use`: `useProjects`.
- Los ficheros de API llevan el sufijo `.api.ts`: `projects.api.ts`.
- Los esquemas de validación llevan el sufijo `Schema`: `ProjectSchema`.
- Las páginas llevan el sufijo `Page`: `ProjectsPage`.

## Estado y datos

- El estado local vive en el componente o en un hook de la funcionalidad.
- El estado global se usa solo cuando varias funcionalidades lo necesitan, y se expone desde `app/providers/`.
- Los datos del backend se validan con `zod` al recibirlos, antes de usarlos en la interfaz.

## Pruebas por capa

- Los componentes de `shared/ui` y los hooks de `shared` tienen pruebas unitarias.
- Cada funcionalidad tiene pruebas de sus componentes, hooks y funciones de `model/`, junto a su código.
- Las pruebas de integración de varias funcionalidades van en `tests/integration/`.
- Las pruebas end-to-end con `playwright` van en `tests/e2e/` y recorren flujos completos de usuario.
