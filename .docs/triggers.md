# Zetatech.DeepSight.Triggers
## Introducción
Aplicación perteneciente a la plataforma **Zetatech DeepSight**, desarrollada por **Zeta Technologies** y que contiene los desencadenadores para el procesamiento de la mensajería.
## Estructura
```
├─ Zetatech
   ├─ DeepSight                   ' Punto de entrada de la aplicación.
      ├─ Extensions               ' Métodos de extensión para iniciar / detener los suscriptores de mensajería.
```
## Control de versiones
### v10.2609.0
- Se incluyen los controladores mvc para la ingesta de datos relativos a dependencias, errores, eventos, métricas, vistas de página, peticiones, pruebas y trazas.
- Se incluye un middleware para comprobar que el tenant utilizado en la url es el admitido por la instancia de la aplicación.
- Se incluye un middleware para el registro de las peticiones gestionadas por la aplicación.
