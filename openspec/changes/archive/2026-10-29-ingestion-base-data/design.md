# Design

## Context

Ver `proposal.md` para la motivación. `ingestion-api` recibe los datos y los publica en RabbitMQ; `ingestion-workers` los consume y los persiste en PostgreSQL. Ambos son proyectos independientes, con el paquete `deepsight` y la arquitectura hexagonal de [la guía de Python](../../../.harness/architecture/python.md). La estructura base debe estar disponible en los dos y servir de punto de partida para los tipos de los tickets #4 a #12.

## Goals / Non-Goals

**Goals:**
- Definir una sola vez, en cada proyecto, la estructura base y su validación.
- Que los tipos concretos la hereden sin duplicar propiedades.

**Non-Goals:**
- Endpoints de ingesta, mensajería y consumidores: se especifican con los tipos concretos.
- Ningún tipo concreto de dato.

## Decisions

- **Base abstracta en cada proyecto.** `ingestion-api` define `ObservabilityDataSchema` (Pydantic, `interfaces/api/schemas/`) y su DTO equivalente en `application/dtos/`. `ingestion-workers` define la entidad abstracta `ObservabilityData` en `domain/entities/`. Se descarta una librería compartida entre proyectos porque añadiría un artefacto y un acoplamiento de versiones que hoy no se justifican; el contrato común es el JSON de esta especificación.
- **Serialización.** El esquema usa alias `camelCase` con `alias_generator=to_camel` y `populate_by_name=False`, de modo que `snake_case` se rechaza (escenario de la especificación). Los atributos Python siguen en `snake_case`.
- **`timespan`.** `datetime` con zona horaria obligatoria (`AwareDatetime`). Se almacena en UTC (`TIMESTAMP WITH TIME ZONE`).
- **`extra_data`.** `dict[str, Any]` en el dominio y columna `JSONB` en PostgreSQL.
- **Persistencia por herencia unida.** La tabla base `observability_data` guarda las propiedades base y una columna discriminadora `type`; cada tipo concreto añade su tabla con clave primaria y foránea hacia la base. Se descartan la tabla única (muchas columnas nulas, mezcla de tipos) y una tabla por tipo (duplica las propiedades base y complica las consultas transversales por `trace_id`).
- **Identidad.** La base añade una clave primaria interna `id` (UUID) generada por el sistema. No forma parte del contrato de entrada.
- **Longitudes.** Las de la especificación son las del dominio y coinciden con el tamaño de las columnas, como exige la guía. Se han elegido como valores razonables porque el ticket no las fija.
- **Alcance de la guía de entidades.** Por ser abstracta, la base entrega modelo ORM base (`ObservabilityDataModel`), mapper base y contrato de repositorio genérico (`ObservabilityDataRepository`). Cada repositorio SQLAlchemy concreto se crea con su tipo, y no antes, porque la base nunca se persiste sola.

## Risks / Trade-offs

- [El contrato JSON se duplica en dos proyectos] → Las pruebas de ambos verifican los mismos escenarios de la especificación.
- [Longitudes asumidas pueden quedarse cortas] → Ampliarlas requiere una migración y un cambio de especificación; se revisarán con el negocio al validar la propuesta.
- [Herencia unida exige `JOIN` en cada lectura de un tipo] → Aceptable al volumen esperado; se revisará con datos reales.
