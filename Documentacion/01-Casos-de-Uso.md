# SISTEMA DE ADMINISTRACIÓN DE PROYECTOS NATURA
## DOCUMENTO DE CASOS DE USO

**Versión:** 1.0
**Fecha:** 2025-01-09
**Cliente:** Natura
**Empresa:** Entersys

---

## TABLA DE CONTENIDOS

1. [Introducción](#introducción)
2. [Actores del Sistema](#actores-del-sistema)
3. [Diagrama General de Casos de Uso](#diagrama-general-de-casos-de-uso)
4. [Casos de Uso Detallados](#casos-de-uso-detallados)

---

## INTRODUCCIÓN

### Propósito del Documento
Este documento describe los casos de uso del Sistema de Administración de Proyectos de Natura, diseñado para gestionar briefs, materiales, usuarios y seguimiento de proyectos de marketing.

### Alcance del Sistema
El sistema permite:
- Gestión de briefs de proyectos
- Administración de materiales asociados a briefs
- Control de usuarios y roles
- Sistema de notificaciones y alertas
- Calendario de entregas
- Reportes y seguimiento

### Definiciones y Acrónimos
- **Brief:** Documento que describe los requerimientos de un proyecto
- **Material:** Activo creativo asociado a un brief
- **PCN:** Product Classification Number (Clasificación de Producto)
- **Ciclo:** Periodo temporal de campaña

---

## ACTORES DEL SISTEMA

### 1. Administrador (RolId: 1)
**Descripción:** Usuario con permisos completos en el sistema.

**Responsabilidades:**
- Gestión completa de usuarios
- Aprobación de solicitudes de registro
- Configuración de catálogos
- Acceso a todos los briefs y materiales
- Gestión de permisos y roles

**Objetivos:**
- Mantener el sistema operativo
- Controlar accesos y seguridad
- Supervisar todas las operaciones

---

### 2. Usuario (RolId: 2)
**Descripción:** Usuario solicitante de proyectos y materiales.

**Responsabilidades:**
- Crear y gestionar briefs propios
- Solicitar materiales
- Hacer seguimiento a sus proyectos
- Agregar participantes a briefs
- Consultar estatus de materiales

**Objetivos:**
- Solicitar materiales creativos
- Dar seguimiento a entregas
- Comunicarse con el equipo de producción

---

### 3. Producción (RolId: 3)
**Descripción:** Usuario responsable de la creación de materiales.

**Responsabilidades:**
- Ver todos los briefs y materiales
- Actualizar estatus de materiales
- Agregar comentarios en bitácora
- Gestionar fechas de entrega
- Subir archivos y referencias

**Objetivos:**
- Producir materiales solicitados
- Mantener comunicación con solicitantes
- Cumplir con fechas de entrega

---

## CASOS DE USO POR MÓDULO

---

## MÓDULO: AUTENTICACIÓN Y USUARIOS

### CU-001: Iniciar Sesión
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar registrado y activo en el sistema
- El usuario debe tener credenciales válidas

**Flujo Principal:**
1. El usuario accede a la URL del sistema
2. El sistema muestra la pantalla de login
3. El usuario ingresa su correo electrónico
4. El usuario ingresa su contraseña
5. El usuario hace clic en "Iniciar Sesión"
6. El sistema valida las credenciales
7. El sistema redirige al usuario a su pantalla principal según su rol

**Flujos Alternativos:**
- **FA1: Credenciales Inválidas**
  - Si las credenciales son incorrectas, el sistema muestra mensaje de error
  - El usuario puede intentar nuevamente

- **FA2: Usuario Inactivo**
  - Si el usuario está inactivo, el sistema deniega el acceso
  - El sistema muestra mensaje informativo

**Postcondiciones:**
- El usuario queda autenticado en el sistema
- Se crea una sesión válida

**Reglas de Negocio:**
- RN-001: Las contraseñas deben estar encriptadas
- RN-002: Usuarios inactivos no pueden acceder

---

### CU-002: Registrar Nuevo Usuario (Autónomo)
**Actor Principal:** Usuario Nuevo
**Precondiciones:**
- El usuario no debe estar previamente registrado
- El correo electrónico debe ser válido

**Flujo Principal:**
1. El usuario accede a la opción "Registrarse"
2. El sistema muestra formulario de registro
3. El usuario completa:
   - Nombre
   - Apellido Paterno
   - Apellido Materno
   - Correo Electrónico
   - Contraseña
   - Confirmar Contraseña
4. El usuario hace clic en "Registrarse"
5. El sistema valida los datos ingresados
6. El sistema crea el usuario con:
   - RolId = 2 (Usuario)
   - Estatus = true (Activo)
   - SolicitudRegistro = true
7. El sistema envía correo de confirmación al usuario
8. El sistema notifica a los administradores sobre la nueva solicitud
9. El sistema crea alerta para el administrador
10. El sistema redirige al login

**Flujos Alternativos:**
- **FA1: Correo Duplicado**
  - Si el correo ya existe, el sistema muestra error
  - El usuario debe usar otro correo

- **FA2: Contraseñas No Coinciden**
  - Si las contraseñas no coinciden, el sistema muestra error
  - El usuario debe corregir

**Postcondiciones:**
- Se crea un nuevo usuario en estado activo
- Se envían notificaciones por correo
- Se genera alerta para administrador

**Reglas de Negocio:**
- RN-003: Usuarios nuevos se crean activos por defecto
- RN-004: Rol por defecto es Usuario (RolId=2)
- RN-005: El correo debe ser único en el sistema

---

### CU-003: Crear Usuario (Administrador)
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado
- El correo del nuevo usuario no debe existir

**Flujo Principal:**
1. El administrador accede al módulo de Usuarios
2. El administrador hace clic en "Nuevo Usuario"
3. El sistema muestra formulario de creación
4. El administrador completa:
   - Nombre
   - Apellido Paterno
   - Apellido Materno
   - Correo Electrónico
   - Contraseña
   - Rol
5. El administrador hace clic en "Guardar"
6. El sistema valida los datos
7. El sistema crea el usuario con Estatus = true
8. El sistema envía correo al nuevo usuario con sus credenciales
9. El sistema muestra mensaje de confirmación

**Flujos Alternativos:**
- **FA1: Datos Incompletos**
  - El sistema marca los campos requeridos
  - El administrador completa la información

**Postcondiciones:**
- Se crea un nuevo usuario activo
- El usuario recibe sus credenciales por correo

**Reglas de Negocio:**
- RN-006: Usuarios creados por admin son activos por defecto
- RN-007: El admin puede asignar cualquier rol

---

### CU-004: Editar Usuario
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado
- El usuario a editar debe existir

**Flujo Principal:**
1. El administrador accede al módulo de Usuarios
2. El administrador selecciona un usuario de la lista
3. El administrador hace clic en "Editar"
4. El sistema muestra formulario con datos actuales
5. El administrador modifica los campos necesarios
6. El administrador hace clic en "Guardar"
7. El sistema actualiza la información
8. El sistema muestra mensaje de confirmación

**Flujos Alternativos:**
- **FA1: Correo Duplicado**
  - Si el nuevo correo existe, el sistema muestra error

**Postcondiciones:**
- Se actualizan los datos del usuario

**Reglas de Negocio:**
- RN-008: El correo debe seguir siendo único

---

### CU-005: Activar/Desactivar Usuario
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado
- El usuario debe existir

**Flujo Principal:**
1. El administrador accede al módulo de Usuarios
2. El administrador localiza al usuario
3. El administrador cambia el estado (Activo/Inactivo)
4. El sistema actualiza el estatus
5. El sistema muestra mensaje de confirmación

**Postcondiciones:**
- Se modifica el estatus del usuario

**Reglas de Negocio:**
- RN-009: Usuarios inactivos no pueden iniciar sesión
- RN-010: No se puede desactivar el propio usuario

---

### CU-006: Cambiar Contraseña
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado

**Flujo Principal:**
1. El usuario accede a su perfil
2. El usuario hace clic en "Cambiar Contraseña"
3. El sistema muestra formulario
4. El usuario ingresa:
   - Contraseña actual
   - Nueva contraseña
   - Confirmar nueva contraseña
5. El usuario hace clic en "Actualizar"
6. El sistema valida la contraseña actual
7. El sistema valida que las nuevas contraseñas coincidan
8. El sistema actualiza la contraseña
9. El sistema muestra mensaje de confirmación

**Flujos Alternativos:**
- **FA1: Contraseña Actual Incorrecta**
  - El sistema muestra error
  - El usuario debe ingresar la contraseña correcta

- **FA2: Contraseñas Nuevas No Coinciden**
  - El sistema muestra error
  - El usuario debe corregir

**Postcondiciones:**
- Se actualiza la contraseña del usuario

---

## MÓDULO: BRIEFS

### CU-007: Crear Brief
**Actor Principal:** Usuario, Administrador
**Precondiciones:**
- El usuario debe estar autenticado
- Deben existir catálogos configurados (PCN, Audiencia, Formato, etc.)

**Flujo Principal:**
1. El usuario accede al módulo de Briefs
2. El usuario hace clic en "Nuevo Brief"
3. El sistema muestra formulario de creación
4. El usuario completa:
   - Nombre del proyecto
   - Ciclo
   - Marca
   - Links de referencias
   - Archivo adjunto (opcional)
5. El usuario agrega materiales:
   - Para cada material:
     - Nombre del material
     - Mensaje
     - PCNs (múltiples)
     - Formato
     - Audiencia
     - Prioridad
     - Responsable
     - Área
     - Fecha de entrega
6. El usuario hace clic en "Guardar"
7. El sistema valida los datos
8. El sistema crea el brief con todos sus materiales
9. El sistema asigna estatus inicial a cada material
10. El sistema muestra mensaje de confirmación

**Flujos Alternativos:**
- **FA1: Datos Incompletos**
  - El sistema marca los campos requeridos
  - El usuario completa la información

- **FA2: Fecha Inválida**
  - Si la fecha es anterior a hoy, el sistema muestra error
  - El usuario debe seleccionar fecha válida

**Postcondiciones:**
- Se crea un nuevo brief
- Se crean todos los materiales asociados
- Los materiales quedan en estatus inicial

**Reglas de Negocio:**
- RN-011: Fechas de entrega no pueden ser anteriores a hoy
- RN-012: Un brief debe tener al menos un material
- RN-013: Cada material puede tener múltiples PCNs

---

### CU-008: Ver Lista de Briefs
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado

**Flujo Principal:**
1. El usuario accede al módulo de Briefs
2. El sistema muestra lista de briefs según el rol:
   - Administrador: Ve todos los briefs
   - Usuario: Ve solo sus briefs
   - Producción: Ve todos los briefs
3. Para cada brief se muestra:
   - Nombre
   - Ciclo
   - Marca
   - Cantidad de materiales
   - Estado
4. El usuario puede filtrar por:
   - Nombre del proyecto
   - Ciclo
   - Marca
5. El usuario puede ordenar las columnas

**Postcondiciones:**
- Se visualiza la lista de briefs

**Reglas de Negocio:**
- RN-014: Usuarios solo ven sus propios briefs
- RN-015: Admin y Producción ven todos los briefs

---

### CU-009: Ver Detalle de Brief
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado
- El brief debe existir
- El usuario debe tener permisos para ver el brief

**Flujo Principal:**
1. El usuario hace clic en un brief de la lista
2. El sistema muestra la información completa:
   - Datos generales del brief
   - Links de referencias
   - Archivo adjunto (si existe)
   - Lista de materiales
   - Historial de cambios
3. El usuario puede visualizar el archivo adjunto
4. El usuario puede acceder a los links de referencias

**Postcondiciones:**
- Se visualiza el detalle del brief

---

### CU-010: Editar Brief
**Actor Principal:** Usuario (propietario), Administrador
**Precondiciones:**
- El usuario debe estar autenticado
- El brief debe existir
- El usuario debe ser el propietario o administrador

**Flujo Principal:**
1. El usuario accede al detalle del brief
2. El usuario hace clic en "Editar"
3. El sistema muestra formulario editable
4. El usuario modifica los campos necesarios
5. El usuario hace clic en "Guardar"
6. El sistema valida los cambios
7. El sistema actualiza el brief
8. El sistema muestra mensaje de confirmación

**Flujos Alternativos:**
- **FA1: Sin Permisos**
  - Si el usuario no es propietario ni admin, el sistema deniega la acción

**Postcondiciones:**
- Se actualiza la información del brief

**Reglas de Negocio:**
- RN-016: Solo el propietario o admin pueden editar

---

### CU-011: Eliminar Brief
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado
- El brief debe existir

**Flujo Principal:**
1. El administrador accede a la lista de briefs
2. El administrador selecciona un brief
3. El administrador hace clic en "Eliminar"
4. El sistema muestra mensaje de confirmación
5. El administrador confirma la eliminación
6. El sistema elimina el brief y todos sus materiales
7. El sistema muestra mensaje de confirmación

**Postcondiciones:**
- Se elimina el brief y sus materiales

**Reglas de Negocio:**
- RN-017: Solo administradores pueden eliminar briefs

---

### CU-012: Agregar Participante a Brief
**Actor Principal:** Usuario (propietario), Administrador
**Precondiciones:**
- El usuario debe estar autenticado
- El brief debe existir
- El participante debe ser un usuario activo

**Flujo Principal:**
1. El usuario accede al detalle del brief
2. El usuario hace clic en "Agregar Participante"
3. El sistema muestra campo de búsqueda
4. El usuario escribe el nombre del participante (mínimo 3 caracteres)
5. El sistema muestra resultados de búsqueda
6. El usuario selecciona un participante
7. El sistema agrega al participante
8. El sistema envía notificación por correo al participante
9. El sistema crea alerta para el participante
10. El sistema muestra el participante en la lista

**Flujos Alternativos:**
- **FA1: Participante Ya Existe**
  - Si el participante ya está agregado, el sistema muestra mensaje

**Postcondiciones:**
- Se agrega un participante al brief
- El participante recibe notificación

**Reglas de Negocio:**
- RN-018: Un participante no puede agregarse dos veces

---

### CU-013: Eliminar Participante de Brief
**Actor Principal:** Usuario (propietario), Administrador
**Precondiciones:**
- El usuario debe estar autenticado
- El participante debe existir en el brief

**Flujo Principal:**
1. El usuario accede a la lista de participantes
2. El usuario hace clic en eliminar junto al participante
3. El sistema remueve al participante
4. El sistema actualiza la lista

**Postcondiciones:**
- Se elimina el participante del brief

---

## MÓDULO: MATERIALES

### CU-014: Ver Lista de Materiales
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado

**Flujo Principal:**
1. El usuario accede al módulo de Materiales
2. El sistema muestra lista de materiales según el rol:
   - Administrador: Ve todos los materiales
   - Usuario: Ve solo materiales de sus briefs
   - Producción: Ve todos los materiales
3. Para cada material se muestra:
   - Nombre del material
   - Mensaje
   - PCNs
   - Formato
   - Estatus
   - Nombre del proyecto
   - Audiencia
   - Responsable
   - Área
   - Fecha de entrega
4. El sistema muestra contador por estatus:
   - En Revisión
   - En Producción
   - Falta Información
   - Aprobado
   - Programado
   - Entregado
5. El usuario puede filtrar por:
   - Nombre del material
   - Nombre del proyecto
   - Área
   - Responsable
   - Estatus
   - Rango de fechas

**Postcondiciones:**
- Se visualiza la lista de materiales con filtros aplicados

**Reglas de Negocio:**
- RN-019: Usuarios solo ven materiales de sus briefs
- RN-020: Admin y Producción ven todos los materiales

---

### CU-015: Editar Material (Actualizar Estatus y Comentarios)
**Actor Principal:** Producción, Administrador
**Precondiciones:**
- El usuario debe estar autenticado
- El material debe existir

**Flujo Principal:**
1. El usuario hace clic en "Editar" en un material
2. El sistema muestra modal con:
   - Nombre del material
   - Nombre del brief
   - Links de referencias
   - Archivo del brief
   - Fecha de entrega (editable)
   - Estatus actual (editable)
   - Editor de texto para comentarios
   - Opción de envío de correo
   - Lista de participantes
3. El usuario puede:
   - Modificar fecha de entrega
   - Cambiar estatus del material
   - Agregar comentario (con formato enriquecido)
   - Agregar participantes para notificar
   - Seleccionar si enviar correo
4. El usuario hace clic en "Agregar Comentario"
5. El sistema valida:
   - Fecha no sea anterior a hoy
   - Todos los campos requeridos estén completos
   - Si envía correo, debe haber participantes
6. El sistema crea entrada en historial
7. El sistema actualiza la fecha de entrega (si cambió)
8. El sistema actualiza el estatus (si cambió)
9. El sistema crea alertas:
   - Al usuario del brief: Sobre nuevo comentario
   - Al usuario del brief: Sobre cambio de estatus (si aplica)
   - Al usuario del brief: Si el material pasó a "Entregado" (si aplica)
10. Si se seleccionó envío de correo:
    - El sistema envía correo a participantes seleccionados
    - El sistema envía correo a usuarios de rol Producción
11. El sistema muestra el historial actualizado

**Flujos Alternativos:**
- **FA1: Usuario Sin Permisos Intenta Cambiar Estatus**
  - Si el usuario es RolId=2 (Usuario) intenta cambiar estatus
  - El sistema muestra error: "No tiene permisos para cambiar el estatus"
  - El usuario solo puede agregar comentarios

- **FA2: Fecha Inválida**
  - Si la fecha es anterior a hoy, el sistema muestra error
  - El usuario debe seleccionar fecha válida

- **FA3: Sin Participantes Con Envío de Correo**
  - Si selecciona enviar correo pero no hay participantes
  - El sistema muestra error: "Debe agregar al menos un participante"
  - El usuario debe agregar participantes

**Postcondiciones:**
- Se crea una entrada en la bitácora del material
- Se actualizan fecha y/o estatus del material
- Se crean alertas para el usuario del brief
- Se envían notificaciones por correo (si se seleccionó)

**Reglas de Negocio:**
- RN-021: Usuario (RolId=2) no puede cambiar estatus, solo comentar
- RN-022: Producción y Admin pueden cambiar estatus
- RN-023: Comentarios se guardan con formato HTML
- RN-024: Al cambiar a estatus "Entregado" se crea alerta especial
- RN-025: Siempre se crea alerta al usuario del brief sobre nuevos comentarios

---

### CU-016: Ver Historial de Material
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado
- El material debe existir
- El usuario debe tener permisos para ver el material

**Flujo Principal:**
1. El usuario abre el modal de edición del material
2. El sistema muestra el historial completo con:
   - Fecha y hora de cada entrada
   - Usuario que realizó la acción
   - Comentarios
   - Estatus en ese momento
   - Fecha de entrega en ese momento
3. Las entradas se muestran ordenadas cronológicamente (más reciente primero)

**Postcondiciones:**
- Se visualiza el historial completo del material

---

### CU-017: Agregar Participante en Material
**Actor Principal:** Producción, Administrador
**Precondiciones:**
- El usuario debe estar en el modal de edición de material
- El participante debe ser un usuario activo

**Flujo Principal:**
1. El usuario escribe en el campo de búsqueda (mínimo 3 caracteres)
2. El sistema muestra resultados de búsqueda
3. El usuario selecciona un participante
4. El sistema agrega al participante a la lista
5. El sistema envía notificación inmediata al participante:
   - Crea alerta: "Te agregaron como participante en el material X"
   - La alerta incluye link al material
6. El participante aparece en la lista de notificaciones

**Flujos Alternativos:**
- **FA1: Error en Notificación**
  - Si falla el envío de notificación, se registra en log
  - El participante se agrega de todas formas

**Postcondiciones:**
- El participante se agrega a la lista temporal
- El participante recibe notificación inmediata

**Reglas de Negocio:**
- RN-026: Participantes agregados reciben notificación automática

---

### CU-018: Eliminar Participante en Material
**Actor Principal:** Producción, Administrador
**Precondiciones:**
- El usuario debe estar en el modal de edición de material
- El participante debe estar en la lista

**Flujo Principal:**
1. El usuario hace clic en "Eliminar" junto al participante
2. El sistema remueve al participante de la lista
3. El sistema actualiza la vista

**Postcondiciones:**
- El participante se remueve de la lista temporal

---

### CU-019: Exportar Materiales a Excel
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado
- Debe haber materiales para exportar

**Flujo Principal:**
1. El usuario aplica filtros deseados en la lista de materiales
2. El usuario hace clic en "Exportar a Excel"
3. El sistema genera archivo Excel con:
   - Nombre de Material
   - Mensaje
   - PCN
   - Formato
   - Estatus
   - Nombre del Proyecto
   - Audiencia
   - Responsable
   - Área
   - Fecha de Entrega
4. El sistema descarga el archivo "MaterialesFiltrados.xlsx"

**Flujos Alternativos:**
- **FA1: Sin Datos**
  - Si no hay materiales para exportar, el sistema muestra mensaje
  - No se genera archivo

**Postcondiciones:**
- Se descarga archivo Excel con los datos filtrados

---

## MÓDULO: CALENDARIO

### CU-020: Ver Calendario de Entregas
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado

**Flujo Principal:**
1. El usuario accede al módulo de Calendario
2. El sistema muestra calendario mensual
3. El sistema marca las fechas con entregas programadas
4. Para cada fecha se muestran los materiales con entrega ese día
5. El usuario puede:
   - Navegar entre meses
   - Ver detalle de materiales
   - Acceder al material desde el calendario

**Postcondiciones:**
- Se visualiza el calendario con las entregas

**Reglas de Negocio:**
- RN-027: Solo se muestran materiales según permisos del usuario

---

## MÓDULO: ALERTAS Y NOTIFICACIONES

### CU-021: Ver Alertas
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado

**Flujo Principal:**
1. El usuario hace clic en el ícono de alertas
2. El sistema muestra lista de alertas del usuario:
   - Nuevos comentarios en materiales
   - Cambios de estatus
   - Materiales entregados
   - Solicitudes de usuarios (solo admin)
   - Participación en briefs/materiales
3. Para cada alerta se muestra:
   - Tipo de alerta
   - Descripción
   - Fecha
   - Estado (leída/no leída)
   - Acción (link al recurso)
4. El sistema muestra contador de alertas no leídas

**Postcondiciones:**
- Se visualiza la lista de alertas

---

### CU-022: Marcar Alerta como Leída
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado
- La alerta debe existir

**Flujo Principal:**
1. El usuario hace clic en una alerta
2. El sistema marca la alerta como leída
3. El sistema redirige al recurso asociado
4. El sistema actualiza el contador de alertas no leídas

**Postcondiciones:**
- La alerta se marca como leída
- Se decrementa el contador

---

### CU-023: Eliminar Alerta
**Actor Principal:** Todos los usuarios
**Precondiciones:**
- El usuario debe estar autenticado
- La alerta debe existir

**Flujo Principal:**
1. El usuario hace clic en eliminar en una alerta
2. El sistema elimina la alerta
3. El sistema actualiza la lista

**Postcondiciones:**
- Se elimina la alerta

---

## MÓDULO: INVITACIONES (SOLICITUDES DE USUARIOS)

### CU-024: Ver Solicitudes de Usuarios
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado

**Flujo Principal:**
1. El administrador accede al módulo de Invitaciones
2. El sistema muestra lista de solicitudes pendientes
3. Para cada solicitud se muestra:
   - Nombre
   - Correo
   - Fecha de solicitud
   - Estado
4. El administrador puede aprobar o rechazar solicitudes

**Postcondiciones:**
- Se visualiza la lista de solicitudes

**Reglas de Negocio:**
- RN-028: Solo administradores pueden ver solicitudes

---

### CU-025: Aprobar Solicitud de Usuario
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado
- La solicitud debe existir y estar pendiente

**Flujo Principal:**
1. El administrador revisa la solicitud
2. El administrador hace clic en "Aprobar"
3. El sistema actualiza el estado de la solicitud
4. El sistema envía correo de bienvenida al usuario
5. El usuario puede ahora iniciar sesión

**Postcondiciones:**
- La solicitud se marca como aprobada
- El usuario recibe confirmación por correo

---

### CU-026: Rechazar Solicitud de Usuario
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado
- La solicitud debe existir y estar pendiente

**Flujo Principal:**
1. El administrador revisa la solicitud
2. El administrador hace clic en "Rechazar"
3. El sistema muestra diálogo de confirmación
4. El administrador confirma el rechazo
5. El sistema marca la solicitud como rechazada
6. El sistema puede enviar correo al solicitante (opcional)

**Postcondiciones:**
- La solicitud se marca como rechazada

---

## MÓDULO: CATÁLOGOS

### CU-027: Gestionar Catálogo de PCN
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado

**Flujo Principal:**
1. El administrador accede a Catálogos > PCN
2. El sistema muestra lista de PCNs existentes
3. El administrador puede:
   - Agregar nuevo PCN
   - Editar PCN existente
   - Eliminar PCN
4. Para cada operación, el sistema valida y confirma

**Postcondiciones:**
- Se actualiza el catálogo de PCN

**Reglas de Negocio:**
- RN-029: No se pueden eliminar PCNs en uso

---

### CU-028: Gestionar Catálogo de Formatos
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado

**Flujo Principal:**
1. El administrador accede a Catálogos > Formatos
2. El sistema muestra lista de formatos existentes
3. El administrador puede:
   - Agregar nuevo formato
   - Editar formato existente
   - Eliminar formato
4. Para cada operación, el sistema valida y confirma

**Postcondiciones:**
- Se actualiza el catálogo de formatos

---

### CU-029: Gestionar Catálogo de Audiencias
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado

**Flujo Principal:**
1. El administrador accede a Catálogos > Audiencias
2. El sistema muestra lista de audiencias existentes
3. El administrador puede:
   - Agregar nueva audiencia
   - Editar audiencia existente
   - Eliminar audiencia
4. Para cada operación, el sistema valida y confirma

**Postcondiciones:**
- Se actualiza el catálogo de audiencias

---

### CU-030: Gestionar Catálogo de Prioridades
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado

**Flujo Principal:**
1. El administrador accede a Catálogos > Prioridades
2. El sistema muestra lista de prioridades existentes
3. El administrador puede:
   - Agregar nueva prioridad
   - Editar prioridad existente
   - Eliminar prioridad
4. Para cada operación, el sistema valida y confirma

**Postcondiciones:**
- Se actualiza el catálogo de prioridades

---

### CU-031: Gestionar Catálogo de Estatus de Materiales
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado

**Flujo Principal:**
1. El administrador accede a Catálogos > Estatus de Materiales
2. El sistema muestra lista de estatus existentes
3. El administrador puede:
   - Agregar nuevo estatus
   - Editar estatus existente
   - Eliminar estatus
4. Para cada operación, el sistema valida y confirma

**Postcondiciones:**
- Se actualiza el catálogo de estatus de materiales

---

## MÓDULO: CORREOS

### CU-032: Configurar Plantillas de Correo
**Actor Principal:** Administrador
**Precondiciones:**
- El administrador debe estar autenticado

**Flujo Principal:**
1. El administrador accede al módulo de Correos
2. El sistema muestra lista de plantillas:
   - RegistroUsuario
   - RegistroUsuarioAdmin
   - RegistroParticipante
   - ComentarioMaterial
   - NuevoBrief
3. El administrador selecciona una plantilla
4. El administrador edita el contenido HTML
5. El administrador puede usar variables dinámicas
6. El administrador hace clic en "Guardar"
7. El sistema actualiza la plantilla

**Postcondiciones:**
- Se actualiza la plantilla de correo

---

## RESUMEN DE REGLAS DE NEGOCIO

| ID | Descripción |
|----|-------------|
| RN-001 | Las contraseñas deben estar encriptadas |
| RN-002 | Usuarios inactivos no pueden acceder al sistema |
| RN-003 | Usuarios nuevos se crean activos por defecto |
| RN-004 | Rol por defecto para auto-registro es Usuario (RolId=2) |
| RN-005 | El correo electrónico debe ser único en el sistema |
| RN-006 | Usuarios creados por administrador son activos por defecto |
| RN-007 | El administrador puede asignar cualquier rol al crear usuario |
| RN-008 | Al editar, el correo debe seguir siendo único |
| RN-009 | Usuarios inactivos no pueden iniciar sesión |
| RN-010 | Un administrador no puede desactivar su propio usuario |
| RN-011 | Fechas de entrega no pueden ser anteriores a la fecha actual |
| RN-012 | Un brief debe tener al menos un material |
| RN-013 | Cada material puede tener múltiples PCNs asociados |
| RN-014 | Usuarios (RolId=2) solo ven sus propios briefs |
| RN-015 | Administradores y Producción ven todos los briefs |
| RN-016 | Solo el propietario o administrador pueden editar un brief |
| RN-017 | Solo administradores pueden eliminar briefs |
| RN-018 | Un participante no puede agregarse dos veces al mismo brief |
| RN-019 | Usuarios (RolId=2) solo ven materiales de sus briefs |
| RN-020 | Administradores y Producción ven todos los materiales |
| RN-021 | Usuarios (RolId=2) no pueden cambiar estatus, solo comentar |
| RN-022 | Producción y Administrador pueden cambiar estatus de materiales |
| RN-023 | Comentarios se guardan con formato HTML enriquecido |
| RN-024 | Al cambiar a estatus "Entregado" (Id=5) se crea alerta especial |
| RN-025 | Siempre se crea alerta al usuario del brief sobre nuevos comentarios |
| RN-026 | Participantes agregados a materiales reciben notificación automática |
| RN-027 | El calendario solo muestra materiales según permisos del usuario |
| RN-028 | Solo administradores pueden ver y gestionar solicitudes de usuarios |
| RN-029 | No se pueden eliminar elementos de catálogo que estén en uso |

---

## TIPOS DE ALERTAS

| ID | Tipo de Alerta | Descripción |
|----|----------------|-------------|
| 1 | Información | Alertas informativas generales |
| 2 | Solicitud Usuario Nuevo | Nueva solicitud de registro pendiente de aprobación |
| 3 | Nuevo Comentario | Nuevo comentario agregado en material |
| 4 | Cambio de Estatus | El estatus de un material ha cambiado |
| 5 | Material Entregado | Un material ha sido marcado como entregado |

---

## MATRIZ DE PERMISOS POR ROL

| Funcionalidad | Administrador | Usuario | Producción |
|---------------|---------------|---------|------------|
| **Autenticación** |
| Iniciar Sesión | ✓ | ✓ | ✓ |
| Registrarse | ✓ | ✓ | ✓ |
| Cambiar Contraseña | ✓ | ✓ | ✓ |
| **Usuarios** |
| Ver Todos los Usuarios | ✓ | ✗ | ✗ |
| Crear Usuario | ✓ | ✗ | ✗ |
| Editar Usuario | ✓ | ✗ | ✗ |
| Activar/Desactivar Usuario | ✓ | ✗ | ✗ |
| **Briefs** |
| Ver Todos los Briefs | ✓ | ✗ | ✓ |
| Ver Mis Briefs | ✓ | ✓ | ✓ |
| Crear Brief | ✓ | ✓ | ✗ |
| Editar Brief Propio | ✓ | ✓ | ✗ |
| Editar Cualquier Brief | ✓ | ✗ | ✗ |
| Eliminar Brief | ✓ | ✗ | ✗ |
| Agregar Participantes | ✓ | ✓ | ✗ |
| **Materiales** |
| Ver Todos los Materiales | ✓ | ✗ | ✓ |
| Ver Materiales de Mis Briefs | ✓ | ✓ | ✓ |
| Cambiar Estatus | ✓ | ✗ | ✓ |
| Agregar Comentarios | ✓ | ✓ | ✓ |
| Modificar Fecha Entrega | ✓ | ✗ | ✓ |
| Exportar a Excel | ✓ | ✓ | ✓ |
| **Calendario** |
| Ver Calendario | ✓ | ✓ | ✓ |
| **Alertas** |
| Ver Alertas Propias | ✓ | ✓ | ✓ |
| Marcar como Leída | ✓ | ✓ | ✓ |
| Eliminar Alerta | ✓ | ✓ | ✓ |
| **Invitaciones** |
| Ver Solicitudes | ✓ | ✗ | ✗ |
| Aprobar/Rechazar | ✓ | ✗ | ✗ |
| **Catálogos** |
| Gestionar PCN | ✓ | ✗ | ✗ |
| Gestionar Formatos | ✓ | ✗ | ✗ |
| Gestionar Audiencias | ✓ | ✗ | ✗ |
| Gestionar Prioridades | ✓ | ✗ | ✗ |
| Gestionar Estatus | ✓ | ✗ | ✗ |
| **Correos** |
| Configurar Plantillas | ✓ | ✗ | ✗ |

---

**FIN DEL DOCUMENTO DE CASOS DE USO**
