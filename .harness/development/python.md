# Guía de desarrollo: Python

Guía para el desarrollo de aplicaciones basadas en Python.

## Versión y entorno

- Usar Python 3.13 o superior.
- Cada proyecto gestiona sus dependencias con `pyproject.toml`.
- Crear un entorno virtual por proyecto (`.venv`). Nunca instalar dependencias en el intérprete global.
- El fichero de bloqueo de dependencias (`uv.lock` o equivalente) se versiona en Git.

## Estructura del proyecto

- El código fuente vive en `src/<nombre_paquete>/`.
- Las pruebas viven en `tests/` y reflejan la estructura de `src/`.
- Los scripts de ejecución y utilidades van en `scripts/`, nunca mezclados con el paquete.
- No se incluyen en Git artefactos generados (`__pycache__`, `.venv`, cachés de herramientas, `dist/`).

## Idioma

- El código fuente se escribe siempre en inglés: módulos, paquetes, clases, métodos, funciones, propiedades, variables y constantes.
- Los docstrings se escriben en español.
- Los comentarios se escriben en español.
- Los mensajes de log y las excepciones se escriben en inglés, ya que forman parte de la interfaz técnica del código.

## Estilo de código

- Seguir PEP 8. El formateo se hace con `black` y la comprobación de estilo con `ruff` (linter).
- Los nombres de módulos, funciones y variables usan `snake_case`. Las clases usan `PascalCase`. Las constantes usan `UPPER_SNAKE_CASE`.
- Las funciones públicas deben tener anotaciones de tipo completas.
- Evitar `from módulo import *`.
- Las importaciones de referencias internas del proyecto se hacen con ruta absoluta desde el paquete raíz (`from app.domain.user import User`), nunca con rutas relativas (`from .user import User`, `from ..domain.user import User`). Las importaciones de bibliotecas externas y de la biblioteca estándar también son absolutas.
- Las importaciones se ordenan cada vez que se añada o elimine alguna, en tres grupos separados por una línea en blanco: biblioteca estándar, bibliotecas externas y referencias internas del proyecto. Dentro de cada grupo se ordenan alfabéticamente. La ordenación se comprueba con la regla `I` (isort) de `ruff`.
- No usar valores mutables como argumentos por defecto.
- Los miembros de una clase se ordenan por tipo: primero los constructores, después las propiedades y por último los métodos.
- Dentro de cada grupo, los miembros se ordenan alfabéticamente: primero los públicos y después los privados.
- Cada fichero contiene un único tipo (clase, enumeración, `Protocol`, alias de tipo o similar). El nombre del fichero en `snake_case` corresponde al del tipo que define.

## Tipado

- Todo el código nuevo debe pasar `mypy` en modo estricto (`strict = true`).
- Usar tipos modernos: `list[str]`, `dict[str, int]`, `X | None`. No usar `typing.List`, `typing.Dict` ni `Optional`.

## Serialización JSON

- Las propiedades de los datos serializados en JSON se exponen siempre en `camelCase`. Nunca en `snake_case`.
- Los atributos internos del código Python siguen usando `snake_case` según PEP 8. La conversión a `camelCase` se hace solo en la capa de serialización (por ejemplo, con alias de `pydantic`).

## Manejo de errores y logging

- Capturar excepciones concretas. Evitar `except Exception` sin relanzar o registrar el error.
- Usar el módulo `logging`. No usar `print` para información de ejecución.
- No registrar secretos, tokens ni datos personales.

## Pruebas

- Usar `pytest` como framework de pruebas.
- Cada módulo de lógica debe tener pruebas unitarias. Las pruebas de integración se separan con un marcador (`@pytest.mark.integration`).
- Las pruebas deben ser deterministas: no dependen del orden de ejecución, del reloj del sistema ni de la red, salvo que estén marcadas como integración.
- Cobertura mínima obligatoria: 80 % de todo el código del proyecto (ver [guardarraíles](../guardrails/python.md)).
- Las pruebas se ejecutan de forma efímera en contenedores de Docker: cada ejecución crea sus contenedores (base de datos, broker y demás servicios) y los elimina al terminar, sin dejar estado entre ejecuciones.
- Si Docker no está instalado en el sistema, se recomienda instalarlo antes de ejecutar las pruebas, siguiendo la [documentación oficial](https://docs.docker.com/engine/install/). La instalación puede requerir permisos elevados.

## Documentación en el código

- Docstrings en formato Google para módulos, clases y funciones públicas.
- Los comentarios explican el *porqué*, no el *qué*.

## Herramientas de calidad

- Configurar `pre-commit` con `black`, `ruff` y `mypy` para ejecutar las comprobaciones antes de cada commit.
- La integración continua debe ejecutar `black --check`, `ruff`, `mypy` y `pytest`. Un fallo en cualquiera bloquea el merge.
- La configuración de `ruff` no puede contradecir las reglas de esta guía. En concreto, la regla `TID252` se activa con `ban-relative-imports = "all"` para prohibir todas las importaciones relativas, y la regla `I` (ordenación de importaciones) está activa en `[tool.ruff.lint]`, con el paquete raíz del proyecto declarado como interno (`known-first-party`).

## Dependencias

- Añadir solo las dependencias necesarias y mantenerlas actualizadas.
- Fijar versiones mínimas en `pyproject.toml` y dejar la resolución exacta al fichero de bloqueo.
- Antes de añadir una dependencia, comprobar que tiene licencia compatible con GPL v3.
