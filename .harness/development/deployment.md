# Guía de desarrollo: despliegues

Guía para generar las versiones distribuibles de un proyecto cuando se solicita desplegarlo. Se aplica a todos los proyectos, con independencia de su lenguaje.

## Carpeta `releases`

- Las versiones distribuibles se alojan en `releases/`, en la raíz del espacio de trabajo.
- Si la carpeta no existe, se crea al solicitar el primer despliegue.
- `releases/` no se versiona con el código fuente. Está incluida en el `.gitignore` y no se debe quitar de él.

## Proceso de despliegue

Cuando se solicita desplegar un proyecto:

1. Comprueba que existe `releases/` y créala si falta. Comprueba que está en el `.gitignore`.
2. Ejecuta las comprobaciones del proyecto (formateo, linter, tipos, pruebas y cobertura) antes de empaquetar.
3. Genera una versión distribuible del proyecto: solo el código y los recursos necesarios para ejecutarlo. No incluyas secretos, ficheros `.env`, entornos virtuales, `node_modules`, cachés, pruebas ni carpetas de control de versiones.
4. Comprime la versión distribuible en un fichero ZIP con el formato `<nombre-proyecto>.v<version>.zip` y guárdalo en `releases/`.
5. Genera junto al ZIP un fichero de texto con el mismo nombre base (`<nombre-proyecto>.v<version>.txt`) con las variables de entorno y las configuraciones necesarias para el despliegue.

## Formato de nombres

- `<nombre-proyecto>`: nombre del proyecto, en minúsculas y con guiones, igual que su carpeta (`backend`, `frontend`) o el nombre indicado por el usuario.
- `<version>`: versión [SemVer](https://semver.org/lang/es/) del proyecto, la misma que figura en su `CHANGELOG.md`.
- Ejemplo: `backend.v1.2.0.zip` y `backend.v1.2.0.txt`.

## Fichero de configuración (`.txt`)

- Cada ZIP tiene un `.txt` equivalente con el mismo nombre base.
- Lista las variables de entorno y las configuraciones que hay que realizar para desplegar el proyecto, cada una con su finalidad.
- **No contiene valores**, ni reales ni de ejemplo. Solo nombres y descripción.
- Indica también los pasos de configuración que no son variables (por ejemplo, migraciones de base de datos, servicios requeridos o ficheros de configuración).
- Formato recomendado:

```
Proyecto: <nombre-proyecto>
Versión: <version>

Variables de entorno
- NOMBRE_VARIABLE: finalidad de la variable.

Configuraciones
- Descripción de la configuración o paso a realizar.
```

## Documentación

- Los cambios de despliegue se documentan en el `README.md` del proyecto (sección **Despliegue**) y en su `CHANGELOG.md`.
- Si cambian las variables de entorno o las configuraciones, se actualiza también la sección **Configuración** del `README.md`.
