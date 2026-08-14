# Zetatech.DeepSight.Domain
## Introducción
Librería perteneciente a la plataforma **Zetatech DeepSight**, desarrollada por **Zeta Technologies** y que contiene los componentes de la capa de dominio.
## Estructura
```
├─ Zetatech
   ├─ DeepSight
      ├─ Application
         ├─ Abstractions          ' Clases base para los componentes de la capa de dominio.
         ├─ Entities              ' Clases utilizadas en la entrada / salida de los repositorios de la capa de dominio.
         ├─ Publishers            ' Contratos para los publicadores de mensajería.
         ├─ Repositories          ' Contratos para los repositorios de acceso a datos.
```
## Control de versiones
### v10.2609.0
- Se incluyen las clases base para entidades.
- Se incluyen entidades para dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
- Se incluyen los contratos de publicadores de mensajería relacionada con dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
- Se incluyen los contratos de repositorios para el acceso a datos relacionados con dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
