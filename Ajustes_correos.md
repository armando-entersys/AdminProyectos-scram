
### 1. ActualizaEstatusProyecto
**Archivo:** `ActualizaEstatusProyecto.html`
**Propósito:** Notificar al usuario cuando el estatus de su proyecto cambia.
**Variables:**
- `{nombreProyecto}` - Nombre del proyecto
- `{estatus}` - Nuevo estatus del proyecto
- `{link}` - Enlace para ver el proyecto
 
**Contenido del mensaje:**
```
Tu proyecto {nombreProyecto}.
se encuentra en estatus {estatus}.
El administrador revisará la información de tu proyecto y determinará su siguiente estatus.
En caso de existir algún faltante o duda te notificaremos.
[Botón: Ver proyecto]



### 2. CambioPassword
**Archivo:** `CambioPassword.html`
**Propósito:** Enviar enlace para restablecer contraseña.
**Variables:**
- `{nombre}` - Nombre del usuario
- `{link}` - Enlace para restablecer contraseña

**Contenido del mensaje:**
```Solicitud de Cambio de Contraseña
 
 
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
El Cambio de Contraseña se realizó      correctamente.
Haz clic en el siguiente enlace para iniciar sesión:
[Botón: Acceder]
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
Se agregó un comentario en el material.
El usuario {usuario} actualizó a {estatus} el estatus del material {nombreMaterial}.
Comentario: {comentario}
[Botón: Ver proyecto]
```
**Observaciones:**
- Error ortográfico: "Se Arego" debería ser "Se Agregó"
- Error ortográfico: "actualizo" debería ser "actualizó"


### 5. EdiciónBrief
**Archivo:** `EdicionBreaf.html`
**Propósito:** Notificar cuando un proyecto es editado.
**Variables:**
- `{nombre}` - Nombre del usuario que editó
- `{nombreProyecto}` - Nombre del proyecto
- `{link}` - Enlace para ver el proyecto
 
**Contenido del mensaje:**
```
Se editó un proyecto.
El usuario {nombre} editó el proyecto {nombreProyecto}.
[Botón: Ver proyecto]
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
El Usuario {usuario} Eliminó el proyecto {nombreProyecto}.
```

**Observaciones:**
- Error ortográfico: "Usuario" debería ser "Usuario"
- Error ortográfico: "Eliminó" debería ser "Eliminó"
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
Se generó un material nuevo.
El Administrador creó un nuevo Material ({nombreMaterial}) en el proyecto {nombreProyecto}.
[Botón: Ver material]
Si no solicitaste esta confirmación, por favor ignora este correo.
```

**Observaciones:**
- Error ortográfico: "Se generó" debería ser "Se generó"
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
Se generó un proyecto nuevo.
El usuario {nombre} creó un nuevo proyecto {nombreProyecto}.
[Botón: Ver proyectos]
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
Has sido agregado como usuario participante del proyecto {nombreProyecto}.
Accede a tu plataforma y revisar tus Alertas para mayor detalle.
[Botón: Ver proyecto]
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
Registro exitoso en ADCON (Administrador de comunicación Natura).
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
Registro exitoso en ADCON (Administrador de comunicación Natura).
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
Recuerda que la entrega de tu material {nombreMaterial} es el día de mañana!

Proyecto: {nombreProyecto}
Material: {nombreMaterial}
Fecha de Entrega: {fechaEntrega}
Responsable: {responsable}
[Botón: Ver material]
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
Se ha aprobado tu solicitud.
Ingresa en el siguiente enlace con tu usuario y contraseña para acceder al sistema.
[Botón: Ingresar al sistema]
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
Se ha rechazado tu solicitud.
Comunícate con el administrador para mayor detalle.
[Botón: Aceptar]
Si no solicitaste esta confirmación, por favor ignora este correo.
```

**Observaciones:**
- Error ortográfico: "Comunicate" debería ser "Comunícate"
- Error ortográfico: "mas" debería ser "más"
- El texto del botón "Aceptar" no tiene sentido en este contexto
- El enlace no es útil si el usuario fue rechazado

---
