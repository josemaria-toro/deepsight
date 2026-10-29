# ADR 0001: Arquitectura hexagonal con DDD

## Contexto

ingestion-api es un proyecto de backend de DeepSight y debe seguir la guía de arquitectura de Python del repositorio.

## Decisión

Se adopta DDD con arquitectura hexagonal y Clean Architecture, con las capas `domain`, `application`, `infrastructure` e `interfaces` y la regla de dependencias de la guía.

## Consecuencias

- El dominio no depende de frameworks.
- Todo acceso externo se define primero como puerto en `domain/ports/`.
