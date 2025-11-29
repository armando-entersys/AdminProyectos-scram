# Plantillas de Correo - ADCON Natura

Este documento detalla todas las plantillas de correo electrónico utilizadas en el sistema.

---

## Resumen de Plantillas

| # | Nombre | Variables | Descripción |
|---|--------|-----------|-------------|
| 1 | ActualizaEstatusProyecto | `{nombreProyecto}`, `{estatus}`, `{link}` | Notifica cambio de estatus de proyecto |
| 2 | CambioPassword | `{nombre}`, `{link}` | Solicitud de restablecimiento de contraseña |
| 3 | CambioPasswordUsuario | `{link}` | Confirmación de cambio de contraseña exitoso |
| 4 | ComentarioMaterial | `{usuario}`, `{estatus}`, `{nombreMaterial}`, `{comentario}`, `{link}` | Notifica nuevo comentario en material |
| 5 | EdicionBreaf | `{nombre}`, `{nombreProyecto}`, `{link}` | Notifica edición de proyecto |
| 6 | EliminarProyecto | `{usuario}`, `{nombreProyecto}` | Notifica eliminación de proyecto |
| 7 | NuevoMaterial | `{nombreMaterial}`, `{nombreProyecto}`, `{link}` | Notifica creación de nuevo material |
| 8 | NuevoProyecto | `{nombre}`, `{nombreProyecto}`, `{link}` | Notifica creación de nuevo proyecto |
| 9 | RegistroParticipante | `{nombreProyecto}`, `{link}` | Notifica asignación como participante |
| 10 | RegistroUsuario | - | Confirmación de registro exitoso |
| 11 | RegistroUsuarioAdmin | `{usuario}`, `{password}`, `{link}` | Credenciales de acceso para nuevo usuario |
| 12 | ReminderEntregaMaterial | `{nombreMaterial}`, `{nombreProyecto}`, `{fechaEntrega}`, `{responsable}`, `{link}` | Recordatorio de entrega de material |
| 13 | UsuarioAceptado | `{link}` | Notifica aprobación de solicitud |
| 14 | UsuarioRechazo | `{link}` | Notifica rechazo de solicitud |

---

## Detalle de cada Plantilla

---

### 1. ActualizaEstatusProyecto

**Archivo:** `ActualizaEstatusProyecto.html`

**Propósito:** Notificar al usuario cuando el estatus de su proyecto cambia.

**Variables:**
- `{nombreProyecto}` - Nombre del proyecto
- `{estatus}` - Nuevo estatus del proyecto
- `{link}` - Enlace para ver el proyecto

**Contenido del mensaje:**
```
Tu proyecto {nombreProyecto}
se encuentra en estatus {estatus}.
El administrador revisará la información de tu proyecto y determinará su siguiente estatus.
En caso de existir algún faltante o duda te notificaremos.
[Botón: ver proyecto]
```

**Observaciones:**
- Hay una tilde incorrecta: "algÚn" debería ser "algún"

---

### 2. CambioPassword

**Archivo:** `CambioPassword.html`

**Propósito:** Enviar enlace para restablecer contraseña.

**Variables:**
- `{nombre}` - Nombre del usuario
- `{link}` - Enlace para restablecer contraseña

**Contenido del mensaje:**
```
Solicitud de Cambio de Contraseña

Hola {nombre},
Hemos recibido una solicitud para restablecer la contraseña de tu cuenta en el Sistema de Administración de Proyectos NATURA ADCON.
Si realizaste esta solicitud, haz clic en el siguiente botón para crear una nueva contraseña:
[Botón: Restablecer Contraseña]

Este enlace es válido únicamente para una sola operación de cambio de contraseña.
Por seguridad, te recomendamos cambiar tu contraseña lo antes posible.

¿No solicitaste este cambio?
Si no realizaste esta solicitud, puedes ignorar este correo de forma segura.
Tu contraseña actual permanecerá sin cambios.
Si tienes alguna duda o necesitas asistencia, contacta al administrador del sistema.
```

**Observaciones:**
- Plantilla bien estructurada con mensaje de seguridad completo

---

### 3. CambioPasswordUsuario

**Archivo:** `CambioPasswordUsuario.html`

**Propósito:** Confirmar que el cambio de contraseña se realizó correctamente.

**Variables:**
- `{link}` - Enlace para iniciar sesión

**Contenido del mensaje:**
```
El Cambio de Contraseña se realizó correactamente
Haz clic en el siguiente enlace para iniciar sesión:
[Botón: Accesar]
Si no solicitaste algún cambio, por favor ignora este mensaje.
```

**Observaciones:**
- Error ortográfico: "correactamente" debería ser "correctamente"
- Error ortográfico: "Accesar" debería ser "Acceder"

---

### 4. ComentarioMaterial

**Archivo:** `ComentarioMaterial.html`

**Propósito:** Notificar cuando se agrega un comentario a un material.

**Variables:**
- `{usuario}` - Nombre del usuario que comentó
- `{estatus}` - Nuevo estatus del material
- `{nombreMaterial}` - Nombre del material
- `{comentario}` - Contenido del comentario
- `{link}` - Enlace para ver el proyecto

**Contenido del mensaje:**
```
Se Arego un comentario en el material
El usuario {usuario} actualizo a {estatus} el estatus del material {nombreMaterial}
Comentario: {comentario}
[Botón: ver proyecto]
```

**Observaciones:**
- Error ortográfico: "Se Arego" debería ser "Se Agregó"
- Error ortográfico: "actualizo" debería ser "actualizó"

---

### 5. EdicionBreaf

**Archivo:** `EdicionBreaf.html`

**Propósito:** Notificar cuando un proyecto es editado.

**Variables:**
- `{nombre}` - Nombre del usuario que editó
- `{nombreProyecto}` - Nombre del proyecto
- `{link}` - Enlace para ver el proyecto

**Contenido del mensaje:**
```
Se edito un proyecto
El usuario {nombre} edito el proyecto {nombreProyecto}
[Botón: ver proyecto]
```

**Observaciones:**
- Error ortográfico: "Se edito" debería ser "Se editó"
- Error ortográfico: "edito" debería ser "editó"
- El nombre del archivo "EdicionBreaf" tiene error, debería ser "EdicionBrief"

---

### 6. EliminarProyecto

**Archivo:** `EliminarProyecto.html`

**Propósito:** Notificar cuando un proyecto es eliminado.

**Variables:**
- `{usuario}` - Nombre del usuario que eliminó
- `{nombreProyecto}` - Nombre del proyecto eliminado

**Contenido del mensaje:**
```
El Usuaio {usuario} Elimino el proyecto {nombreProyecto}
```

**Observaciones:**
- Error ortográfico: "Usuaio" debería ser "Usuario"
- Error ortográfico: "Elimino" debería ser "Eliminó"
- No tiene botón de acción
- No tiene título (h1)

---

### 7. NuevoMaterial

**Archivo:** `NuevoMaterial.html`

**Propósito:** Notificar la creación de un nuevo material en un proyecto.

**Variables:**
- `{nombreMaterial}` - Nombre del material creado
- `{nombreProyecto}` - Nombre del proyecto
- `{link}` - Enlace para ver el material

**Contenido del mensaje:**
```
Se genero un material nuevo
El Administrador creo un nuevo Material ({nombreMaterial}) en el proyecto {nombreProyecto}
[Botón: ver Material]
Si no solicitaste esta confirmación, por favor ignora este correo.
```

**Observaciones:**
- Error ortográfico: "Se genero" debería ser "Se generó"
- Error ortográfico: "creo" debería ser "creó"

---

### 8. NuevoProyecto

**Archivo:** `NuevoProyecto.html`

**Propósito:** Notificar la creación de un nuevo proyecto.

**Variables:**
- `{nombre}` - Nombre del usuario que creó el proyecto
- `{nombreProyecto}` - Nombre del proyecto
- `{link}` - Enlace para ver proyectos

**Contenido del mensaje:**
```
Se genero un proyecto nuevo
El usuario {nombre} creo un nuevo proyecto {nombreProyecto}
[Botón: ver proyectos]
Si no solicitaste esta confirmación, por favor ignora este correo.
```

**Observaciones:**
- Error ortográfico: "Se genero" debería ser "Se generó"
- Error ortográfico: "creo" debería ser "creó"

---

### 9. RegistroParticipante

**Archivo:** `RegistroParticipante.html`

**Propósito:** Notificar cuando un usuario es agregado como participante a un proyecto.

**Variables:**
- `{nombreProyecto}` - Nombre del proyecto
- `{link}` - Enlace para ver el proyecto

**Contenido del mensaje:**
```
Registro Participante en ADCON (Administrador de comunicación Natura)
Has sido agregado como usuario participante del proyecto {nombreProyecto}.
[Botón: ver proyecto]
Si no solicitaste esta confirmación, por favor ignora este mensaje.
```

**Observaciones:**
- El mensaje del footer es confuso, el usuario no "solicitó" ser agregado

---

### 10. RegistroUsuario

**Archivo:** `RegistroUsuario.html`

**Propósito:** Confirmar registro exitoso de un nuevo usuario.

**Variables:**
- Ninguna

**Contenido del mensaje:**
```
Registro exitoso en ADCON (Administrador de comunicación Natura)
El administrador revisará tu solicitud y te notificaremos por correo los siguientes pasos.
Si no solicitaste esta confirmación, por favor ignora este mensaje.
```

**Observaciones:**
- No tiene botón de acción
- Plantilla simple y correcta

---

### 11. RegistroUsuarioAdmin

**Archivo:** `RegistroUsuarioAdmin.html`

**Propósito:** Enviar credenciales de acceso a un nuevo usuario creado por administrador.

**Variables:**
- `{usuario}` - Nombre de usuario/correo
- `{password}` - Contraseña asignada
- `{link}` - Enlace para ingresar al sistema

**Contenido del mensaje:**
```
Registro exitoso en ADCON (Administrador de comunicación Natura)
Tus credenciales de acceso son:
Usuario: {usuario}
Contraseña: {password}
[Botón: Ingresa al sistema]
Si no solicitaste esta confirmación, por favor ignora este correo.
```

**Observaciones:**
- **SEGURIDAD:** Se envía contraseña en texto plano por correo
- Considerar usar enlace de activación en lugar de enviar contraseña

---

### 12. ReminderEntregaMaterial

**Archivo:** `ReminderEntregaMaterial.html`

**Propósito:** Recordar al usuario sobre la entrega próxima de un material.

**Variables:**
- `{nombreMaterial}` - Nombre del material
- `{nombreProyecto}` - Nombre del proyecto
- `{fechaEntrega}` - Fecha de entrega
- `{responsable}` - Nombre del responsable
- `{link}` - Enlace para ver el material

**Contenido del mensaje:**
```
¡Recordatorio de Entrega!
Recuerda que tu Material {nombreMaterial} es el día de mañana!

Proyecto: {nombreProyecto}
Material: {nombreMaterial}
Fecha de Entrega: {fechaEntrega}
Responsable: {responsable}

[Botón: Ver Material]
Este es un recordatorio automático. Por favor, asegúrate de tener todo listo para la entrega.
```

**Observaciones:**
- Plantilla bien estructurada con información clara
- Mejor diseño visual con sección de información destacada

---

### 13. UsuarioAceptado

**Archivo:** `UsuarioAceptado.html`

**Propósito:** Notificar al usuario que su solicitud de registro fue aprobada.

**Variables:**
- `{link}` - Enlace para ingresar al sistema

**Contenido del mensaje:**
```
Se ha aprobado tu solicitud
Ingresa en el siguiente enlace con tu usuario y contraseña para ingresar al sistema
[Botón: Aceptar]
Si no solicitaste esta confirmación, por favor ignora este correo.
```

**Observaciones:**
- El texto del botón "Aceptar" no es muy descriptivo, podría ser "Ingresar al sistema"

---

### 14. UsuarioRechazo

**Archivo:** `UsuarioRechazo.html`

**Propósito:** Notificar al usuario que su solicitud de registro fue rechazada.

**Variables:**
- `{link}` - Enlace (no queda claro para qué)

**Contenido del mensaje:**
```
Se ha rechazado tu solicitud
Comunicate con el administrador para mas detalles
[Botón: Aceptar]
Si no solicitaste esta confirmación, por favor ignora este correo.
```

**Observaciones:**
- Error ortográfico: "Comunicate" debería ser "Comunícate"
- Error ortográfico: "mas" debería ser "más"
- El texto del botón "Aceptar" no tiene sentido en este contexto
- El enlace no es útil si el usuario fue rechazado

---

## Resumen de Errores a Corregir

### Errores Ortográficos

| Plantilla | Error | Corrección |
|-----------|-------|------------|
| ActualizaEstatusProyecto | algÚn | algún |
| CambioPasswordUsuario | correactamente | correctamente |
| CambioPasswordUsuario | Accesar | Acceder |
| ComentarioMaterial | Se Arego | Se Agregó |
| ComentarioMaterial | actualizo | actualizó |
| EdicionBreaf | Se edito | Se editó |
| EdicionBreaf | edito | editó |
| EliminarProyecto | Usuaio | Usuario |
| EliminarProyecto | Elimino | Eliminó |
| NuevoMaterial | Se genero | Se generó |
| NuevoMaterial | creo | creó |
| NuevoProyecto | Se genero | Se generó |
| NuevoProyecto | creo | creó |
| UsuarioRechazo | Comunicate | Comunícate |
| UsuarioRechazo | mas | más |

### Problemas de UX/Contenido

1. **EliminarProyecto** - No tiene título ni botón de acción
2. **UsuarioAceptado** - Botón con texto poco descriptivo
3. **UsuarioRechazo** - Botón "Aceptar" no tiene sentido
4. **RegistroUsuarioAdmin** - Envía contraseña en texto plano (riesgo de seguridad)
5. **RegistroParticipante** - Mensaje de footer confuso

### Nombres de Archivo

- `EdicionBreaf.html` debería ser `EdicionBrief.html`

---

## Estilo Visual

Todas las plantillas comparten el mismo estilo CSS:
- Fondo gris claro (#f4f4f4)
- Contenedor blanco con sombra
- Logo de Natura centrado
- Botón con borde rojo (#AE0025)
- Fuente Arial
- Ancho máximo 600px

---

## Uso de Variables

Para usar las plantillas en el código, se envían las variables dinámicas así:

```csharp
var valoresDinamicos = new Dictionary<string, string>()
{
    { "nombreProyecto", brief.Nombre },
    { "link", urlBase + "/Brief/Index" }
};
_emailSender.SendEmail(destinatarios, "NombrePlantilla", valoresDinamicos);
```

Las variables se reemplazan en el HTML buscando `{nombreVariable}` y sustituyéndolo por el valor correspondiente.
