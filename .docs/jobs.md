# Zetatech.DeepSight.Jobs
## Introducción
Aplicación perteneciente a la plataforma **Zetatech DeepSight**, desarrollada por **Zeta Technologies** y que contiene los procesos en segundo plano para el mantenimiento de la plataforma.
## Estructura
```
├─ Zetatech
   ├─ DeepSight                   ' Punto de entrada de la aplicación.
      ├─ DependencyInjection      ' Métodos de extensión para el registro de componentes en el contenedor de dependencias.
      ├─ Extensions               ' Métodos de extensión para iniciar / detener los procesos en segundo plano.
      ├─ Jobs                     ' Clase que contienen la lógica de ejecución de los procesos en segundo plano.
```
## Control de versiones
### v10.2609.0
- Se incluyen los procesos en segundo plano para el mantenimiento de los datos relativos a dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
- Se incluyen métodos de extensión para iniciar / detener los procesos en segundo plano.
