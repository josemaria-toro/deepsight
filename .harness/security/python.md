# Guía de seguridad: Python

Guía de seguridad para aplicaciones basadas en Python.

## Gestión de secretos

- Ningún secreto se escribe en el código ni en ficheros versionados. Esto incluye contraseñas, claves de API, tokens y cadenas de conexión.
- Los secretos se leen desde variables de entorno o desde un gestor de secretos.
- Los ficheros de configuración local con secretos (`.env`, etc.) se excluyen de Git mediante `.gitignore`.
- Si un secreto se expone por error, se revoca y se rota de inmediato. Eliminarlo del código no basta.
- Los secretos no se registran en logs, mensajes de error ni trazas.

## Validación de entradas

- Toda entrada externa (peticiones HTTP, ficheros, variables de entorno, respuestas de servicios externos) se valida antes de usarla.
- Usar modelos tipados para validar datos, por ejemplo `pydantic`.
- No confiar en validaciones solo del lado del cliente.

## Inyección

- Las consultas SQL usan siempre parámetros enlazados. Nunca se construyen concatenando cadenas.
- No usar `eval`, `exec` ni `compile` sobre datos que no sean de confianza.
- Al invocar procesos externos con `subprocess`, usar listas de argumentos y no `shell=True`.
- Las rutas construidas a partir de entradas se normalizan y se comprueba que permanecen dentro del directorio permitido.

## Deserialización

- No usar `pickle`, `marshal` ni `shelve` con datos que no sean de confianza.
- Para intercambio de datos usar formatos sin ejecución de código, como JSON.
- Usar `yaml.safe_load` en lugar de `yaml.load`.

## Autenticación y criptografía

- Las contraseñas se almacenan solo como hash con un algoritmo de derivación diseñado para ello, como `argon2` o `bcrypt`. Nunca con MD5, SHA-1 ni SHA-256 sin sal y sin derivación.
- Para generar valores aleatorios de seguridad usar el módulo `secrets`, no `random`.
- Las comunicaciones externas usan TLS con verificación de certificados activa. No desactivar `verify` en peticiones HTTP.
- No implementar criptografía propia. Usar bibliotecas establecidas.

## Errores y datos expuestos

- Las respuestas de error al usuario son genéricas. Los detalles técnicos se registran internamente.
- Debug desactivado en producción.
- No registrar datos personales ni información sensible en logs.

## Dependencias

- Las dependencias se revisan con `pip-audit` en cada ejecución de la integración continua.
- Una vulnerabilidad conocida de severidad alta o crítica bloquea el merge.
- Los CVE reportados se verifican en fuentes oficiales: [cve.org](https://www.cve.org) e [INCIBE](https://www.incibe.es) (incluido su catálogo de vulnerabilidades).

## Análisis estático

- La integración continua ejecuta `bandit` sobre `src/`.
- Los hallazgos de severidad alta bloquean el merge. Las excepciones se justifican con un comentario `# nosec` que explique el motivo.

## Privilegios

- La aplicación se ejecuta con el mínimo de privilegios necesario.
- No se ejecuta como `root` en contenedores ni en servidores.
