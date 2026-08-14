# Zetatech.DeepSight.Logging
## Introducción
Librería perteneciente a la plataforma **Zetatech DeepSight**, desarrollada por **Zeta Technologies** y que contiene los componentes que registran la actividad de diagnóstico de las aplicaciones.
## Estructura
```
├─ Zetatech
   ├─ DeepSight
      ├─ DependencyInjection      ' Métodos de extensión para el registro de componentes en el contenedor de dependencias.
      ├─ Logging                  ' Clases especializadas para el registro de actividad de diagnóstico.
         ├─ Dtos                  ' Clases utilizadas como entrada, en las llamadas a la api de ingesta.
         ├─ Providers             ' Proveedores para crear instancias especializadas para el registro de actividad de diagnóstico.
```
## Configuración
``` json
{
   "logging": {
      "deepSight": {
         "appName": "",
         "appVersion": "", // Version in format major.minor.revision
         "logLevel": "debug | information | warning | error | critical",
         "url": ""
      },
      "logLevel": {
         "deepSight": "information"
      }
   }
}
```
## Control de versiones
### v10.2609.0
- Se incluye la clase para la configuración del logger especializado en el sistema DeepSight.
- Se incluye la clase especializada que realiza el registro de la actividad de diagnóstico de las aplicaciones en el sistema DeepSight.
- Se incluye el proveedor para crear instancias especializadas para el registro de actividad de diagnóstico en el sistema DeepSight.
- Se incluyen los DTOs, que son utilizados como entrada en las llamadas a la api de ingesta.
