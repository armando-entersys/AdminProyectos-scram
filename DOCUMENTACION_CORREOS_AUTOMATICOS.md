# Correos Automáticos - Sistema AdminProyectos NATURA

**Fecha**: Noviembre 2025
**Versión**: 1.0
**Reporte**: #8

---

## Índice

1. [Resumen Ejecutivo](#1-resumen-ejecutivo)
2. [Configuración General](#2-configuración-general)
3. [Instancias que Disparan Correos](#3-instancias-que-disparan-correos)
4. [Detalles por Categoría](#4-detalles-por-categoría)
5. [Destinatarios por Rol](#5-destinatarios-por-rol)
6. [Plantillas de Correo](#6-plantillas-de-correo)
7. [Servicio de Correos Programados](#7-servicio-de-correos-programados)

---

## 1. Resumen Ejecutivo

El sistema AdminProyectos NATURA envía correos electrónicos automáticos en **13 instancias diferentes** relacionadas con:
- Gestión de usuarios
- Ciclo de vida de Briefs/Proyectos
- Gestión de Materiales
- Participantes en proyectos
- Recordatorios automáticos (nuevo)

### Servidor de Correo

**Configuración SMTP:**
- Servidor: smtp.gmail.com
- Puerto: 587
- SSL: Habilitado
- Remitente: "NATURA ADCON" <no-reply@natura.com>

---

## 2. Configuración General

### Ubicación de Configuración

**Archivo**: `PresentationLayer/appsettings.json`

```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderName": "NATURA ADCON",
  "SenderEmail": "no-reply@natura.com",
  "Username": "ajcortest@gmail.com",
  "Password": "wfqbvzuiwzrwlpeg",
  "UseSsl": true
}
```

### Categorías de Correo

Todas las categorías están definidas en `appsettings.json` bajo la sección `CategoriasDeCorreo`.

---

## 3. Instancias que Disparan Correos

### Tabla Resumen

| # | Categoría | Evento Disparador | Destinatarios | Automático |
|---|-----------|-------------------|---------------|------------|
| 1 | MensajeBienvenida | Nuevo usuario registrado y aceptado | Usuario nuevo (roles 2 y 3) | Sí |
| 2 | NuevoProyecto | Se crea un nuevo Brief/Proyecto | Administradores (rol 1) | Sí |
| 3 | ActualizaEstatusProyecto | Se modifica el estatus de un Brief | Administradores (rol 1) | Sí |
| 4 | UsuarioNuevo | Usuario solicita registro en plataforma | Administradores (rol 1) | Sí |
| 5 | UsuarioAceptado | Admin acepta solicitud de usuario | Usuario aceptado | Sí |
| 6 | UsuarioRechazo | Admin rechaza solicitud de usuario | Usuario rechazado | Sí |
| 7 | EdicionBreaf | Usuario edita información de Brief | Administradores (rol 1) | Sí |
| 8 | CambioEstatusBreaf | Admin cambia estatus de Brief | Creador del Brief (rol 2) | Sí |
| 9 | CambioPassword | Usuario solicita recuperar contraseña | Usuario solicitante | Sí |
| 10 | CambioPasswordUsuario | Usuario completa cambio de contraseña | Usuario | Sí |
| 11 | RegistroUsuario | Nuevo registro de usuario en sistema | Usuario nuevo | Sí |
| 12 | RegistroUsuarioAdmin | Admin crea usuario manualmente | Administradores | Sí |
| 13 | ComentarioMaterial | Se agrega comentario a un Material | Participantes del material | Sí |
| 14 | RegistroParticipante | Se agrega participante a un Brief | Participante agregado | Sí |
| 15 | NuevoMaterial | Se crea un nuevo Material | Participantes del Brief | Sí |
| 16 | EliminarProyecto | Admin elimina un proyecto/Brief | Administradores | Sí |
| 17 | **ReminderEntregaMaterial** | **1 día antes de fecha de entrega** | **Creador Brief + Producción** | **Sí - 9:00 AM** |

---

## 4. Detalles por Categoría

### 4.1 Gestión de Usuarios

#### MensajeBienvenida
**Trigger**: Usuario nuevo es aceptado por administrador
- **Asunto**: "Bienvenido {nombre} a Administrador de Proyectos."
- **Destinatarios**: Usuario nuevo (roles Usuario y Producción)
- **Contenido**: Mensaje de bienvenida + enlace para cambiar contraseña
- **Variables**: `{nombre}`, `{url}`

#### UsuarioNuevo
**Trigger**: Usuario se registra y solicita acceso
- **Asunto**: "Confirmar cuenta - NATURA ADCON"
- **Destinatarios**: Administradores
- **Contenido**: Notificación de nueva solicitud de registro
- **Acción Admin**: Debe aprobar o rechazar desde plataforma

#### UsuarioAceptado
**Trigger**: Admin acepta solicitud de usuario
- **Asunto**: "Autorización de acceso - NATURA ADCON"
- **Destinatarios**: Usuario aceptado
- **Contenido**: Confirmación de acceso aprobado

#### UsuarioRechazo
**Trigger**: Admin rechaza solicitud de usuario
- **Asunto**: "Rechazo acceso - NATURA ADCON"
- **Destinatarios**: Usuario rechazado
- **Contenido**: Notificación de rechazo

---

### 4.2 Gestión de Contraseñas

#### CambioPassword
**Trigger**: Usuario solicita recuperar contraseña desde login
- **Asunto**: "Cambio Contraseña - NATURA ADCON"
- **Destinatarios**: Usuario solicitante
- **Plantilla**: `EmailTemplates/CambioPassword.html`
- **Contenido mejorado** (Reporte #9):
  - Saludo personalizado
  - Explicación clara del proceso
  - Botón "Restablecer Contraseña"
  - Nota de seguridad
  - Instrucciones si no solicitó el cambio
- **Variables**: `{nombre}`, `{link}`
- **Controlador**: `LoginController.CambioPasswordEmail()` (línea 122)

#### CambioPasswordUsuario
**Trigger**: Usuario completa exitosamente el cambio de contraseña
- **Asunto**: "Nueva Contraseña - NATURA ADCON"
- **Destinatarios**: Usuario
- **Contenido**: Confirmación de contraseña cambiada exitosamente
- **Variables**: `{nombre}`, `{link}`
- **Controlador**: `LoginController.CambiarPasswordUsuario()` (línea 153)

---

### 4.3 Gestión de Briefs/Proyectos

#### NuevoProyecto
**Trigger**: Usuario crea un nuevo Brief
- **Asunto**: "Se creo un nuevo Proyecto - NATURA ADCON"
- **Destinatarios**: Todos los administradores
- **Contenido**: Notificación de nuevo Brief creado
- **Variables**: `{nombre}`, `{nombreBreaf}`

#### EdicionBreaf
**Trigger**: Usuario edita información de un Brief existente
- **Asunto**: "Se edito un Breaf - NATURA ADCON"
- **Destinatarios**: Administradores
- **Contenido**: Notificación de Brief modificado
- **Variables**: `{nombre}`, `{nombreBreaf}`

#### CambioEstatusBreaf
**Trigger**: Administrador cambia el estatus de un Brief
- **Asunto**: "Autorización Breaf - NATURA ADCON"
- **Destinatarios**: Usuario creador del Brief (rol Usuario)
- **Contenido**: Notificación de cambio de estatus

#### ActualizaEstatusProyecto
**Trigger**: Se actualiza el estatus de un proyecto
- **Asunto**: "Se actualizo estatus Proyecto - NATURA ADCON"
- **Destinatarios**: Administradores
- **Variables**: `{nombre}`, `{nombreBreaf}`

#### EliminarProyecto
**Trigger**: Administrador elimina un Brief/Proyecto
- **Asunto**: "Eliminación de Proyecto - NATURA ADCON"
- **Destinatarios**: Administradores
- **Contenido**: Notificación de proyecto eliminado

---

### 4.4 Gestión de Materiales

#### NuevoMaterial
**Trigger**: Se crea un nuevo Material asociado a un Brief
- **Asunto**: "Registro Material - NATURA ADCON"
- **Destinatarios**: Participantes del Brief
- **Contenido**: Notificación de nuevo material creado

#### ComentarioMaterial
**Trigger**: Usuario agrega comentario a un Material
- **Asunto**: "Registro Comentario Material - NATURA ADCON"
- **Destinatarios**: Participantes del Material
- **Contenido**: Notificación de nuevo comentario

#### ReminderEntregaMaterial ⚠️ NUEVO
**Trigger**: Servicio programado ejecutado diariamente a las 9:00 AM
- **Asunto**: "Recordatorio: Entrega de Material Mañana - NATURA ADCON"
- **Destinatarios**:
  - Usuario creador del Brief
  - Todos los usuarios con rol Producción (rol 3)
- **Plantilla**: `EmailTemplates/ReminderEntregaMaterial.html`
- **Contenido**:
  - Recordatorio de entrega al día siguiente
  - Nombre del proyecto
  - Nombre del material
  - Fecha de entrega
  - Responsable
  - Enlace directo al material
- **Variables**: `{nombreProyecto}`, `{nombreMaterial}`, `{fechaEntrega}`, `{responsable}`, `{link}`
- **Servicio**: `PresentationLayer/Services/MaterialReminderService.cs`
- **Frecuencia**: Diariamente a las 9:00 AM
- **Lógica**:
  ```csharp
  // Buscar materiales donde:
  // FechaEntrega = DateTime.Today.AddDays(1)
  // (es decir, materiales que se entregan mañana)
  ```

---

### 4.5 Gestión de Participantes

#### RegistroParticipante
**Trigger**: Se agrega un participante a un Brief/Proyecto
- **Asunto**: "Registro Participante - NATURA ADCON"
- **Destinatarios**: Usuario agregado como participante
- **Contenido**: Notificación de que fue agregado al proyecto

---

## 5. Destinatarios por Rol

### Roles del Sistema

| ID | Nombre | Descripción |
|----|--------|-------------|
| 1 | Administrador | Control total del sistema |
| 2 | Usuario | Creador de Briefs y Materiales |
| 3 | Producción | Gestión de materiales en producción |

### Matriz de Correos por Rol

| Categoría | Admin (1) | Usuario (2) | Producción (3) |
|-----------|-----------|-------------|----------------|
| MensajeBienvenida | - | ✓ | ✓ |
| NuevoProyecto | ✓ | - | - |
| ActualizaEstatusProyecto | ✓ | - | - |
| UsuarioNuevo | ✓ | - | - |
| UsuarioAceptado | ✓ | - | - |
| UsuarioRechazo | ✓ | - | - |
| EdicionBreaf | ✓ | - | - |
| CambioEstatusBreaf | - | ✓ | - |
| CambioPassword | - | ✓ | - |
| CambioPasswordUsuario | - | ✓ | - |
| RegistroUsuario | - | ✓ | - |
| RegistroUsuarioAdmin | - | ✓ | - |
| ComentarioMaterial | - | ✓ | - |
| RegistroParticipante | - | ✓ | - |
| NuevoMaterial | - | ✓ | - |
| EliminarProyecto | - | ✓ | - |
| **ReminderEntregaMaterial** | - | **✓** | **✓** |

---

## 6. Plantillas de Correo

### Ubicación

**Directorio**: `PresentationLayer/EmailTemplates/`

### Plantillas Disponibles

```
EmailTemplates/
├── CambioPassword.html              (Mejorada - Reporte #9)
├── CambioPasswordUsuario.html
├── ReminderEntregaMaterial.html     (Nueva - Reporte #10)
├── [Otras plantillas según categoría]
```

### Variables Dinámicas

Las plantillas utilizan placeholders que son reemplazados dinámicamente:

- `{nombre}` - Nombre del usuario
- `{link}` - URL de acción (cambio contraseña, ver material, etc.)
- `{url}` - URL general de la plataforma
- `{nombreBreaf}` - Nombre del Brief/Proyecto
- `{nombreProyecto}` - Nombre del proyecto (ReminderEntregaMaterial)
- `{nombreMaterial}` - Nombre del material (ReminderEntregaMaterial)
- `{fechaEntrega}` - Fecha de entrega formateada (ReminderEntregaMaterial)
- `{responsable}` - Usuario responsable del material (ReminderEntregaMaterial)

### Estructura de Plantilla

Todas las plantillas siguen un diseño consistente:
- Logo de NATURA en la parte superior
- Título principal
- Cuerpo del mensaje
- Botón de acción (cuando aplica)
- Footer con información adicional

---

## 7. Servicio de Correos Programados

### MaterialReminderService (NUEVO)

**Archivo**: `PresentationLayer/Services/MaterialReminderService.cs`

**Tipo**: IHostedService (Background Service)

**Función**: Enviar recordatorios automáticos de entrega de materiales

**Configuración**:
```csharp
// Registrado en Program.cs línea 119
builder.Services.AddHostedService<MaterialReminderService>();
```

**Ejecución**:
- Primer ejecución: Al día siguiente a las 9:00 AM
- Frecuencia: Cada 24 horas (diariamente)
- Horario: 9:00 AM hora del servidor

**Proceso**:
1. Se ejecuta a las 9:00 AM
2. Consulta base de datos por materiales con `FechaEntrega = DateTime.Today.AddDays(1)`
3. Por cada material encontrado:
   - Obtiene información del Brief
   - Identifica al creador del Brief
   - Obtiene todos los usuarios con rol Producción (RolId = 3)
   - Envía correo a ambos grupos
4. Registra en log la cantidad de recordatorios enviados

**Logs**:
```csharp
_logger.LogInformation("MaterialReminderService iniciado. Primera ejecución programada para {time}", scheduledTime);
_logger.LogInformation("Procesando recordatorios de entrega. Materiales encontrados: {count}", materiales.Count);
```

---

## 8. Servicio de Envío de Correos

### Implementación

**Interfaz**: `IEmailSender`
**Implementación**: Clase que utiliza MailKit
**Configuración**: `EmailSettings` en appsettings.json

### Método Principal

```csharp
void SendEmail(
    List<string> destinatarios,
    string categoria,
    Dictionary<string, string> valoresDinamicos
)
```

**Parámetros**:
- `destinatarios`: Lista de correos electrónicos
- `categoria`: Nombre de la categoría de correo (debe existir en appsettings.json)
- `valoresDinamicos`: Diccionario con variables a reemplazar en la plantilla

---

## 9. Consideraciones Técnicas

### Manejo de Errores

- Los errores de envío se registran en logs
- No se detiene la ejecución principal si falla un envío
- Se implementa try-catch en todos los controladores que envían correos

### Rendimiento

- Correos programados se ejecutan en background
- No bloquean requests HTTP
- Uso de IHostedService para tareas programadas

### Seguridad

- Contraseña SMTP almacenada en appsettings (considerar mover a Azure Key Vault en producción)
- SSL habilitado para todas las conexiones SMTP
- Correos enviados desde cuenta no-reply

---

## 10. Pruebas y Monitoreo

### Logs de Correos

Revisar logs en:
```
docker logs adminproyectos-web | grep -i "email\|correo\|reminder"
```

### Verificación de Envío

1. Revisar logs del servidor SMTP
2. Verificar bandeja de entrada de destinatarios
3. Revisar carpeta de spam
4. Validar que las variables dinámicas se reemplazan correctamente

### Métricas Recomendadas

- Cantidad de correos enviados por día
- Categoría de correo más utilizada
- Tasa de errores de envío
- Tiempo promedio de procesamiento

---

## 11. Mantenimiento

### Agregar Nueva Categoría de Correo

1. Agregar configuración en `appsettings.json`:
```json
"NuevaCategoria": {
  "DestinatariosRol": ["1", "2"],
  "Asunto": "Asunto del correo",
  "Cuerpo": ""
}
```

2. Crear plantilla HTML en `EmailTemplates/NuevaCategoria.html`

3. Llamar método `SendEmail()` desde el controlador correspondiente:
```csharp
_emailSender.SendEmail(destinatarios, "NuevaCategoria", valoresDinamicos);
```

---

## 12. Contacto y Soporte

Para modificaciones o consultas sobre el sistema de correos automáticos:

- **Equipo de Desarrollo**: AdminProyectos NATURA
- **Documentación Técnica**: Ver código fuente en repositorio
- **Logs del Sistema**: Docker logs en servidor de producción

---

**Documento elaborado por**: Sistema AdminProyectos - Equipo de Desarrollo
**Última actualización**: Noviembre 2025
**Próxima revisión**: Cada vez que se agregue una nueva categoría de correo
