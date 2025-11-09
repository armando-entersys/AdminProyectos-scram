# DOCUMENTACIÓN DEL SISTEMA DE ADMINISTRACIÓN DE PROYECTOS NATURA

**Versión:** 1.0
**Fecha:** 2025-01-09
**Cliente:** Natura
**Empresa:** Entersys
**URL Sistema:** https://adminproyectos.entersys.mx

---

## INTRODUCCIÓN

Este directorio contiene la documentación técnica completa del Sistema de Administración de Proyectos de Natura, una plataforma web diseñada para gestionar briefs, materiales creativos, y flujo de trabajo entre solicitantes y el equipo de producción.

---

## CONTENIDO DE LA DOCUMENTACIÓN

### 01. Casos de Uso
**Archivo:** `01-Casos-de-Uso.md`

Documento profesional que describe:
- 32 casos de uso detallados organizados por módulo
- Actores del sistema (Administrador, Usuario, Producción)
- Flujos principales y alternativos
- Precondiciones y postcondiciones
- 29 reglas de negocio documentadas
- Matriz de permisos por rol
- Tipos de alertas del sistema

**Módulos Documentados:**
- Autenticación y Usuarios
- Gestión de Briefs
- Gestión de Materiales
- Calendario de Entregas
- Alertas y Notificaciones
- Invitaciones (Solicitudes de Usuarios)
- Catálogos del Sistema
- Configuración de Correos

---

### 02. Base de Datos
**Archivo:** `02-Base-de-Datos.md`

Documentación técnica de la base de datos que incluye:
- Diagrama Entidad-Relación completo
- Descripción detallada de 17 tablas
- Diccionario de datos con tipos, constraints e índices
- Relaciones y llaves foráneas
- Catálogos del sistema
- Consultas SQL comunes
- Scripts de inicialización
- Estrategia de backup y mantenimiento
- Políticas de seguridad

**Tablas Principales:**
- Usuarios, Roles
- Briefs, Materiales
- HistorialMaterial
- MaterialPCN (Relación N:N)
- Participantes
- Alertas, TipoAlerta
- Catálogos: PCN, Formato, Audiencia, Prioridad, EstatusMateriales

---

### 03. Matrices de Pruebas por Rol
**Archivo:** `03-Matrices-de-Pruebas-por-Rol.md`

Plan de pruebas exhaustivo con:
- **153 casos de prueba** organizados por rol
- Usuarios de prueba con credenciales
- Priorización (P1-Crítica, P2-Alta, P3-Media, P4-Baja)
- Estados de prueba (PASS, FAIL, BLOCKED, PENDING, RETEST)

**Desglose por Rol:**

#### Administrador (75 pruebas)
- Autenticación: 5 casos
- Gestión de Usuarios: 11 casos
- Gestión de Briefs: 10 casos
- Gestión de Materiales: 22 casos
- Invitaciones: 4 casos
- Calendario: 3 casos
- Alertas: 4 casos
- Catálogos: 9 casos (CRUD completo)
- Correos: 3 casos

#### Usuario (43 pruebas)
- Autenticación: 5 casos (incluyendo auto-registro)
- Gestión de Briefs: 10 casos
- Gestión de Materiales: 15 casos (solo comentar, no cambiar estatus)
- Calendario: 3 casos (solo sus materiales)
- Alertas: 4 casos
- Seguridad: 6 casos (intentos de acceso no autorizado)

#### Producción (35 pruebas)
- Autenticación: 3 casos
- Gestión de Briefs: 5 casos (solo ver, no editar)
- Gestión de Materiales: 15 casos (cambiar estatus, modificar fechas)
- Calendario: 3 casos (ver todo)
- Alertas: 4 casos
- Seguridad: 5 casos

**Pruebas de Integración:**
- 10 escenarios end-to-end
- Ciclo completo: Solicitud → Producción → Entrega
- Múltiples participantes
- Materiales con múltiples PCNs
- Cambio de roles
- Eliminación en cascada
- Validaciones y concurrencia

**Criterios de Aceptación:**
- Funcionalidad
- Permisos y Seguridad
- Integridad de Datos
- Notificaciones
- Experiencia de Usuario

---

## ARQUITECTURA DEL SISTEMA

### Stack Tecnológico

**Backend:**
- ASP.NET Core 6.0 MVC
- Entity Framework Core 6.0
- SQL Server (Docker)
- C# 10

**Frontend:**
- Knockout.js (MVVM)
- Bootstrap 5
- TinyMCE (Editor Rico)
- jQuery
- SheetJS (Exportación Excel)

**Infraestructura:**
- Docker / Docker Compose
- Google Cloud Platform (GCP)
- Nginx (Reverse Proxy)
- SSL/TLS

---

## INFORMACIÓN DEL SISTEMA

### Base de Datos
- **Motor:** Microsoft SQL Server
- **Nombre:** AdminProyectosNaturaDB
- **Servidor:** adminproyectos-sqlserver (Docker)
- **Puerto:** 1433
- **Usuario:** sa
- **Contraseña:** Operaciones.2025

### Roles del Sistema
1. **Administrador (RolId: 1)** - Permisos completos
2. **Usuario (RolId: 2)** - Solicita y gestiona briefs propios
3. **Producción (RolId: 3)** - Ejecuta materiales, cambia estatus

### Usuarios Activos
Actualmente el sistema tiene:
- **20 usuarios** registrados y activos
- **24 briefs** en el sistema
- **3 administradores**
- **10 usuarios**
- **7 usuarios de producción**

---

## REGLAS DE NEGOCIO PRINCIPALES

| ID | Regla |
|----|-------|
| RN-003 | Usuarios nuevos se crean activos por defecto |
| RN-011 | Fechas de entrega no pueden ser anteriores a la fecha actual |
| RN-012 | Un brief debe tener al menos un material |
| RN-013 | Cada material puede tener múltiples PCNs asociados |
| RN-014 | Usuarios (RolId=2) solo ven sus propios briefs |
| RN-015 | Administradores y Producción ven todos los briefs |
| RN-021 | Usuarios (RolId=2) no pueden cambiar estatus, solo comentar |
| RN-022 | Producción y Administrador pueden cambiar estatus de materiales |
| RN-024 | Al cambiar a estatus "Entregado" (Id=5) se crea alerta especial |
| RN-025 | Siempre se crea alerta al usuario del brief sobre nuevos comentarios |

Consultar documento completo de Casos de Uso para las 29 reglas documentadas.

---

## FLUJOS PRINCIPALES

### 1. Flujo de Solicitud de Material

```
Usuario
  ↓
Crear Brief + Materiales
  ↓
Sistema guarda y notifica Producción
  ↓
Producción recibe alerta
  ↓
Producción cambia estatus a "En Producción"
  ↓
Usuario recibe alerta de cambio
  ↓
Producción sube avances con imágenes
  ↓
Producción marca como "Entregado"
  ↓
Usuario recibe 3 alertas:
  - Nuevo Comentario
  - Cambio de Estatus
  - Material Entregado
```

### 2. Flujo de Registro de Usuario

```
Nuevo Usuario
  ↓
Completar formulario de registro
  ↓
Sistema crea usuario con Estatus=true
  ↓
Usuario recibe correo de confirmación
  ↓
Administrador recibe alerta + correo
  ↓
(Opcional) Admin aprueba en /Invitaciones
  ↓
Usuario puede iniciar sesión
```

### 3. Flujo de Notificaciones

```
Evento (comentario, cambio estatus, entrega)
  ↓
Sistema crea entrada en HistorialMaterial
  ↓
Sistema actualiza Material (si cambió estatus/fecha)
  ↓
Sistema crea Alertas para:
  - Usuario del brief
  - Participantes (si aplica)
  ↓
Si se seleccionó "Enviar Correo":
  - Envía a participantes seleccionados
  - Envía a todos los usuarios Producción (RolId=3)
  ↓
Destinatarios reciben notificación en tiempo real
```

---

## CARACTERÍSTICAS DESTACADAS

### Gestión de Materiales
- ✅ Sistema de bitácora completo (HistorialMaterial)
- ✅ Soporte para múltiples PCNs por material (relación N:N)
- ✅ Editor de texto enriquecido con TinyMCE
- ✅ Upload de imágenes en comentarios
- ✅ 6 estatus de seguimiento
- ✅ Alertas automáticas al usuario del brief
- ✅ Exportación a Excel con filtros

### Sistema de Alertas
- ✅ 5 tipos de alertas diferentes
- ✅ Contador de alertas no leídas
- ✅ Enlaces directos al recurso
- ✅ Notificaciones por correo electrónico
- ✅ Templates personalizables

### Seguridad
- ✅ Autenticación basada en Claims
- ✅ Autorización por roles
- ✅ Contraseñas encriptadas
- ✅ Validación de permisos en cada acción
- ✅ Protección contra accesos no autorizados

### Gestión de Permisos
- ✅ 3 roles claramente definidos
- ✅ Cada rol solo ve sus recursos permitidos
- ✅ Usuario: solo briefs propios
- ✅ Producción: todos los briefs, puede cambiar estatus
- ✅ Admin: acceso completo al sistema

---

## PUNTOS IMPORTANTES PARA PRUEBAS

### Validaciones Críticas
1. **Fechas:** No permitir fechas pasadas
2. **Estatus:** Usuario NO puede cambiar estatus (RN-021)
3. **Visibilidad:** Usuario solo ve sus briefs/materiales (RN-014, RN-019)
4. **PCNs Múltiples:** Verificar tabla MaterialPCN (RN-013)
5. **Alertas:** 3 alertas al entregar material (RN-024, RN-025)

### Escenarios de Seguridad
1. Usuario intenta acceder a brief ajeno por URL
2. Usuario intenta cambiar estatus (debe fallar)
3. Usuario intenta acceder a /Catalogos (debe fallar)
4. Producción intenta crear/editar brief (debe fallar)
5. Producción intenta acceder a /Usuarios (debe fallar)

### Integridad de Datos
1. Eliminar brief elimina materiales en cascada
2. No se puede eliminar PCN en uso
3. Correo debe ser único
4. Un brief debe tener al menos un material

---

## CONTACTO Y SOPORTE

Para preguntas sobre la documentación o el sistema:

**Equipo de Desarrollo:**
- Entersys
- Email: desarrollo@entersys.mx

**Cliente:**
- Natura
- Contacto: [Especificar contacto del cliente]

---

## CONTROL DE VERSIONES

| Versión | Fecha | Autor | Cambios |
|---------|-------|-------|---------|
| 1.0 | 2025-01-09 | Claude Code (Entersys) | Documentación inicial completa |

---

## ARCHIVOS ADICIONALES

En este directorio también encontrarás:
- Scripts de prueba (test-*.js)
- Capturas de pantalla (*.png)
- Diagramas (si se generan posteriormente)

---

**© 2025 Entersys - Documentación Sistema Admin Proyectos Natura**
