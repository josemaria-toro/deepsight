# Zetatech.DeepSight.Application
## Introducción
Librería perteneciente a la plataforma **Zetatech DeepSight**, desarrollada por **Zeta Technologies** y que contiene los componentes de la capa de aplicación.
## Estructura
```
├─ Zetatech
   ├─ DeepSight
      ├─ Application
         ├─ Abstractions          ' Clases base para los componentes de la capa de aplicación.
         ├─ Builders              ' Clases de extensión para la construcción de objetos a partir de otros objetos.
         ├─ Dtos                  ' Clases utilizadas en la entrada / salida de los servicios de la capa de aplicación.
         ├─ Services              ' Contratos para los servicios de la capa de aplicación.
         ├─ Subscribers           ' Contratos para los suscriptores de mensajería.
```
## Control de versiones
### v10.2609.0
- Se incluyen las clases base para DTOs.
- Se incluyen clases constructoras para objetos de tipo DTO y entidades.
- Se incluye DTO genérico para exponerlo a través de la API y la mensajería.
- Se incluyen DTOs para dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
- Se incluyen los contratos de servicios para la gestión de dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
- Se incluyen los contratos de subscriptores para procesar la mensajería relacionada con dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
