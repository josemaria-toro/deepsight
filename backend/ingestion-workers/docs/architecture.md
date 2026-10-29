# Arquitectura de ingestion-workers

Procesos que consumen los mensajes publicados por la API de ingesta y los persisten en PostgreSQL.

El proyecto sigue DDD con arquitectura hexagonal y Clean Architecture, según [la guía de arquitectura de Python](../../../.harness/architecture/python.md).

```mermaid
flowchart LR
    interfaces[interfaces] --> application[application]
    application --> domain[domain]
    infrastructure[infrastructure] --> domain
    infrastructure --> application
```

| Capa | Responsabilidad |
|---|---|
| `domain` | Núcleo del negocio: entidades, puertos y excepciones. Sin dependencias externas. |
| `application` | Casos de uso y DTO. |
| `infrastructure` | Adaptadores de salida: configuración, persistencia y mensajería. |
| `interfaces` | Adaptadores de entrada: CLI y runner de los workers. |
