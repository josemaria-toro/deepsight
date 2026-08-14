# Zetatech.DeepSight.Telemetry
## Introducción
Librería perteneciente a la plataforma **Zetatech DeepSight**, desarrollada por **Zeta Technologies** y que contiene los componentes que registran información sobre la telemetría de las aplicaciones.
## Estructura
```
├─ Zetatech
   ├─ DeepSight
      ├─ DependencyInjection      ' Métodos de extensión para el registro de componentes en el contenedor de dependencias.
      ├─ Telemetry                ' Clases especializadas para el registro de actividad de diagnóstico.
```
## Configuración
### DeepSight
``` json
{
   "logging": {
      "deepSight": {
         "appName": "",
         "appVersion": "", // Version in format major.minor.revision
         "url": ""
      }
   }
}
```
## Control de versiones
### v10.2609.0
- Implementación especializada para el envío de información al sistema DeepSight.
