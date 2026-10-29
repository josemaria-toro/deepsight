# Guardarraíles: React

Guía que incluye los guardarraíles definidos para aplicaciones basadas en React.

## Pruebas obligatorias

- Las pruebas unitarias, de integración y end-to-end son obligatorias.
- Ningún merge se puede completar si las pruebas unitarias, de integración o end-to-end fallan, no se ejecutan o no existen para el código modificado.
- Estas pruebas bloquean cualquier merge en `main`.

## Cobertura de código

- Es obligatorio alcanzar una cobertura de código del 80 %.
- Si la cobertura es inferior al 80 %, se bloquea el merge.
- Este requisito aplica a cualquier merge en `main`.

## Información sensible

- No se permite incluir en el código contraseñas, claves, tokens ni ningún otro tipo de información sensible.
- La presencia de este tipo de información en el código bloquea el merge.
- Este requisito aplica a cualquier merge en `main`.

## Vulnerabilidades en librerías

- La detección de una vulnerabilidad de severidad alta o crítica en una librería utilizada por el proyecto bloquea el merge.
- Este requisito aplica a cualquier merge en `main`.

## Tecnologías no aprobadas

- Solo se pueden usar las tecnologías incluidas en la guía de tecnologías admitidas.
- El uso de una tecnología no aprobada bloquea el merge.
- Este requisito aplica a cualquier merge en `main`.

## Licencia

- La licencia del proyecto debe ser la definida en la guía de licencias para React (GNU GPL versión 3).
- Si la licencia no coincide con la definida, se bloquea el merge.
- Este requisito aplica a cualquier merge en `main`.

## Referencias limpias y ordenadas

- Las referencias no utilizadas (importaciones, variables, espacios de nombres) se eliminan.
- Las importaciones, espacios de nombres y métodos se ordenan correctamente según las reglas definidas en la guía de desarrollo.
- Si las referencias no están limpias u ordenadas, se bloquea el merge.
- Este requisito aplica a cualquier merge en `main`.

## Documentación actualizada

- La documentación del proyecto debe estar actualizada en el mismo PR que el cambio de código que la afecta.
- Si la documentación no queda actualizada, se bloquea el merge.
- Este requisito aplica a cualquier merge en `main`.
