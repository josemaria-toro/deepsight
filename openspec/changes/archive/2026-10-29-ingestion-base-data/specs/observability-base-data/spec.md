# Spec Delta

## Purpose

Define la estructura base común a todos los objetos de observabilidad ingeridos por DeepSight, de modo que cualquier tipo concreto de dato pueda identificarse, correlacionarse y procesarse de forma uniforme.

## ADDED Requirements

### Requirement: Base properties of an observability object
Todo objeto de observabilidad SHALL contener las propiedades base `app_name`, `app_version`, `client_ip_address`, `client_version`, `extra_data`, `host_name`, `span_id`, `timespan` y `trace_id`, además de las propias de su tipo.

#### Scenario: Object with all base properties
- **WHEN** se recibe un objeto con todas las propiedades base informadas
- **THEN** el objeto se acepta y conserva el valor de cada propiedad base

#### Scenario: Type-specific properties are preserved
- **WHEN** se recibe un objeto de un tipo concreto con las propiedades base y las propias de su tipo
- **THEN** el objeto contiene tanto las propiedades base como las propias del tipo

### Requirement: Mandatory base properties
Las propiedades `app_name`, `app_version`, `span_id`, `timespan` y `trace_id` SHALL ser obligatorias y no admitir valores nulos ni textos vacíos.

#### Scenario: Missing mandatory property
- **WHEN** se recibe un objeto sin alguna de las propiedades obligatorias
- **THEN** el objeto se rechaza con un error de validación que indica la propiedad ausente

#### Scenario: Null or empty mandatory property
- **WHEN** se recibe un objeto cuya propiedad obligatoria es nula o un texto vacío
- **THEN** el objeto se rechaza con un error de validación que indica la propiedad afectada

### Requirement: Optional base properties
Las propiedades `client_ip_address`, `client_version`, `extra_data` y `host_name` SHALL ser opcionales y admitir valores nulos.

#### Scenario: Object without optional properties
- **WHEN** se recibe un objeto que omite las propiedades opcionales o las informa como nulas
- **THEN** el objeto se acepta y las propiedades opcionales quedan sin valor

### Requirement: Timestamp with time zone
La propiedad `timespan` SHALL representar una fecha y hora con zona horaria, y se serializa en formato ISO 8601.

#### Scenario: Valid timestamp
- **WHEN** se recibe `timespan` con una fecha y hora ISO 8601 que incluye zona horaria
- **THEN** el objeto se acepta y el instante se conserva sin cambios

#### Scenario: Timestamp without time zone
- **WHEN** se recibe `timespan` sin zona horaria o con un formato no ISO 8601
- **THEN** el objeto se rechaza con un error de validación sobre `timespan`

### Requirement: Free-form extra data
La propiedad `extra_data` SHALL ser un diccionario cuyas claves son textos y cuyos valores son objetos JSON arbitrarios.

#### Scenario: Valid extra data
- **WHEN** se recibe `extra_data` con claves de texto y valores de cualquier tipo JSON, incluidos objetos anidados
- **THEN** el objeto se acepta y `extra_data` conserva la estructura recibida

#### Scenario: Extra data is not a dictionary
- **WHEN** se recibe `extra_data` con un valor que no es un diccionario de claves de texto
- **THEN** el objeto se rechaza con un error de validación sobre `extra_data`

### Requirement: Maximum length of text properties
Las propiedades de texto de la base SHALL respetar estas longitudes máximas: `app_name` 255, `app_version` 64, `client_ip_address` 45, `client_version` 64, `host_name` 255, `span_id` 64 y `trace_id` 64 caracteres.

#### Scenario: Text within the limit
- **WHEN** se recibe un objeto cuyas propiedades de texto no superan su longitud máxima
- **THEN** el objeto se acepta

#### Scenario: Text over the limit
- **WHEN** se recibe un objeto con una propiedad de texto que supera su longitud máxima
- **THEN** el objeto se rechaza con un error de validación sobre esa propiedad

### Requirement: JSON representation in camelCase
Las propiedades base SHALL intercambiarse en JSON en `camelCase`: `appName`, `appVersion`, `clientIpAddress`, `clientVersion`, `extraData`, `hostName`, `spanId`, `timespan` y `traceId`.

#### Scenario: JSON uses camelCase
- **WHEN** se recibe o se emite un objeto en JSON
- **THEN** las propiedades base aparecen con sus nombres en `camelCase` y nunca en `snake_case`

#### Scenario: Snake case is not accepted
- **WHEN** se recibe un objeto cuyas propiedades base llegan en `snake_case`
- **THEN** el objeto se rechaza porque faltan las propiedades obligatorias en `camelCase`

### Requirement: Base is extensible by concrete types
La estructura base SHALL poder heredarse por los tipos concretos de dato sin que estos redefinan ni eliminen ninguna propiedad base.

#### Scenario: Concrete type inherits the base
- **WHEN** se define un tipo concreto de dato
- **THEN** incluye todas las propiedades base con la misma obligatoriedad y validación

#### Scenario: Persisted base properties are recoverable
- **WHEN** un objeto válido se persiste y se recupera
- **THEN** todas sus propiedades base tienen el mismo valor que tenían al recibirse
