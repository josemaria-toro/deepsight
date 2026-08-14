# Zetatech.DeepSight.Logging
## Introducción
Librería perteneciente a la plataforma **Zetatech DeepSight**, desarrollada por **Zeta Technologies** y que contiene los componentes que registran la actividad de diagnóstico de las aplicaciones.
## Estructura
```
├─ Zetatech
   ├─ DeepSight
      ├─ DependencyInjection      ' Métodos de extensión para el registro de los proveedores de logging.
```
## Configuración
``` json
{
   "logging": {
      "deepSight": {
         "appName": "",
         "appVersion": "x.x.x",
         "logLevel": "debug | information | warning | error | critical",
         "tenant": "",
         "url": ""
      }
   }
}
```
## Control de versiones
### v10.2609.0
- Implementación especializada para el envío de información al sistema DeepSight.