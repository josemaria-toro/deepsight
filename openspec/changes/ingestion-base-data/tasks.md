# Tasks

## 1. ingestion-api: esquema de entrada

- [x] 1.1 Crear el DTO base en `application/dtos/` y verificar con pruebas unitarias que conserva las propiedades base y que las obligatorias no admiten nulos ni textos vacíos
- [x] 1.2 Crear `ObservabilityDataSchema` en `interfaces/api/schemas/` con alias `camelCase`, `timespan` con zona horaria, `extra_data` y las longitudes máximas, y verificar con pruebas todos los escenarios de `observability-base-data` (aceptación, ausencias, nulos, vacíos, zona horaria, `extra_data`, longitudes, `snake_case` rechazado)
- [x] 1.3 Verificar con una prueba de un tipo de ejemplo en `tests/` que un tipo concreto hereda la base sin redefinirla
- [x] 1.4 Actualizar `docs/classes.md` del proyecto con los diagramas y la definición de las clases nuevas, y verificar que los diagramas Mermaid se renderizan

## 2. ingestion-workers: entidad base y persistencia

- [x] 2.1 Crear la entidad abstracta `ObservabilityData` en `domain/entities/` y verificar con pruebas unitarias la obligatoriedad, las longitudes y que no se puede instanciar sola
- [x] 2.2 Crear el contrato `ObservabilityDataRepository` en `domain/ports/repositories/` y verificar que `mypy` estricto lo valida
- [x] 2.3 Crear `ObservabilityDataModel` en `infrastructure/persistence/models.py` con columna discriminadora `type`, `timespan` con zona horaria, `extra_data` en `JSONB` y tamaños iguales a las longitudes de la entidad, y verificar con una prueba de integración que el esquema se crea en PostgreSQL
- [x] 2.4 Crear el mapper base en `infrastructure/persistence/mappers.py` y verificar con pruebas de integración que un objeto persistido y recuperado conserva todas sus propiedades base
- [x] 2.5 Crear la migración de Alembic de la tabla `observability_data` y verificar que `alembic upgrade head` y `alembic downgrade base` funcionan sobre una base de datos efímera de Docker
- [x] 2.6 Actualizar `docs/classes.md` y registrar la decisión de herencia unida en `docs/adr/`, y verificar que los enlaces del README siguen siendo válidos

## 3. Comprobación final

- [x] 3.1 Ejecutar en ambos proyectos `black --check`, `ruff`, `mypy`, `pytest` con cobertura mínima del 80 % y `bandit`, y verificar que pasan sin errores
- [x] 3.2 Comprobar que cada escenario de la especificación tiene al menos una prueba que lo verifica

## Workflow follow-up

- Archivar el cambio con `/opsx:archive` en la rama de trabajo antes del merge.
