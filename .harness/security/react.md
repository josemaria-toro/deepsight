# Guía de seguridad: React

Guía de seguridad para aplicaciones basadas en React.

## Gestión de secretos

- Ningún secreto se escribe en el código ni en ficheros versionados. Esto incluye contraseñas, claves de API, tokens y cadenas de conexión.
- Todo lo que se incluye en el bundle del cliente es público. Las variables de entorno expuestas al frontend (por ejemplo, las prefijadas con `VITE_` o `NEXT_PUBLIC_`) no pueden contener secretos.
- Los secretos que necesita la aplicación se gestionan en el backend, nunca en el cliente.
- Los ficheros de configuración local con secretos (`.env`, etc.) se excluyen de Git mediante `.gitignore`.
- Si un secreto se expone por error, se revoca y se rota de inmediato. Eliminarlo del código no basta.

## Almacenamiento en el navegador

- Los tokens de sesión no se guardan en `localStorage` ni en `sessionStorage`. Se usan cookies con los atributos `HttpOnly`, `Secure` y `SameSite`.
- No se almacena en el navegador información personal ni sensible sin una necesidad justificada.

## Inyección y XSS

- No usar `dangerouslySetInnerHTML` salvo que el contenido se haya sanitizado previamente con una biblioteca establecida, como `DOMPurify`.
- No construir HTML, atributos ni URLs concatenando cadenas con datos del usuario.
- No usar `eval`, `new Function` ni `setTimeout` con cadenas de texto.
- Las URLs introducidas por el usuario se validan antes de usarlas en `href` o `src`. Se rechazan los esquemas `javascript:` y `data:`.
- Se define una Content Security Policy (CSP) restrictiva que impida la ejecución de scripts en línea y de orígenes no autorizados.

## Validación de entradas

- Toda entrada externa (formularios, parámetros de URL, respuestas de la API, `localStorage` y mensajes de `postMessage`) se valida antes de usarla.
- Usar esquemas tipados para validar datos, por ejemplo `zod`.
- La validación en el cliente es una mejora de experiencia de usuario. La validación de seguridad se hace siempre en el backend.

## Comunicación con el backend

- Las peticiones HTTP usan HTTPS.
- Las peticiones que modifican estado incluyen protección contra CSRF cuando la autenticación se basa en cookies.
- Los mensajes de `postMessage` comprueban el origen (`event.origin`) antes de procesarlos.
- Las redirecciones a URLs recibidas como parámetro se limitan a una lista de destinos permitidos.

## Autenticación y autorización

- La autorización se aplica en el backend. Ocultar elementos de la interfaz no es un control de seguridad.
- No implementar criptografía propia. Usar bibliotecas establecidas.

## Errores y datos expuestos

- Los mensajes de error mostrados al usuario son genéricos. Los detalles técnicos se registran internamente.
- Los mapas de código fuente (source maps) no se publican en producción, o se publican sin referencias accesibles públicamente.
- No registrar datos personales ni información sensible en la consola ni en herramientas de monitorización.

## Dependencias

- Las dependencias se revisan con `pnpm audit` en cada ejecución de la integración continua.
- Una vulnerabilidad de severidad alta o crítica en una librería utilizada por el proyecto bloquea el merge.
- Los CVE reportados se verifican en fuentes oficiales: [cve.org](https://www.cve.org) e [INCIBE](https://www.incibe.es) (incluido su catálogo de vulnerabilidades).
- El fichero de bloqueo (`pnpm-lock.yaml`) se versiona y se usa siempre en la instalación (`pnpm install --frozen-lockfile`) para evitar cambios no revisados en las dependencias.

## Análisis estático

- La integración continua ejecuta `eslint` con reglas de seguridad (por ejemplo, `eslint-plugin-security`) sobre `src/`.
- Los hallazgos de severidad alta bloquean el merge. Las excepciones se justifican con un comentario que explique el motivo.

## Privilegios y despliegue

- La aplicación se despliega con las cabeceras de seguridad necesarias: CSP, `X-Content-Type-Options`, `Referrer-Policy` y `Strict-Transport-Security`.
- El código del cliente no tiene acceso a recursos que no necesite.
