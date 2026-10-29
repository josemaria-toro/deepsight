# ADR 0002: Herencia unida para los datos de observabilidad

## Contexto

Todos los tipos de dato de observabilidad comparten una estructura base (`observability-base-data`) y añaden propiedades propias. Hay que decidir cómo persistirlos en PostgreSQL.

## Decisión

Se usa herencia unida: una tabla base `observability_data` con las propiedades comunes y la columna discriminadora `type`, y una tabla por tipo concreto con clave primaria y foránea hacia la base.

Se descartan la tabla única, por las muchas columnas nulas y la mezcla de tipos, y una tabla por tipo con las propiedades base duplicadas, porque complica las consultas transversales por `trace_id`.

## Consecuencias

- Las consultas por `trace_id` o por instante recorren una sola tabla.
- Leer un tipo concreto requiere un `JOIN` con la tabla base.
- La entidad base es abstracta y no se persiste sola, por lo que su repositorio es un contrato genérico que cada tipo concreto especializa.
