# SISTEMA DE ADMINISTRACIÓN DE PROYECTOS NATURA
## MATRICES DE PRUEBAS POR ROL

**Versión:** 1.0
**Fecha:** 2025-01-09
**Cliente:** Natura
**Empresa:** Entersys
**URL de Pruebas:** https://adminproyectos.entersys.mx

---

## TABLA DE CONTENIDOS

1. [Introducción](#introducción)
2. [Usuarios de Prueba](#usuarios-de-prueba)
3. [Matriz de Pruebas - ROL ADMINISTRADOR](#matriz-de-pruebas---rol-administrador)
4. [Matriz de Pruebas - ROL USUARIO](#matriz-de-pruebas---rol-usuario)
5. [Matriz de Pruebas - ROL PRODUCCIÓN](#matriz-de-pruebas---rol-producción)
6. [Pruebas de Integración](#pruebas-de-integración)
7. [Criterios de Aceptación](#criterios-de-aceptación)

---

## INTRODUCCIÓN

### Propósito del Documento
Este documento define las matrices de pruebas organizadas por rol de usuario para garantizar que todas las funcionalidades del sistema operen correctamente según los permisos asignados a cada rol.

### Metodología de Pruebas
- **Tipo:** Pruebas funcionales manuales
- **Enfoque:** Basado en roles y permisos
- **Cobertura:** 100% de casos de uso documentados
- **Criterio de éxito:** Todas las pruebas deben pasar sin errores

### Niveles de Prioridad
- **P1 - Crítica:** Funcionalidad core del sistema, debe funcionar sin fallas
- **P2 - Alta:** Funcionalidad importante, afecta la experiencia del usuario
- **P3 - Media:** Funcionalidad secundaria, no bloquea el uso del sistema
- **P4 - Baja:** Funcionalidad de mejora o "nice to have"

### Estados de Prueba
- ✅ **PASS:** Prueba exitosa, funcionalidad opera correctamente
- ❌ **FAIL:** Prueba fallida, requiere corrección
- ⚠️ **BLOCKED:** Prueba bloqueada por dependencia
- ⏳ **PENDING:** Prueba pendiente de ejecución
- 🔄 **RETEST:** Requiere re-prueba después de corrección

---

## USUARIOS DE PRUEBA

### Credenciales de Prueba

#### Administrador de Prueba
```
Rol: Administrador (RolId: 1)
Usuario: Admin Sistema
Email: ajcortest@gmail.com
Password: [Contactar al equipo de desarrollo]
```

#### Usuario de Prueba
```
Rol: Usuario (RolId: 2)
Usuario: Roberto Castellanos
Email: ivanldg@hotmail.com
Password: [Contactar al equipo de desarrollo]
```

#### Producción de Prueba
```
Rol: Producción (RolId: 3)
Usuario: Test ap
Email: zero.armando@gmail.com
Password: [Contactar al equipo de desarrollo]
```

---

## MATRIZ DE PRUEBAS - ROL ADMINISTRADOR

### Módulo: Autenticación

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-AUTH-001 | Iniciar sesión con credenciales válidas | P1 | 1. Navegar a /Account/Login<br/>2. Ingresar email de admin<br/>3. Ingresar contraseña correcta<br/>4. Click en "Iniciar Sesión" | Sistema autentica y redirige al dashboard. Muestra menú completo de admin. | ⏳ | |
| ADM-AUTH-002 | Iniciar sesión con contraseña incorrecta | P1 | 1. Navegar a /Account/Login<br/>2. Ingresar email válido<br/>3. Ingresar contraseña incorrecta<br/>4. Click en "Iniciar Sesión" | Sistema muestra mensaje de error "Credenciales inválidas". No autentica. | ⏳ | |
| ADM-AUTH-003 | Iniciar sesión con usuario inactivo | P2 | 1. Desactivar usuario desde otro admin<br/>2. Intentar login con usuario inactivo | Sistema deniega acceso con mensaje apropiado | ⏳ | |
| ADM-AUTH-004 | Cerrar sesión | P1 | 1. Estando autenticado<br/>2. Click en "Cerrar Sesión" | Sistema cierra sesión y redirige a login | ⏳ | |
| ADM-AUTH-005 | Cambiar contraseña propia | P2 | 1. Acceder a perfil<br/>2. Click "Cambiar Contraseña"<br/>3. Ingresar contraseña actual<br/>4. Ingresar nueva contraseña<br/>5. Confirmar nueva contraseña<br/>6. Guardar | Sistema actualiza contraseña. Se puede iniciar sesión con nueva contraseña. | ⏳ | |

---

### Módulo: Gestión de Usuarios

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-USR-001 | Ver lista de todos los usuarios | P1 | 1. Iniciar sesión como admin<br/>2. Navegar a /Usuarios | Sistema muestra lista completa de usuarios (20 usuarios actuales) | ⏳ | |
| ADM-USR-002 | Crear nuevo usuario con rol Usuario | P1 | 1. En /Usuarios<br/>2. Click "Nuevo Usuario"<br/>3. Completar:<br/>   - Nombre: "Test"<br/>   - Apellido Paterno: "Usuario"<br/>   - Correo: "test_user@test.com"<br/>   - Contraseña: "Test123$"<br/>   - Rol: Usuario (2)<br/>4. Click "Guardar" | Sistema crea usuario con Estatus=true (activo). Usuario recibe correo con credenciales. Aparece en lista de usuarios. | ⏳ | Verificar que Estatus=true por defecto |
| ADM-USR-003 | Crear nuevo usuario con rol Producción | P1 | 1. En /Usuarios<br/>2. Click "Nuevo Usuario"<br/>3. Completar datos con Rol: Producción (3)<br/>4. Guardar | Usuario se crea activo. Recibe correo. Puede iniciar sesión. | ⏳ | |
| ADM-USR-004 | Crear nuevo usuario con rol Administrador | P1 | 1. En /Usuarios<br/>2. Click "Nuevo Usuario"<br/>3. Completar datos con Rol: Administrador (1)<br/>4. Guardar | Usuario se crea con permisos de admin. Puede acceder a todas las funciones. | ⏳ | |
| ADM-USR-005 | Intentar crear usuario con correo duplicado | P1 | 1. En /Usuarios<br/>2. Click "Nuevo Usuario"<br/>3. Ingresar correo existente<br/>4. Completar otros campos<br/>5. Guardar | Sistema muestra error "El correo ya existe". No crea usuario duplicado. | ⏳ | |
| ADM-USR-006 | Editar información de usuario existente | P1 | 1. En /Usuarios<br/>2. Seleccionar un usuario<br/>3. Click "Editar"<br/>4. Modificar nombre y apellido<br/>5. Guardar | Sistema actualiza información. Cambios se reflejan en lista. | ⏳ | |
| ADM-USR-007 | Cambiar rol de un usuario | P1 | 1. En /Usuarios<br/>2. Seleccionar usuario rol Usuario<br/>3. Editar<br/>4. Cambiar a rol Producción<br/>5. Guardar | Sistema actualiza rol. Usuario tiene nuevos permisos en siguiente login. | ⏳ | |
| ADM-USR-008 | Desactivar usuario | P1 | 1. En /Usuarios<br/>2. Seleccionar usuario activo<br/>3. Cambiar Estatus a Inactivo<br/>4. Guardar | Usuario se marca como inactivo. No puede iniciar sesión. | ⏳ | |
| ADM-USR-009 | Reactivar usuario inactivo | P1 | 1. En /Usuarios<br/>2. Seleccionar usuario inactivo<br/>3. Cambiar Estatus a Activo<br/>4. Guardar | Usuario puede iniciar sesión nuevamente. | ⏳ | |
| ADM-USR-010 | Eliminar usuario | P2 | 1. En /Usuarios<br/>2. Seleccionar usuario sin briefs asociados<br/>3. Click "Eliminar"<br/>4. Confirmar | Usuario se elimina de la BD. No aparece en lista. | ⏳ | Verificar constraints de FK |
| ADM-USR-011 | Intentar eliminar usuario con briefs | P2 | 1. Seleccionar usuario con briefs activos<br/>2. Intentar eliminar | Sistema muestra error por integridad referencial. Usuario no se elimina. | ⏳ | |

---

### Módulo: Gestión de Briefs

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-BRF-001 | Ver todos los briefs del sistema | P1 | 1. Iniciar sesión como admin<br/>2. Navegar a /Brief | Sistema muestra TODOS los briefs (de todos los usuarios). Actualmente 24 briefs. | ⏳ | |
| ADM-BRF-002 | Crear nuevo brief completo | P1 | 1. En /Brief<br/>2. Click "Nuevo Brief"<br/>3. Completar:<br/>   - Nombre: "Campaña Test Admin"<br/>   - Ciclo: "C1 2025"<br/>   - Marca: "Natura"<br/>   - Links: "https://ref1.com, https://ref2.com"<br/>   - Subir archivo PDF<br/>4. Agregar material:<br/>   - Nombre: "Banner Principal"<br/>   - Mensaje: "Texto del banner"<br/>   - PCNs: Natura, Ekos (múltiples)<br/>   - Formato: Banner<br/>   - Audiencia: Consultoras<br/>   - Prioridad: Alta<br/>   - Responsable: "Juan Pérez"<br/>   - Área: "Marketing"<br/>   - Fecha entrega: Mañana<br/>5. Agregar segundo material<br/>6. Guardar | Brief se crea con ID único. Ambos materiales se guardan. Links y archivo se asocian correctamente. PCNs múltiples se guardan en MaterialPCN. | ⏳ | Verificar relación N:N con PCN |
| ADM-BRF-003 | Crear brief sin materiales | P2 | 1. Intentar crear brief solo con datos generales<br/>2. Sin agregar materiales<br/>3. Guardar | Sistema muestra error "Debe agregar al menos un material". No crea brief. | ⏳ | Validar RN-012 |
| ADM-BRF-004 | Editar brief de otro usuario | P1 | 1. En lista de briefs<br/>2. Seleccionar brief creado por Usuario<br/>3. Click "Editar"<br/>4. Modificar nombre del proyecto<br/>5. Guardar | Sistema permite edición (admin tiene permisos). Cambios se guardan. | ⏳ | |
| ADM-BRF-005 | Eliminar brief | P1 | 1. Seleccionar un brief<br/>2. Click "Eliminar"<br/>3. Confirmar eliminación | Brief y todos sus materiales se eliminan (CASCADE). Ya no aparece en lista. | ⏳ | Verificar eliminación en cascada |
| ADM-BRF-006 | Ver detalle de brief | P1 | 1. Click en un brief de la lista | Sistema muestra:<br/>- Datos del brief<br/>- Lista de materiales<br/>- Links de referencias (clickeables)<br/>- Archivo adjunto (descargable)<br/>- Lista de participantes | ⏳ | |
| ADM-BRF-007 | Filtrar briefs por nombre | P2 | 1. En /Brief<br/>2. Ingresar texto en filtro de nombre<br/>3. Aplicar filtro | Sistema muestra solo briefs que coinciden con el filtro. | ⏳ | |
| ADM-BRF-008 | Agregar participante a brief | P2 | 1. Abrir detalle de brief<br/>2. Click "Agregar Participante"<br/>3. Buscar usuario (escribir min 3 caracteres)<br/>4. Seleccionar de resultados<br/>5. Confirmar | Participante se agrega. Recibe notificación por correo. Se crea alerta. Aparece en lista de participantes. | ⏳ | |
| ADM-BRF-009 | Eliminar participante de brief | P2 | 1. En detalle de brief<br/>2. Click "Eliminar" junto a participante<br/>3. Confirmar | Participante se remueve. Ya no aparece en lista. | ⏳ | |
| ADM-BRF-010 | Descargar archivo adjunto del brief | P2 | 1. Abrir brief con archivo<br/>2. Click en link del archivo | Archivo se descarga correctamente. | ⏳ | |

---

### Módulo: Gestión de Materiales

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-MAT-001 | Ver todos los materiales del sistema | P1 | 1. Iniciar sesión como admin<br/>2. Navegar a /Materiales | Sistema muestra TODOS los materiales del sistema. Muestra contadores por estatus. | ⏳ | |
| ADM-MAT-002 | Ver contadores de estatus | P1 | 1. En /Materiales<br/>2. Observar contadores superiores | Sistema muestra 6 contadores:<br/>- En Revisión<br/>- En Producción<br/>- Falta Información<br/>- Aprobado<br/>- Programado<br/>- Entregado<br/>Con cantidades correctas. | ⏳ | |
| ADM-MAT-003 | Filtrar materiales por nombre | P2 | 1. En /Materiales<br/>2. Ingresar texto en "Nombre Material"<br/>3. Aplicar | Solo se muestran materiales que contienen el texto. | ⏳ | |
| ADM-MAT-004 | Filtrar por nombre de proyecto | P2 | 1. Ingresar texto en "Nombre Proyecto"<br/>2. Aplicar | Solo se muestran materiales cuyos briefs contienen el texto. | ⏳ | |
| ADM-MAT-005 | Filtrar por área | P2 | 1. Ingresar texto en "Área"<br/>2. Aplicar | Solo se muestran materiales del área filtrada. | ⏳ | |
| ADM-MAT-006 | Filtrar por responsable | P2 | 1. Ingresar texto en "Responsable"<br/>2. Aplicar | Solo se muestran materiales del responsable. | ⏳ | |
| ADM-MAT-007 | Filtrar por estatus | P2 | 1. Seleccionar estatus del dropdown<br/>2. Aplicar | Solo se muestran materiales con ese estatus. | ⏳ | |
| ADM-MAT-008 | Filtrar por rango de fechas | P2 | 1. Seleccionar fecha inicio<br/>2. Seleccionar fecha fin<br/>3. Aplicar | Solo materiales con entrega en ese rango. | ⏳ | |
| ADM-MAT-009 | Exportar materiales filtrados a Excel | P2 | 1. Aplicar filtros deseados<br/>2. Click "Exportar a Excel" | Archivo "MaterialesFiltrados.xlsx" se descarga con:<br/>- Nombre de Material<br/>- Mensaje<br/>- PCN<br/>- Formato<br/>- Estatus<br/>- Nombre del Proyecto<br/>- Audiencia<br/>- Responsable<br/>- Área<br/>- Fecha de Entrega | ⏳ | |
| ADM-MAT-010 | Editar material - Cambiar estatus | P1 | 1. Click "Editar" en un material<br/>2. Modal se abre mostrando:<br/>   - Nombre material<br/>   - Nombre brief<br/>   - Links referencias<br/>   - Archivo brief<br/>   - Fecha entrega (editable)<br/>   - Estatus (dropdown)<br/>   - Editor TinyMCE para comentario<br/>3. Cambiar estatus de "En Revisión" a "En Producción"<br/>4. Agregar comentario: "Iniciando producción"<br/>5. Click "Agregar Comentario" | Sistema guarda en HistorialMaterial. Actualiza EstatusMaterialId del material. Crea alerta para usuario del brief: "Cambio de Estatus". Crea alerta: "Nuevo Comentario". Modal se cierra. Lista se actualiza. | ⏳ | Verificar ambas alertas |
| ADM-MAT-011 | Editar material - Cambiar fecha entrega | P1 | 1. Editar material<br/>2. Cambiar fecha de entrega a fecha futura válida<br/>3. Agregar comentario<br/>4. Guardar | Fecha se actualiza. Se registra en historial. | ⏳ | |
| ADM-MAT-012 | Editar material - Fecha inválida (pasada) | P1 | 1. Editar material<br/>2. Intentar poner fecha anterior a hoy<br/>3. Guardar | Sistema muestra error: "La fecha de entrega no puede ser anterior a la fecha actual". No guarda. | ⏳ | Validar RN-011 |
| ADM-MAT-013 | Cambiar a estatus "Entregado" | P1 | 1. Editar material<br/>2. Cambiar estatus a "Entregado" (Id=5)<br/>3. Agregar comentario: "Material completado"<br/>4. Guardar | Sistema crea TRES alertas:<br/>1. "Nuevo Comentario"<br/>2. "Cambio de Estatus"<br/>3. "Material Entregado" (especial)<br/>Todas para usuario del brief. | ⏳ | Verificar RN-024 |
| ADM-MAT-014 | Agregar comentario con formato | P2 | 1. Editar material<br/>2. En editor TinyMCE agregar:<br/>   - Texto en negrita<br/>   - Texto en cursiva<br/>   - Lista con viñetas<br/>   - Link<br/>3. Guardar | Comentario se guarda con formato HTML. Se visualiza correctamente en historial. | ⏳ | Verificar RN-023 |
| ADM-MAT-015 | Subir imagen en comentario | P2 | 1. Editar material<br/>2. En TinyMCE click botón imagen<br/>3. Seleccionar imagen local<br/>4. Imagen se sube<br/>5. Agregar texto<br/>6. Guardar | Imagen se sube a /wwwroot/uploads. Se guarda URL en comentario. Imagen se visualiza en historial. | ⏳ | Endpoint: /Materiales/upload |
| ADM-MAT-016 | Agregar participante para notificar | P2 | 1. Editar material<br/>2. En campo búsqueda escribir nombre (min 3 chars)<br/>3. Seleccionar usuario de resultados | Usuario se agrega a lista temporal. Sistema envía notificación INMEDIATA: Alerta "Te agregaron como participante en material X". | ⏳ | Verificar RN-026 |
| ADM-MAT-017 | Eliminar participante de notificación | P2 | 1. Con participantes agregados<br/>2. Click botón eliminar (icono basura roja)<br/>3. Confirmar | Participante se remueve de lista temporal. | ⏳ | |
| ADM-MAT-018 | Enviar correo a participantes | P2 | 1. Editar material<br/>2. Agregar 2 participantes<br/>3. Seleccionar "Enviar Correo: Sí"<br/>4. Agregar comentario<br/>5. Guardar | Sistema envía correo a:<br/>- Participantes seleccionados<br/>- Todos los usuarios rol Producción (RolId=3)<br/>Correo incluye: nombre material, usuario, estatus, comentario, link. | ⏳ | Template: ComentarioMaterial |
| ADM-MAT-019 | Intentar enviar correo sin participantes | P2 | 1. Editar material<br/>2. Seleccionar "Enviar Correo: Sí"<br/>3. NO agregar participantes<br/>4. Intentar guardar | Sistema muestra error: "Debe agregar al menos un participante para enviar notificaciones por correo". No guarda. | ⏳ | |
| ADM-MAT-020 | Ver historial completo de material | P1 | 1. Editar material<br/>2. Observar sección historial | Sistema muestra todas las entradas cronológicamente (más reciente primero):<br/>- Fecha y hora<br/>- Usuario que comentó<br/>- Comentarios (con formato HTML)<br/>- Estatus en ese momento<br/>- Fecha de entrega en ese momento | ⏳ | |
| ADM-MAT-021 | Paginación de materiales | P2 | 1. En /Materiales con más de 8 materiales<br/>2. Observar paginación | Sistema muestra 8 materiales por página. Controles de paginación funcionan. | ⏳ | pageSize=8 |
| ADM-MAT-022 | Material sin PCNs | P3 | 1. Ver material sin PCNs asignados | Sistema muestra "N/A" en columna PCN. | ⏳ | |

---

### Módulo: Invitaciones (Solicitudes de Usuario)

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-INV-001 | Ver lista de solicitudes pendientes | P1 | 1. Iniciar sesión como admin<br/>2. Navegar a /Invitaciones | Sistema muestra solicitudes con SolicitudRegistro=true. Para cada una:<br/>- Nombre completo<br/>- Correo<br/>- Fecha de solicitud<br/>- Botones Aprobar/Rechazar | ⏳ | Solo visible para admin |
| ADM-INV-002 | Aprobar solicitud de usuario | P1 | 1. En /Invitaciones<br/>2. Click "Aprobar" en solicitud<br/>3. Confirmar | Usuario se marca como aprobado. Recibe correo de bienvenida. Puede iniciar sesión. | ⏳ | |
| ADM-INV-003 | Rechazar solicitud de usuario | P2 | 1. Click "Rechazar"<br/>2. Confirmar | Solicitud se marca como rechazada. Usuario no puede acceder. | ⏳ | |
| ADM-INV-004 | Recibir alerta de nueva solicitud | P1 | 1. Usuario nuevo se registra desde /Login/Registro<br/>2. Revisar alertas de admin | Sistema crea alerta tipo 2: "Solicitud Usuario Nuevo". Alerta incluye link a /Invitaciones. Contador de alertas aumenta. | ⏳ | |

---

### Módulo: Calendario

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-CAL-001 | Ver calendario con todos los materiales | P1 | 1. Iniciar sesión como admin<br/>2. Navegar a /Calendario | Sistema muestra calendario mensual. Fechas con entregas están marcadas. Admin ve TODOS los materiales programados. | ⏳ | |
| ADM-CAL-002 | Navegar entre meses | P2 | 1. En /Calendario<br/>2. Click flechas prev/next | Calendario cambia de mes. Entregas se actualizan. | ⏳ | |
| ADM-CAL-003 | Ver detalle de entrega en fecha | P2 | 1. Click en fecha con entregas | Sistema muestra lista de materiales con entrega ese día. Permite acceder al material. | ⏳ | |

---

### Módulo: Alertas

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-ALE-001 | Ver contador de alertas no leídas | P1 | 1. Tener alertas sin leer<br/>2. Observar ícono de alertas | Contador muestra número correcto de alertas no leídas (lectura=false). | ⏳ | |
| ADM-ALE-002 | Ver lista de alertas | P1 | 1. Click en ícono de alertas | Sistema muestra dropdown con todas las alertas del admin:<br/>- Solicitudes de usuarios<br/>- Comentarios en briefs propios<br/>- Cambios de estatus<br/>Ordenadas por fecha (más reciente primero). | ⏳ | |
| ADM-ALE-003 | Marcar alerta como leída | P1 | 1. Click en una alerta | Alerta se marca como leída (lectura=true). Contador disminuye. Sistema redirige a la acción de la alerta. | ⏳ | |
| ADM-ALE-004 | Eliminar alerta | P2 | 1. Click en eliminar (icono X)<br/>2. Confirmar | Alerta se elimina de BD. Ya no aparece en lista. | ⏳ | |

---

### Módulo: Catálogos

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-CAT-001 | Acceder a módulo de catálogos | P1 | 1. Iniciar sesión como admin<br/>2. Navegar a /Catalogos | Sistema muestra opciones para gestionar:<br/>- PCN<br/>- Formatos<br/>- Audiencias<br/>- Prioridades<br/>- Estatus de Materiales | ⏳ | Solo admin tiene acceso |
| ADM-CAT-002 | Agregar nuevo PCN | P2 | 1. En Catálogos > PCN<br/>2. Click "Nuevo"<br/>3. Ingresar descripción: "Avon"<br/>4. Guardar | PCN se crea en BD. Aparece en lista. Disponible en formularios de materiales. | ⏳ | |
| ADM-CAT-003 | Editar PCN existente | P2 | 1. Seleccionar PCN<br/>2. Click "Editar"<br/>3. Modificar descripción<br/>4. Guardar | Cambios se guardan. Se reflejan en todos los materiales asociados. | ⏳ | |
| ADM-CAT-004 | Eliminar PCN sin uso | P2 | 1. Crear PCN nuevo sin materiales asociados<br/>2. Intentar eliminar | PCN se elimina exitosamente. | ⏳ | |
| ADM-CAT-005 | Intentar eliminar PCN en uso | P2 | 1. Seleccionar PCN usado en materiales<br/>2. Intentar eliminar | Sistema muestra error por FK constraint. PCN no se elimina. | ⏳ | Validar RN-029 |
| ADM-CAT-006 | Gestionar catálogo de Formatos | P2 | Similar a PCN | Todas las operaciones CRUD funcionan correctamente. | ⏳ | |
| ADM-CAT-007 | Gestionar catálogo de Audiencias | P2 | Similar a PCN | Todas las operaciones CRUD funcionan correctamente. | ⏳ | |
| ADM-CAT-008 | Gestionar catálogo de Prioridades | P2 | Similar a PCN | Todas las operaciones CRUD funcionan correctamente. | ⏳ | |
| ADM-CAT-009 | Gestionar catálogo de Estatus | P2 | Similar a PCN | Todas las operaciones CRUD funcionan correctamente. | ⏳ | |

---

### Módulo: Correos

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| ADM-COR-001 | Ver plantillas de correo | P2 | 1. Navegar a /Correos | Sistema muestra todas las plantillas:<br/>- RegistroUsuario<br/>- RegistroUsuarioAdmin<br/>- RegistroParticipante<br/>- ComentarioMaterial<br/>- NuevoBrief | ⏳ | |
| ADM-COR-002 | Editar plantilla de correo | P2 | 1. Seleccionar plantilla<br/>2. Editar HTML<br/>3. Guardar | Plantilla se actualiza. Correos futuros usan nueva plantilla. | ⏳ | |
| ADM-COR-003 | Usar variables dinámicas | P2 | 1. En edición de plantilla<br/>2. Agregar variables como {{nombre}}, {{link}}<br/>3. Guardar | Variables se reemplazan correctamente al enviar correos. | ⏳ | |

---

## RESUMEN - ADMINISTRADOR

### Total de Casos de Prueba: 75

| Módulo | Cantidad | P1 | P2 | P3 |
|--------|----------|----|----|-----|
| Autenticación | 5 | 4 | 1 | 0 |
| Usuarios | 11 | 9 | 2 | 0 |
| Briefs | 10 | 6 | 4 | 0 |
| Materiales | 22 | 11 | 10 | 1 |
| Invitaciones | 4 | 3 | 1 | 0 |
| Calendario | 3 | 1 | 2 | 0 |
| Alertas | 4 | 3 | 1 | 0 |
| Catálogos | 9 | 1 | 8 | 0 |
| Correos | 3 | 0 | 3 | 0 |
| **TOTAL** | **75** | **38** | **32** | **1** |

---

## MATRIZ DE PRUEBAS - ROL USUARIO

### Módulo: Autenticación

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| USR-AUTH-001 | Registrarse como nuevo usuario | P1 | 1. Navegar a /Login<br/>2. Click "Registrarse"<br/>3. Completar:<br/>   - Nombre: "Nuevo"<br/>   - Apellido Paterno: "Usuario"<br/>   - Apellido Materno: "Test"<br/>   - Correo: "nuevo@test.com"<br/>   - Contraseña: "Test123$"<br/>   - Confirmar Contraseña: "Test123$"<br/>4. Click "Registrarse" | Sistema crea usuario con:<br/>- RolId=2 (Usuario)<br/>- Estatus=true (Activo)<br/>- SolicitudRegistro=true<br/>Usuario recibe correo de confirmación. Admin recibe correo y alerta. Redirige a login. | ⏳ | Validar RN-003, RN-004, RN-005 |
| USR-AUTH-002 | Iniciar sesión con credenciales válidas | P1 | 1. Navegar a /Login<br/>2. Ingresar correo de usuario<br/>3. Ingresar contraseña<br/>4. Click "Iniciar Sesión" | Sistema autentica. Redirige a /Brief. Muestra menú de usuario (sin opciones de admin). | ⏳ | |
| USR-AUTH-003 | Cambiar contraseña propia | P2 | Similar a ADM-AUTH-005 | Contraseña se actualiza correctamente. | ⏳ | |
| USR-AUTH-004 | Intentar acceso con usuario inactivo | P1 | 1. Admin desactiva al usuario<br/>2. Usuario intenta login | Sistema deniega acceso con mensaje apropiado. | ⏳ | Validar RN-009 |
| USR-AUTH-005 | Cerrar sesión | P1 | 1. Click "Cerrar Sesión" | Sistema cierra sesión y redirige a login. | ⏳ | |

---

### Módulo: Gestión de Briefs

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| USR-BRF-001 | Ver solo mis briefs | P1 | 1. Iniciar sesión como usuario<br/>2. Navegar a /Brief | Sistema muestra SOLO los briefs donde UsuarioId = usuario logueado. NO muestra briefs de otros usuarios. | ⏳ | Validar RN-014 |
| USR-BRF-002 | Crear nuevo brief con múltiples materiales | P1 | 1. En /Brief<br/>2. Click "Nuevo Brief"<br/>3. Completar datos del brief<br/>4. Agregar Material 1 con 3 PCNs diferentes<br/>5. Agregar Material 2 con 2 PCNs<br/>6. Guardar | Brief se crea con UsuarioId del usuario logueado. Ambos materiales se crean. Relaciones MaterialPCN se guardan correctamente (5 registros total). | ⏳ | Validar RN-013 |
| USR-BRF-003 | Intentar crear brief sin materiales | P2 | 1. Completar solo datos generales<br/>2. No agregar materiales<br/>3. Intentar guardar | Sistema muestra error: "Debe agregar al menos un material". No crea brief. | ⏳ | Validar RN-012 |
| USR-BRF-004 | Editar mi propio brief | P1 | 1. Seleccionar brief propio<br/>2. Click "Editar"<br/>3. Modificar nombre y ciclo<br/>4. Guardar | Sistema permite edición. Cambios se guardan. | ⏳ | Validar RN-016 |
| USR-BRF-005 | Intentar editar brief de otro usuario | P1 | 1. Usuario B crea brief<br/>2. Usuario A intenta acceder a ese brief por URL directa | Sistema deniega acceso o no muestra el brief en la lista. | ⏳ | Seguridad: Validar RN-016 |
| USR-BRF-006 | Agregar participante a mi brief | P2 | 1. En mi brief<br/>2. Agregar participante<br/>3. Buscar y seleccionar | Participante se agrega. Recibe correo y alerta. Aparece en lista. | ⏳ | |
| USR-BRF-007 | Eliminar participante de mi brief | P2 | 1. En mi brief<br/>2. Eliminar participante | Participante se remueve correctamente. | ⏳ | |
| USR-BRF-008 | Subir archivo en brief | P2 | 1. Al crear/editar brief<br/>2. Seleccionar archivo PDF<br/>3. Guardar | Archivo se sube a servidor. RutaArchivo y NombreArchivo se guardan en BD. | ⏳ | |
| USR-BRF-009 | Ver briefs donde soy participante | P2 | 1. Otro usuario me agrega como participante<br/>2. Navegar a /Brief | Brief aparece en mi lista (aunque no sea propietario). Puedo ver detalle. | ⏳ | |
| USR-BRF-010 | Filtrar mis briefs por nombre | P2 | 1. En /Brief<br/>2. Aplicar filtro de nombre | Solo mis briefs que coinciden se muestran. | ⏳ | |

---

### Módulo: Gestión de Materiales

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| USR-MAT-001 | Ver solo materiales de mis briefs | P1 | 1. Iniciar sesión como usuario<br/>2. Navegar a /Materiales | Sistema muestra SOLO materiales de briefs donde soy propietario o participante. NO muestra materiales de otros usuarios. | ⏳ | Validar RN-019 |
| USR-MAT-002 | Ver contadores de mis materiales | P1 | 1. En /Materiales<br/>2. Observar contadores | Contadores muestran cantidades solo de mis materiales, no del sistema completo. | ⏳ | |
| USR-MAT-003 | Agregar comentario a material propio | P1 | 1. Click "Editar" en material de mi brief<br/>2. Modal se abre<br/>3. Agregar comentario: "Requiero cambio de fecha"<br/>4. Fecha de entrega: dejar igual<br/>5. Estatus: NO CAMBIAR (usuario no puede)<br/>6. Click "Agregar Comentario" | Comentario se guarda en HistorialMaterial. Se crea alerta "Nuevo Comentario" para mí mismo (o producción). Material NO cambia de estatus. Fecha NO cambia. | ⏳ | Validar RN-021 |
| USR-MAT-004 | Intentar cambiar estatus de material | P1 | 1. Editar material<br/>2. Intentar cambiar estatus<br/>3. Agregar comentario<br/>4. Guardar | Sistema muestra error: "No tiene permisos para cambiar el estatus del material". Comentario NO se guarda si intentó cambiar estatus. | ⏳ | CRÍTICO: Validar RN-021 |
| USR-MAT-005 | Intentar cambiar fecha de entrega | P1 | 1. Editar material<br/>2. Intentar cambiar fecha<br/>3. Agregar comentario<br/>4. Guardar | Sistema muestra error o ignora cambio de fecha (usuario no puede modificarla). | ⏳ | Usuario solo puede comentar |
| USR-MAT-006 | Agregar comentario con formato | P2 | 1. Editar material<br/>2. En TinyMCE formatear texto:<br/>   - Negrita<br/>   - Cursiva<br/>   - Lista<br/>3. Guardar | Comentario se guarda con formato HTML. Se visualiza correctamente. | ⏳ | |
| USR-MAT-007 | Subir imagen en comentario | P2 | 1. Editar material<br/>2. Agregar imagen desde TinyMCE<br/>3. Guardar | Imagen se sube. URL se guarda en comentario. Se visualiza en historial. | ⏳ | |
| USR-MAT-008 | Ver historial de material | P1 | 1. Editar material<br/>2. Ver historial | Sistema muestra todo el historial del material, incluyendo comentarios de Producción. | ⏳ | |
| USR-MAT-009 | Filtrar materiales por estatus | P2 | 1. Seleccionar estatus<br/>2. Aplicar filtro | Solo materiales propios con ese estatus. | ⏳ | |
| USR-MAT-010 | Exportar mis materiales a Excel | P2 | 1. Aplicar filtros<br/>2. Click "Exportar" | Excel contiene solo mis materiales filtrados. | ⏳ | |
| USR-MAT-011 | Ver información del brief desde material | P2 | 1. Editar material<br/>2. Observar datos del brief en modal | Sistema muestra: nombre brief, links, archivo (descargable). | ⏳ | |
| USR-MAT-012 | Recibir alerta de nuevo comentario | P1 | 1. Producción agrega comentario en mi material<br/>2. Revisar alertas | Sistema crea alerta tipo 3: "Nuevo Comentario en Material". Contador aumenta. | ⏳ | Validar RN-025 |
| USR-MAT-013 | Recibir alerta de cambio de estatus | P1 | 1. Producción cambia estatus de mi material<br/>2. Revisar alertas | Sistema crea alerta tipo 4: "Cambio de Estatus". Describe el nuevo estatus. | ⏳ | |
| USR-MAT-014 | Recibir alerta de material entregado | P1 | 1. Producción marca material como "Entregado"<br/>2. Revisar alertas | Sistema crea alerta tipo 5: "Material Entregado". Incluye link al material. | ⏳ | Validar RN-024 |
| USR-MAT-015 | Click en alerta para ver material | P1 | 1. Recibir alerta de material<br/>2. Click en la alerta | Sistema redirige a /Materiales con filtro aplicado al material específico. Alerta se marca como leída. | ⏳ | |

---

### Módulo: Calendario

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| USR-CAL-001 | Ver calendario de mis materiales | P1 | 1. Navegar a /Calendario | Sistema muestra calendario con solo mis materiales programados. NO muestra materiales de otros usuarios. | ⏳ | Validar RN-027 |
| USR-CAL-002 | Ver detalle de entrega | P2 | 1. Click en fecha con entregas | Sistema muestra solo mis materiales con entrega ese día. | ⏳ | |
| USR-CAL-003 | Navegar entre meses | P2 | 1. Click flechas prev/next | Calendario cambia. Entregas se actualizan. | ⏳ | |

---

### Módulo: Alertas

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| USR-ALE-001 | Ver contador de alertas | P1 | 1. Tener alertas sin leer<br/>2. Observar contador | Muestra número correcto de alertas no leídas. | ⏳ | |
| USR-ALE-002 | Ver lista de mis alertas | P1 | 1. Click en ícono alertas | Sistema muestra mis alertas:<br/>- Nuevos comentarios<br/>- Cambios de estatus<br/>- Materiales entregados<br/>- Participación en briefs | ⏳ | |
| USR-ALE-003 | Marcar alerta como leída | P1 | 1. Click en alerta | Alerta se marca como leída. Redirige a recurso. Contador disminuye. | ⏳ | |
| USR-ALE-004 | Eliminar alerta | P2 | 1. Click eliminar<br/>2. Confirmar | Alerta se elimina. | ⏳ | |

---

### Módulo: Intentos de Acceso No Autorizado

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| USR-SEC-001 | Intentar acceder a /Usuarios | P1 | 1. Como usuario<br/>2. Navegar a /Usuarios por URL | Sistema deniega acceso. Muestra error 403 o redirige a /Brief. | ⏳ | SEGURIDAD |
| USR-SEC-002 | Intentar acceder a /Catalogos | P1 | 1. Navegar a /Catalogos por URL | Sistema deniega acceso. | ⏳ | SEGURIDAD |
| USR-SEC-003 | Intentar acceder a /Invitaciones | P1 | 1. Navegar a /Invitaciones por URL | Sistema deniega acceso. | ⏳ | SEGURIDAD |
| USR-SEC-004 | Intentar acceder a /Correos | P1 | 1. Navegar a /Correos por URL | Sistema deniega acceso. | ⏳ | SEGURIDAD |
| USR-SEC-005 | Intentar ver brief de otro usuario por URL | P1 | 1. Obtener ID de brief de otro usuario<br/>2. Navegar a /Brief/Details/{id} | Sistema deniega acceso o muestra vacío. | ⏳ | SEGURIDAD CRÍTICA |
| USR-SEC-006 | Intentar editar material de otro usuario | P1 | 1. Obtener ID de material ajeno<br/>2. Intentar editar por URL/API | Sistema deniega operación. | ⏳ | SEGURIDAD CRÍTICA |

---

## RESUMEN - USUARIO

### Total de Casos de Prueba: 40

| Módulo | Cantidad | P1 | P2 | P3 |
|--------|----------|----|----|-----|
| Autenticación | 5 | 4 | 1 | 0 |
| Briefs | 10 | 5 | 5 | 0 |
| Materiales | 15 | 9 | 6 | 0 |
| Calendario | 3 | 1 | 2 | 0 |
| Alertas | 4 | 3 | 1 | 0 |
| Seguridad | 6 | 6 | 0 | 0 |
| **TOTAL** | **43** | **28** | **15** | **0** |

---

## MATRIZ DE PRUEBAS - ROL PRODUCCIÓN

### Módulo: Autenticación

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| PRO-AUTH-001 | Iniciar sesión como Producción | P1 | 1. Navegar a /Login<br/>2. Ingresar credenciales de producción<br/>3. Click "Iniciar Sesión" | Sistema autentica. Redirige a /Materiales. Muestra menú de producción. | ⏳ | |
| PRO-AUTH-002 | Cambiar contraseña | P2 | Similar a otros roles | Contraseña se actualiza correctamente. | ⏳ | |
| PRO-AUTH-003 | Cerrar sesión | P1 | 1. Click "Cerrar Sesión" | Sesión se cierra. Redirige a login. | ⏳ | |

---

### Módulo: Gestión de Briefs

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| PRO-BRF-001 | Ver todos los briefs del sistema | P1 | 1. Iniciar sesión como producción<br/>2. Navegar a /Brief | Sistema muestra TODOS los briefs (igual que admin). Producción necesita ver todos los proyectos. | ⏳ | Validar RN-015 |
| PRO-BRF-002 | Ver detalle de cualquier brief | P1 | 1. Click en cualquier brief | Sistema permite acceso completo a:<br/>- Datos del brief<br/>- Materiales<br/>- Links y archivos<br/>- Participantes | ⏳ | |
| PRO-BRF-003 | Intentar crear brief | P1 | 1. Buscar botón "Nuevo Brief" | Botón NO debe estar visible para Producción. Si intenta por URL, sistema deniega. | ⏳ | Producción solo ejecuta |
| PRO-BRF-004 | Intentar editar brief | P1 | 1. Intentar editar datos generales de brief | Sistema deniega operación. Producción no puede modificar briefs. | ⏳ | |
| PRO-BRF-005 | Descargar archivo de brief | P1 | 1. Abrir brief con archivo<br/>2. Click en archivo | Archivo se descarga correctamente. Producción necesita acceso a especificaciones. | ⏳ | |

---

### Módulo: Gestión de Materiales

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| PRO-MAT-001 | Ver todos los materiales del sistema | P1 | 1. Iniciar sesión como producción<br/>2. Navegar a /Materiales | Sistema muestra TODOS los materiales (igual que admin). Producción debe ver todo el trabajo. | ⏳ | Validar RN-020 |
| PRO-MAT-002 | Ver contadores globales | P1 | 1. En /Materiales<br/>2. Observar contadores | Contadores muestran totales del sistema completo, no solo de un usuario. | ⏳ | |
| PRO-MAT-003 | Cambiar estatus de material | P1 | 1. Click "Editar" en cualquier material<br/>2. Cambiar estatus de "En Revisión" a "En Producción"<br/>3. Agregar comentario: "Iniciando diseño"<br/>4. Click "Agregar Comentario" | Sistema permite cambio de estatus. Guarda en HistorialMaterial. Actualiza Material.EstatusMaterialId. Crea alertas para usuario del brief:<br/>- "Nuevo Comentario"<br/>- "Cambio de Estatus" | ⏳ | Validar RN-022 |
| PRO-MAT-004 | Modificar fecha de entrega | P1 | 1. Editar material<br/>2. Cambiar fecha a fecha futura válida<br/>3. Agregar comentario: "Ajuste de timeline"<br/>4. Guardar | Sistema permite modificación. Nueva fecha se guarda. Se registra en historial. | ⏳ | Validar RN-022 |
| PRO-MAT-005 | Marcar material como "Entregado" | P1 | 1. Editar material en estatus "Aprobado"<br/>2. Cambiar a "Entregado" (Id=5)<br/>3. Agregar comentario: "Material completado y enviado"<br/>4. Guardar | Sistema guarda cambio. Crea TRES alertas para usuario del brief:<br/>1. "Nuevo Comentario" (tipo 3)<br/>2. "Cambio de Estatus" (tipo 4)<br/>3. "Material Entregado" (tipo 5) | ⏳ | CRÍTICO: Validar RN-024 |
| PRO-MAT-006 | Solicitar información adicional | P1 | 1. Editar material<br/>2. Cambiar estatus a "Falta Información"<br/>3. Agregar comentario detallado: "Necesito especificaciones de medidas y colores"<br/>4. Seleccionar "Enviar Correo: Sí"<br/>5. Agregar al usuario del brief como participante<br/>6. Guardar | Sistema:<br/>- Cambia estatus<br/>- Guarda comentario<br/>- Envía correo al usuario<br/>- Envía correo a todos de Producción<br/>- Crea alertas<br/>Usuario recibe notificación inmediata | ⏳ | Flujo común |
| PRO-MAT-007 | Agregar comentario sin cambiar estatus ni fecha | P2 | 1. Editar material<br/>2. Dejar estatus igual<br/>3. Dejar fecha igual<br/>4. Solo agregar comentario: "Trabajo en progreso 50%"<br/>5. Guardar | Solo se crea entrada en historial. Material no cambia. Se crea alerta "Nuevo Comentario". | ⏳ | |
| PRO-MAT-008 | Agregar comentario con imagen | P1 | 1. Editar material<br/>2. En TinyMCE agregar texto: "Avance del diseño:"<br/>3. Subir imagen JPG desde TinyMCE<br/>4. Agregar más texto<br/>5. Guardar | Imagen se sube a /wwwroot/uploads con GUID. URL se guarda en HTML del comentario. Se visualiza correctamente en historial. | ⏳ | Producción comparte avances |
| PRO-MAT-009 | Notificar a participantes adicionales | P2 | 1. Editar material<br/>2. Agregar comentario de actualización<br/>3. Buscar y agregar 2 usuarios como participantes<br/>4. Seleccionar "Enviar Correo: Sí"<br/>5. Guardar | Sistema:<br/>- Envía notificación inmediata a participantes agregados (alerta)<br/>- Envía correo a participantes seleccionados<br/>- Envía correo a todos de Producción (RolId=3) | ⏳ | |
| PRO-MAT-010 | Procesar múltiples materiales en secuencia | P1 | 1. Filtrar materiales por estatus "En Revisión"<br/>2. Editar primer material:<br/>   - Cambiar a "En Producción"<br/>   - Comentar<br/>   - Guardar<br/>3. Editar segundo material (mismo flujo)<br/>4. Editar tercer material (mismo flujo) | Todos los materiales se actualizan correctamente. Alertas se crean para cada usuario correspondiente. Sin errores de concurrencia. | ⏳ | Flujo real diario |
| PRO-MAT-011 | Filtrar materiales por fecha de entrega | P2 | 1. En /Materiales<br/>2. Filtrar por rango de fechas: próximos 7 días<br/>3. Aplicar | Sistema muestra solo materiales con entrega en ese rango. Producción puede priorizar urgentes. | ⏳ | |
| PRO-MAT-012 | Filtrar por responsable/área | P2 | 1. Filtrar por área: "Marketing"<br/>2. Aplicar | Solo materiales de esa área. Producción puede organizar trabajo. | ⏳ | |
| PRO-MAT-013 | Exportar materiales para reporte | P2 | 1. Filtrar materiales de la semana<br/>2. Click "Exportar a Excel" | Excel se descarga con materiales filtrados. Producción puede reportar avances. | ⏳ | |
| PRO-MAT-014 | Ver historial completo | P1 | 1. Editar material con historial largo<br/>2. Revisar todas las entradas | Sistema muestra cronología completa:<br/>- Comentarios de todos los usuarios<br/>- Cambios de estatus<br/>- Cambios de fecha<br/>Ordenado por fecha (más reciente primero) | ⏳ | |
| PRO-MAT-015 | Intentar fecha inválida | P1 | 1. Editar material<br/>2. Intentar poner fecha anterior a hoy<br/>3. Guardar | Sistema muestra error: "La fecha de entrega no puede ser anterior a la fecha actual". No guarda. | ⏳ | Validar RN-011 |

---

### Módulo: Calendario

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| PRO-CAL-001 | Ver calendario completo | P1 | 1. Navegar a /Calendario | Sistema muestra TODOS los materiales programados (de todos los usuarios). Producción necesita ver todo el trabajo. | ⏳ | |
| PRO-CAL-002 | Identificar entregas urgentes | P1 | 1. En calendario<br/>2. Observar fechas cercanas | Materiales próximos a vencer están visibles. Producción puede priorizar. | ⏳ | |
| PRO-CAL-003 | Acceder a material desde calendario | P2 | 1. Click en fecha con entregas<br/>2. Click en material específico | Sistema redirige a /Materiales con filtro aplicado. | ⏳ | |

---

### Módulo: Alertas

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| PRO-ALE-001 | Recibir alerta de nuevo brief | P2 | 1. Usuario crea nuevo brief<br/>2. Revisar alertas de producción | (Si está implementado) Producción recibe notificación de nuevos proyectos. | ⏳ | Verificar si existe |
| PRO-ALE-002 | Recibir alerta de comentario en material que participo | P2 | 1. Usuario comenta en material donde producción participó<br/>2. Revisar alertas | Alerta "Nuevo Comentario" se crea. | ⏳ | |
| PRO-ALE-003 | Ver y gestionar alertas | P1 | 1. Click en ícono alertas<br/>2. Ver lista<br/>3. Marcar como leídas | Alertas se gestionan correctamente. Contador actualiza. | ⏳ | |
| PRO-ALE-004 | Recibir correos de notificación | P1 | 1. Usuario solicita información (Falta Información)<br/>2. Revisar correo de producción | Producción recibe correo (todos los RolId=3). Template "ComentarioMaterial". | ⏳ | Validar que TODOS producción reciben |

---

### Módulo: Intentos de Acceso No Autorizado

| ID | Caso de Prueba | Prioridad | Pasos | Resultado Esperado | Estado | Notas |
|----|----------------|-----------|-------|-------------------|--------|-------|
| PRO-SEC-001 | Intentar acceder a /Usuarios | P1 | 1. Navegar a /Usuarios por URL | Sistema deniega acceso. | ⏳ | SEGURIDAD |
| PRO-SEC-002 | Intentar acceder a /Catalogos | P1 | 1. Navegar a /Catalogos | Sistema deniega acceso. | ⏳ | SEGURIDAD |
| PRO-SEC-003 | Intentar acceder a /Invitaciones | P1 | 1. Navegar a /Invitaciones | Sistema deniega acceso. | ⏳ | SEGURIDAD |
| PRO-SEC-004 | Intentar crear brief por URL | P1 | 1. Navegar a /Brief/Create<br/>2. Intentar POST | Sistema deniega operación. | ⏳ | SEGURIDAD |
| PRO-SEC-005 | Intentar eliminar material | P1 | 1. Intentar DELETE en material | Sistema deniega (solo admin puede eliminar). | ⏳ | SEGURIDAD |

---

## RESUMEN - PRODUCCIÓN

### Total de Casos de Prueba: 33

| Módulo | Cantidad | P1 | P2 | P3 |
|--------|----------|----|----|-----|
| Autenticación | 3 | 2 | 1 | 0 |
| Briefs | 5 | 5 | 0 | 0 |
| Materiales | 15 | 11 | 4 | 0 |
| Calendario | 3 | 2 | 1 | 0 |
| Alertas | 4 | 2 | 2 | 0 |
| Seguridad | 5 | 5 | 0 | 0 |
| **TOTAL** | **35** | **27** | **8** | **0** |

---

## PRUEBAS DE INTEGRACIÓN

### Flujos End-to-End

| ID | Escenario Completo | Prioridad | Pasos Detallados | Resultado Esperado | Estado |
|----|-------------------|-----------|------------------|-------------------|--------|
| INT-001 | Ciclo completo: Solicitud → Producción → Entrega | P1 | **USUARIO:**<br/>1. Crear brief "Campaña Verano"<br/>2. Agregar material "Banner Instagram"<br/>3. Guardar<br/><br/>**PRODUCCIÓN:**<br/>4. Ver material en /Materiales<br/>5. Cambiar estatus a "En Producción"<br/>6. Comentar con avance<br/>7. Subir imagen de avance<br/>8. Cambiar a "Aprobado"<br/>9. Cambiar a "Entregado"<br/><br/>**USUARIO:**<br/>10. Recibir 3 alertas:<br/>    - Nuevo Comentario<br/>    - Cambio de Estatus (2 veces)<br/>    - Material Entregado<br/>11. Revisar historial completo | TODO el flujo funciona sin errores. Todas las alertas se crean. Historial muestra cronología completa. | ⏳ |
| INT-002 | Múltiples participantes en brief | P2 | **USUARIO A:**<br/>1. Crear brief<br/>2. Agregar Usuario B como participante<br/>3. Agregar Usuario C como participante<br/><br/>**USUARIOS B y C:**<br/>4. Reciben alertas<br/>5. Reciben correos<br/>6. Pueden ver el brief<br/><br/>**PRODUCCIÓN:**<br/>7. Agrega comentario<br/>8. Selecciona "Enviar Correo: Sí"<br/>9. Agrega B y C como destinatarios<br/><br/>**VERIFICACIÓN:**<br/>10. B y C reciben correo<br/>11. B y C reciben alerta<br/>12. Todos de Producción reciben correo | Sistema notifica correctamente a todos los participantes. | ⏳ |
| INT-003 | Material con múltiples PCNs | P1 | 1. Crear brief<br/>2. Crear material con PCNs: Natura, Ekos, Plant<br/>3. Guardar<br/>4. Verificar en BD tabla MaterialPCN<br/>5. Ver material en /Materiales<br/>6. Exportar a Excel | 3 registros en MaterialPCN. Columna PCN muestra "Natura, Ekos, Plant" separados por comas. Excel muestra los PCNs correctamente. | ⏳ |
| INT-004 | Cambio de rol de usuario | P2 | 1. Admin crea usuario con rol Usuario<br/>2. Usuario crea 2 briefs<br/>3. Usuario ve solo sus 2 briefs<br/>4. Admin cambia rol a Producción<br/>5. Usuario cierra sesión<br/>6. Usuario inicia sesión nuevamente<br/>7. Usuario ahora ve TODOS los briefs<br/>8. Usuario puede cambiar estatus de materiales | Permisos cambian correctamente según el nuevo rol. | ⏳ |
| INT-005 | Desactivación de usuario | P1 | 1. Usuario A tiene 3 briefs activos<br/>2. Admin desactiva Usuario A<br/>3. Usuario A intenta login<br/>4. Login falla<br/>5. Admin reactiva Usuario A<br/>6. Usuario A puede login<br/>7. Sus 3 briefs siguen existiendo | Desactivación no elimina datos. Solo bloquea acceso. Reactivación restaura acceso. | ⏳ |
| INT-006 | Fecha de entrega pasada | P1 | 1. Material con fecha de entrega = Ayer<br/>2. Producción intenta cambiar fecha a 3 días atrás<br/>3. Sistema muestra error<br/>4. Producción cambia a fecha futura<br/>5. Se guarda correctamente | Sistema valida fechas en tiempo real. | ⏳ |
| INT-007 | Eliminación en cascada de brief | P1 | 1. Crear brief con 5 materiales<br/>2. Cada material tiene:<br/>   - 3 entradas en historial<br/>   - 2 PCNs<br/>3. Admin elimina el brief<br/>4. Verificar en BD:<br/>   - Brief eliminado<br/>   - 5 Materiales eliminados<br/>   - 15 HistorialMaterial eliminados<br/>   - 10 MaterialPCN eliminados | Eliminación en cascada funciona correctamente. Integridad referencial OK. | ⏳ |
| INT-008 | Correo duplicado | P1 | 1. Crear usuario: test@test.com<br/>2. Intentar crear otro: test@test.com<br/>3. Sistema muestra error<br/>4. Usuario existente intenta cambiar email a otro existente<br/>5. Error | Unicidad de correo se mantiene en todas las operaciones. | ⏳ |
| INT-009 | TinyMCE - Upload de imagen | P1 | 1. Editar material<br/>2. En TinyMCE click "Insert Image"<br/>3. Seleccionar imagen 2MB JPG<br/>4. Imagen se sube<br/>5. Guardar comentario<br/>6. Verificar en servidor:<br/>   - Archivo en /wwwroot/uploads<br/>   - Nombre con GUID<br/>7. Ver historial<br/>8. Imagen se visualiza | Upload funciona. Imágenes se almacenan y visualizan correctamente. | ⏳ |
| INT-010 | Concurrencia - Dos usuarios editan mismo material | P2 | 1. Producción A abre material para editar<br/>2. Producción B abre mismo material<br/>3. A cambia estatus a "En Producción" y guarda<br/>4. B cambia estatus a "Aprobado" y guarda<br/>5. Verificar historial<br/>6. Verificar estatus final | Ambos cambios se registran en historial. Estatus final es el último guardado (B). Sin pérdida de datos. | ⏳ |

---

## CRITERIOS DE ACEPTACIÓN

### Criterios Generales

#### 1. Funcionalidad
- ✅ Todas las pruebas de prioridad P1 deben pasar
- ✅ Al menos 95% de pruebas P2 deben pasar
- ✅ Sin errores críticos de seguridad
- ✅ Sin pérdida de datos en operaciones CRUD

#### 2. Permisos y Seguridad
- ✅ Cada rol solo puede acceder a funciones permitidas
- ✅ Intentos de acceso no autorizados son bloqueados
- ✅ No hay SQL injection, XSS, o CSRF vulnerabilities
- ✅ Contraseñas nunca se muestran en texto plano

#### 3. Integridad de Datos
- ✅ Relaciones de llaves foráneas se mantienen
- ✅ Eliminaciones en cascada funcionan correctamente
- ✅ No hay registros huérfanos
- ✅ Constraints de unicidad se respetan

#### 4. Notificaciones
- ✅ Alertas se crean en todos los eventos especificados
- ✅ Correos se envían a destinatarios correctos
- ✅ Templates de correo se procesan correctamente
- ✅ Variables dinámicas se reemplazan

#### 5. Experiencia de Usuario
- ✅ Mensajes de error son claros y descriptivos
- ✅ Sin errores en consola del navegador
- ✅ Tiempos de respuesta < 2 segundos
- ✅ Interfaz responsive en dispositivos móviles

---

## REPORTE DE EJECUCIÓN

### Template de Reporte

```
REPORTE DE PRUEBAS - [ROL]
Fecha de Ejecución: [FECHA]
Ejecutado por: [NOMBRE]

RESUMEN:
- Total de Pruebas: [N]
- Pasadas (PASS): [N]
- Fallidas (FAIL): [N]
- Bloqueadas (BLOCKED): [N]
- Pendientes (PENDING): [N]

PRUEBAS FALLIDAS:
[ID] - [Nombre]
  Error: [Descripción del error]
  Severidad: [P1/P2/P3]
  Screenshot: [URL]

OBSERVACIONES:
- [Nota 1]
- [Nota 2]

RECOMENDACIONES:
- [Recomendación 1]
- [Recomendación 2]
```

---

## CHECKLIST PRE-PRODUCCIÓN

Antes de dar por aceptado el sistema, verificar:

- [ ] Todas las pruebas P1 de Administrador pasan
- [ ] Todas las pruebas P1 de Usuario pasan
- [ ] Todas las pruebas P1 de Producción pasan
- [ ] Todas las pruebas de Seguridad pasan
- [ ] Todas las pruebas de Integración pasan
- [ ] Backup de base de datos configurado
- [ ] Logs de error funcionando
- [ ] SSL/TLS configurado (producción)
- [ ] Variables de entorno configuradas
- [ ] Correos electrónicos funcionando
- [ ] Usuarios de producción creados
- [ ] Catálogos iniciales cargados
- [ ] Documentación entregada
- [ ] Capacitación completada

---

**FIN DEL DOCUMENTO DE MATRICES DE PRUEBAS**
