# Zetatech.DeepSight.Infrastructure
## Introducción
Librería perteneciente a la plataforma **Zetatech DeepSight**, desarrollada por **Zeta Technologies** y que contiene los componentes de la capa de infraestructura.
## Estructura
```
├─ Zetatech
   ├─ DeepSight
      ├─ DependencyInjection      ' Métodos de extensión para el registro de componentes en el contenedor de dependencias.
      ├─ Infrastructure
         ├─ Abstractions          ' Clases base para los componentes de la capa de infraestructura.
         ├─ Persistencia          ' Clases especializadas (repositorios) en el acceso a los datos.
         ├─ Publishers            ' Clases especializadas (publicadores) para la publicación de mensajería.
         ├─ Services              ' Implementación de los servicios de la capa de aplicación.
         ├─ Subscribers           ' Clases especializadas (subscriptores) para el procesamiento de la mensajería.
```
## Control de versiones
### v10.2609.0
- Se incluyen metódos de extensión para la inyección de dependencias para publicadores, repositorios, servicios y suscriptores.
- Se incluyen métodos de extensión para iniciar / detener los suscriptores de mensajería.
- Se incluye la clase base para los servicios de la capa de aplicación.
- Se incluye la implementación de los repositorios para el acceso a datos relacionados con dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
- Se incluye la implementación de los publicadores para la mensajería genérica.
- Se incluye la implementación de los servicios de aplicación relacionados con dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
- Se incluye la implementación de suscriptores para el procesamiento de la mensajería relacionada con dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
