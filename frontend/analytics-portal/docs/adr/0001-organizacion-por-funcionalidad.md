# ADR 0001: Organización por funcionalidad

## Contexto

analytics-portal debe seguir la guía de arquitectura de React del repositorio.

## Decisión

El código se organiza en `app`, `features` y `shared`, con la regla de dependencias `app → features → shared`. El JSX se transforma con el transformador integrado de Vite, sin `@vitejs/plugin-react`, por no estar entre las tecnologías admitidas.

## Consecuencias

- Cada funcionalidad es autocontenida y solo expone su `index.ts`.
- `jsdom` (aprobado expresamente) se usa como entorno DOM de las pruebas de componentes. Se fija en la versión 26 por compatibilidad con Node 20.
