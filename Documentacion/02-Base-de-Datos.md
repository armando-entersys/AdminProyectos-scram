# SISTEMA DE ADMINISTRACIÓN DE PROYECTOS NATURA
## DOCUMENTACIÓN DE BASE DE DATOS

**Versión:** 1.0
**Fecha:** 2025-01-09
**Cliente:** Natura
**Empresa:** Entersys
**Motor de Base de Datos:** Microsoft SQL Server
**Nombre de Base de Datos:** AdminProyectosNaturaDB

---

## TABLA DE CONTENIDOS

1. [Diagrama Entidad-Relación](#diagrama-entidad-relación)
2. [Descripción de Tablas](#descripción-de-tablas)
3. [Diccionario de Datos](#diccionario-de-datos)
4. [Relaciones y Llaves Foráneas](#relaciones-y-llaves-foráneas)
5. [Índices y Constraints](#índices-y-constraints)
6. [Catálogos del Sistema](#catálogos-del-sistema)

---

## ARQUITECTURA DE BASE DE DATOS

### Tecnologías
- **Motor:** Microsoft SQL Server (compatible con versión 2019+)
- **ORM:** Entity Framework Core 6.0
- **Patrón:** Code First con Fluent API
- **Servidor:** Docker Container (adminproyectos-sqlserver)
- **Puerto:** 1433

### Cadena de Conexión
```
Server=adminproyectos-sqlserver;Database=AdminProyectosNaturaDB;User Id=sa;Password=Operaciones.2025;TrustServerCertificate=True;
```

---

## DIAGRAMA ENTIDAD-RELACIÓN

```
┌─────────────┐       ┌──────────────┐       ┌─────────────┐
│   Usuarios  │───┐   │    Briefs    │───┬───│  Materiales │
└─────────────┘   │   └──────────────┘   │   └─────────────┘
      │           │          │            │          │
      │           │          │            │          ├──────┐
      │           │          │            │          │      │
      ↓           ↓          ↓            ↓          ↓      ↓
┌─────────────┐  ┌────────────────┐  ┌─────────────────────────┐
│    Roles    │  │ Participantes  │  │ HistorialMaterial       │
└─────────────┘  └────────────────┘  └─────────────────────────┘
                        │                     │
                        │                     │
┌──────────────┐       │                     │
│   Alertas    │───────┘                     │
└──────────────┘                             │
      │                                       │
      ↓                                       ↓
┌──────────────┐                    ┌─────────────────┐
│ TipoAlerta   │                    │ EstatusMateriales│
└──────────────┘                    └─────────────────┘

┌─────────────┐       ┌──────────────┐
│ MaterialPCN │───────│     PCN      │
└─────────────┘       └──────────────┘
      │
      └───────────────┐
                      │
                      ↓
              ┌─────────────┐
              │  Materiales │
              └─────────────┘

Catálogos:
┌────────────┐  ┌───────────┐  ┌──────────┐  ┌───────────┐
│  Formato   │  │ Audiencia │  │ Prioridad│  │  TipoBrief│
└────────────┘  └───────────┘  └──────────┘  └───────────┘
```

---

## DESCRIPCIÓN DE TABLAS

### Tablas Principales

#### 1. Usuarios
**Propósito:** Almacena la información de todos los usuarios del sistema.

**Características:**
- Tabla principal de autenticación
- Contraseñas almacenadas con hash
- Incluye campo de estatus para activación/desactivación
- Soporta auto-registro con aprobación

#### 2. Briefs
**Propósito:** Almacena los proyectos solicitados por los usuarios.

**Características:**
- Cada brief pertenece a un usuario (UsuarioId)
- Puede contener múltiples materiales
- Incluye información de ciclo, marca, referencias
- Permite adjuntar archivo de especificaciones

#### 3. Materiales
**Propósito:** Almacena los materiales creativos solicitados en cada brief.

**Características:**
- Pertenece a un brief específico (BriefId)
- Tiene estatus de seguimiento
- Incluye fecha de entrega
- Puede tener múltiples PCNs asociados

#### 4. HistorialMaterial
**Propósito:** Bitácora de cambios y comentarios de cada material.

**Características:**
- Registra cada actualización del material
- Almacena comentarios con formato HTML
- Guarda usuario que realizó el cambio
- Registra fecha de entrega y estatus en ese momento

#### 5. MaterialPCN
**Propósito:** Tabla intermedia para relación muchos a muchos entre Material y PCN.

**Características:**
- Permite múltiples PCNs por material
- Llave primaria compuesta (MaterialId, PCNId)

---

## DICCIONARIO DE DATOS

### Tabla: Usuarios

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único del usuario |
| Nombre | nvarchar(MAX) | NO | Nombre(s) del usuario |
| ApellidoPaterno | nvarchar(MAX) | NO | Apellido paterno |
| ApellidoMaterno | nvarchar(MAX) | YES | Apellido materno (opcional) |
| Correo | nvarchar(MAX) | NO | Correo electrónico (único) |
| Contrasena | nvarchar(MAX) | NO | Contraseña encriptada |
| ConfirmarContrasena | nvarchar(MAX) | YES | Confirmación de contraseña (solo UI) |
| RolId | int | NO | FK a Roles, define el rol del usuario |
| Estatus | bit | NO | true=Activo, false=Inactivo |
| CambioContrasena | bit | YES | Indica si debe cambiar contraseña |
| SolicitudRegistro | bit | YES | true si es registro pendiente de aprobación |
| FechaRegistro | datetime2 | YES | Fecha de registro en el sistema |
| FechaModificacion | datetime2 | YES | Última modificación del registro |

**Índices:**
- PK_Usuarios (Id)
- IX_Usuarios_RolId (RolId)

**Constraints:**
- FK_Usuarios_Roles (RolId → Roles.Id) ON DELETE CASCADE

---

### Tabla: Roles

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único del rol |
| Descripcion | nvarchar(MAX) | NO | Nombre del rol |

**Índices:**
- PK_Roles (Id)

**Valores Predefinidos:**
```sql
INSERT INTO Roles (Id, Descripcion) VALUES
(1, 'Administrador'),
(2, 'Usuario'),
(3, 'Producción');
```

---

### Tabla: Briefs

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único del brief |
| Nombre | nvarchar(MAX) | NO | Nombre del proyecto |
| Ciclo | nvarchar(MAX) | YES | Ciclo de campaña (ej: "C1 2025") |
| Marca | nvarchar(MAX) | YES | Marca asociada al proyecto |
| LinksReferencias | nvarchar(MAX) | YES | URLs de referencias separadas por comas |
| RutaArchivo | nvarchar(MAX) | YES | Ruta del archivo adjunto |
| NombreArchivo | nvarchar(MAX) | YES | Nombre original del archivo |
| UsuarioId | int | NO | FK a Usuarios, propietario del brief |
| FechaCreacion | datetime2 | YES | Fecha de creación |
| FechaModificacion | datetime2 | YES | Última modificación |

**Índices:**
- PK_Briefs (Id)
- IX_Briefs_UsuarioId (UsuarioId)

**Constraints:**
- FK_Briefs_Usuarios (UsuarioId → Usuarios.Id) ON DELETE CASCADE

---

### Tabla: Materiales

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único del material |
| Nombre | nvarchar(MAX) | NO | Nombre del material |
| Mensaje | nvarchar(MAX) | NO | Mensaje o copy del material |
| PrioridadId | int | NO | FK a Prioridad |
| Ciclo | nvarchar(MAX) | NO | Ciclo de entrega |
| InicioCiclo | bit | YES | Indica si es inicio de ciclo |
| NoCompartio | bit | YES | Indica si no compartió algo |
| AudienciaId | int | NO | FK a Audiencia |
| FormatoId | int | NO | FK a Formato |
| Responsable | nvarchar(MAX) | NO | Nombre del responsable |
| Area | nvarchar(MAX) | NO | Área responsable |
| FechaEntrega | datetime2 | NO | Fecha comprometida de entrega |
| BriefId | int | NO | FK a Briefs |
| EstatusMaterialId | int | NO | FK a EstatusMateriales |

**Índices:**
- PK_Materiales (Id)
- IX_Materiales_AudienciaId (AudienciaId)
- IX_Materiales_BriefId (BriefId)
- IX_Materiales_EstatusMaterialId (EstatusMaterialId)
- IX_Materiales_FormatoId (FormatoId)
- IX_Materiales_PrioridadId (PrioridadId)

**Constraints:**
- FK_Materiales_Audiencia ON DELETE CASCADE
- FK_Materiales_Briefs ON DELETE CASCADE
- FK_Materiales_EstatusMateriales ON DELETE CASCADE
- FK_Materiales_Formato ON DELETE CASCADE
- FK_Materiales_Prioridad ON DELETE CASCADE

---

### Tabla: MaterialPCN

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| MaterialId | int | NO | PK, FK a Materiales |
| PCNId | int | NO | PK, FK a PCN |

**Índices:**
- PK_MaterialPCN (MaterialId, PCNId)
- IX_MaterialPCN_PCNId (PCNId)

**Constraints:**
- FK_MaterialPCN_Materiales (MaterialId → Materiales.Id) ON DELETE CASCADE
- FK_MaterialPCN_PCN (PCNId → PCN.Id) ON DELETE CASCADE

**Notas:**
- Llave primaria compuesta
- Permite múltiples PCNs por material

---

### Tabla: HistorialMaterial

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único del registro |
| Comentarios | nvarchar(MAX) | NO | Comentarios en formato HTML |
| FechaCreacion | datetime2 | YES | Fecha de creación del comentario |
| FechaEntrega | datetime2 | YES | Fecha de entrega en ese momento |
| UsuarioId | int | NO | FK a Usuarios, quien hizo el comentario |
| MaterialId | int | NO | FK a Materiales |
| EstatusMaterialId | int | YES | FK a EstatusMateriales, estatus en ese momento |

**Índices:**
- PK_HistorialMaterial (Id)
- IX_HistorialMaterial_EstatusMaterialId (EstatusMaterialId)
- IX_HistorialMaterial_MaterialId (MaterialId)
- IX_HistorialMaterial_UsuarioId (UsuarioId)

**Constraints:**
- FK_HistorialMaterial_EstatusMateriales ON DELETE NO ACTION
- FK_HistorialMaterial_Materiales ON DELETE NO ACTION
- FK_HistorialMaterial_Usuarios ON DELETE NO ACTION

---

### Tabla: Participantes

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único |
| UsuarioId | int | NO | FK a Usuarios, participante |
| BriefId | int | NO | FK a Briefs |

**Índices:**
- PK_Participantes (Id)
- IX_Participantes_BriefId (BriefId)
- IX_Participantes_UsuarioId (UsuarioId)

**Constraints:**
- FK_Participantes_Briefs (BriefId → Briefs.Id) ON DELETE CASCADE
- FK_Participantes_Usuarios (UsuarioId → Usuarios.Id) ON DELETE NO ACTION

**Notas:**
- Evita cascada en Usuarios para prevenir eliminación accidental

---

### Tabla: Alertas

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único de la alerta |
| IdUsuario | int | NO | FK a Usuarios, destinatario |
| Nombre | nvarchar(MAX) | NO | Título de la alerta |
| Descripcion | nvarchar(MAX) | NO | Descripción detallada |
| FechaCreacion | datetime2 | YES | Fecha de creación |
| Accion | nvarchar(MAX) | YES | URL de acción/redirección |
| IdTipoAlerta | int | NO | FK a TipoAlerta |
| lectura | bit | NO | true=leída, false=no leída |

**Índices:**
- PK_Alertas (Id)
- IX_Alertas_IdTipoAlerta (IdTipoAlerta)
- IX_Alertas_IdUsuario (IdUsuario)

**Constraints:**
- FK_Alertas_TipoAlerta (IdTipoAlerta → TipoAlerta.Id) ON DELETE CASCADE
- FK_Alertas_Usuarios (IdUsuario → Usuarios.Id) ON DELETE CASCADE

---

### Tabla: TipoAlerta

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único |
| Descripcion | nvarchar(MAX) | NO | Descripción del tipo |

**Índices:**
- PK_TipoAlerta (Id)

**Valores Predefinidos:**
```sql
INSERT INTO TipoAlerta (Id, Descripcion) VALUES
(1, 'Información'),
(2, 'Solicitud Usuario Nuevo'),
(3, 'Nuevo Comentario'),
(4, 'Cambio de Estatus'),
(5, 'Material Entregado');
```

---

### Tabla: Menu

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity, ID único |
| Nombre | nvarchar(MAX) | NO | Nombre del menú |
| Ruta | nvarchar(MAX) | NO | URL del menú |
| Orden | int | YES | Orden de visualización |
| Icono | nvarchar(MAX) | NO | Clase CSS del ícono |
| RolId | int | NO | FK a Roles |

**Índices:**
- PK_Menu (Id)
- IX_Menu_RolId (RolId)

**Constraints:**
- FK_Menu_Roles (RolId → Roles.Id) ON DELETE CASCADE

---

## TABLAS DE CATÁLOGOS

### Tabla: PCN (Product Classification Number)

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity |
| Descripcion | nvarchar(MAX) | NO | Descripción del PCN |

**Índices:**
- PK_PCN (Id)

**Ejemplos de Datos:**
```sql
INSERT INTO PCN (Descripcion) VALUES
('Natura'),
('Ekos'),
('Chronos'),
('Plant'),
('Kaiak');
```

---

### Tabla: Formato

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity |
| Descripcion | nvarchar(MAX) | NO | Descripción del formato |

**Índices:**
- PK_Formato (Id)

**Ejemplos de Datos:**
```sql
INSERT INTO Formato (Descripcion) VALUES
('Imagen Estática'),
('Video'),
('GIF Animado'),
('Banner'),
('Story'),
('Post Instagram'),
('Post Facebook');
```

---

### Tabla: Audiencia

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity |
| Descripcion | nvarchar(MAX) | NO | Descripción de la audiencia |

**Índices:**
- PK_Audiencia (Id)

**Ejemplos de Datos:**
```sql
INSERT INTO Audiencia (Descripcion) VALUES
('Consultoras'),
('Consumidores Finales'),
('Gerentes'),
('Líderes de Red'),
('General');
```

---

### Tabla: Prioridad

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity |
| Descripcion | nvarchar(MAX) | NO | Descripción de la prioridad |

**Índices:**
- PK_Prioridad (Id)

**Ejemplos de Datos:**
```sql
INSERT INTO Prioridad (Descripcion) VALUES
('Alta'),
('Media'),
('Baja'),
('Urgente');
```

---

### Tabla: EstatusMateriales

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity |
| Descripcion | nvarchar(MAX) | NO | Descripción del estatus |

**Índices:**
- PK_EstatusMateriales (Id)

**Valores del Sistema:**
```sql
INSERT INTO EstatusMateriales (Id, Descripcion) VALUES
(1, 'En Revisión'),
(2, 'En Producción'),
(3, 'Falta Información'),
(4, 'Aprobado'),
(5, 'Programado'),
(6, 'Entregado');
```

**Notas:**
- El estatus 5 (Entregado) activa alertas especiales

---

### Tabla: TipoBrief

| Campo | Tipo | Nulo | Descripción |
|-------|------|------|-------------|
| Id | int | NO | PK, Identity |
| Descripcion | nvarchar(MAX) | NO | Descripción del tipo |

**Índices:**
- PK_TipoBrief (Id)

---

## RELACIONES Y LLAVES FORÁNEAS

### Relaciones Uno a Muchos

#### Usuarios → Briefs
```
Usuarios.Id (1) ←→ (N) Briefs.UsuarioId
```
- Un usuario puede crear múltiples briefs
- Un brief pertenece a un solo usuario
- Eliminación en cascada

#### Briefs → Materiales
```
Briefs.Id (1) ←→ (N) Materiales.BriefId
```
- Un brief puede tener múltiples materiales
- Un material pertenece a un solo brief
- Eliminación en cascada

#### Materiales → HistorialMaterial
```
Materiales.Id (1) ←→ (N) HistorialMaterial.MaterialId
```
- Un material puede tener múltiple historial
- Un historial pertenece a un solo material
- Sin eliminación en cascada (prevención)

#### Usuarios → HistorialMaterial
```
Usuarios.Id (1) ←→ (N) HistorialMaterial.UsuarioId
```
- Un usuario puede crear múltiples entradas de historial
- Un historial es creado por un solo usuario
- Sin eliminación en cascada

#### Roles → Usuarios
```
Roles.Id (1) ←→ (N) Usuarios.RolId
```
- Un rol puede tener múltiples usuarios
- Un usuario tiene un solo rol
- Eliminación en cascada

#### EstatusMateriales → Materiales
```
EstatusMateriales.Id (1) ←→ (N) Materiales.EstatusMaterialId
```
- Un estatus puede estar en múltiples materiales
- Un material tiene un solo estatus
- Eliminación en cascada

#### PCN → MaterialPCN
```
PCN.Id (1) ←→ (N) MaterialPCN.PCNId
```
- Parte de relación muchos a muchos
- Eliminación en cascada

---

### Relaciones Muchos a Muchos

#### Materiales ←→ PCN (a través de MaterialPCN)
```
Materiales.Id (N) ←→ MaterialPCN ←→ (N) PCN.Id
```
- Un material puede tener múltiples PCNs
- Un PCN puede estar en múltiples materiales
- Tabla intermedia: MaterialPCN
- Llave primaria compuesta: (MaterialId, PCNId)

#### Usuarios ←→ Briefs (a través de Participantes)
```
Usuarios.Id (N) ←→ Participantes ←→ (N) Briefs.Id
```
- Un usuario puede participar en múltiples briefs
- Un brief puede tener múltiples participantes
- Tabla intermedia: Participantes

---

## ÍNDICES Y CONSTRAINTS

### Índices de Rendimiento

**Índices en Llaves Foráneas:**
- Todas las llaves foráneas tienen índices automáticos
- Mejora el rendimiento de JOINs

**Índices Compuestos:**
- MaterialPCN: (MaterialId, PCNId) - PK compuesta

**Recomendaciones para Optimización:**
```sql
-- Índice para búsqueda de briefs por nombre
CREATE INDEX IX_Briefs_Nombre ON Briefs(Nombre);

-- Índice para búsqueda de usuarios por correo
CREATE UNIQUE INDEX IX_Usuarios_Correo ON Usuarios(Correo);

-- Índice para alertas no leídas
CREATE INDEX IX_Alertas_Lectura ON Alertas(lectura, IdUsuario);

-- Índice para materiales por fecha de entrega
CREATE INDEX IX_Materiales_FechaEntrega ON Materiales(FechaEntrega);
```

---

### Constraints de Integridad

#### Primary Keys
- Todas las tablas tienen PK identity
- Generación automática de IDs

#### Foreign Keys
- Configuradas con comportamiento en cascada apropiado
- Prevención de eliminación accidental en historial

#### Unique Constraints
- Correo electrónico único en Usuarios
- Llave compuesta única en MaterialPCN

---

## CONFIGURACIÓN DE ENTITY FRAMEWORK

### Cadena de Conexión en appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=adminproyectos-sqlserver;Database=AdminProyectosNaturaDB;User Id=sa;Password=Operaciones.2025;TrustServerCertificate=True;"
  }
}
```

### DbContext
```csharp
public class AdminProyectosContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Brief> Briefs { get; set; }
    public DbSet<Material> Materiales { get; set; }
    public DbSet<HistorialMaterial> HistorialMaterial { get; set; }
    public DbSet<PCN> PCN { get; set; }
    public DbSet<MaterialPCN> MaterialPCN { get; set; }
    public DbSet<Formato> Formato { get; set; }
    public DbSet<Audiencia> Audiencia { get; set; }
    public DbSet<Prioridad> Prioridad { get; set; }
    public DbSet<EstatusMateriales> EstatusMateriales { get; set; }
    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<TipoAlerta> TipoAlerta { get; set; }
    public DbSet<Participante> Participantes { get; set; }
    public DbSet<Menu> Menu { get; set; }
}
```

---

## SCRIPTS DE INICIALIZACIÓN

### Script de Creación de Roles
```sql
INSERT INTO Roles (Descripcion) VALUES
('Administrador'),
('Usuario'),
('Producción');
```

### Script de Creación de Tipos de Alerta
```sql
INSERT INTO TipoAlerta (Descripcion) VALUES
('Información'),
('Solicitud Usuario Nuevo'),
('Nuevo Comentario'),
('Cambio de Estatus'),
('Material Entregado');
```

### Script de Creación de Estatus de Materiales
```sql
INSERT INTO EstatusMateriales (Descripcion) VALUES
('En Revisión'),
('En Producción'),
('Falta Información'),
('Aprobado'),
('Programado'),
('Entregado');
```

### Script de Usuario Administrador Inicial
```sql
INSERT INTO Usuarios (Nombre, ApellidoPaterno, ApellidoMaterno, Correo, Contrasena, RolId, Estatus)
VALUES (
    'Admin',
    'Sistema',
    '',
    'admin@adminproyectos.com',
    'HASH_DE_CONTRASEÑA', -- Debe ser hasheada
    1, -- RolId Administrador
    1  -- Activo
);
```

---

## CONSULTAS COMUNES

### Obtener todos los materiales de un usuario
```sql
SELECT m.*, b.Nombre AS NombreProyecto
FROM Materiales m
INNER JOIN Briefs b ON m.BriefId = b.Id
WHERE b.UsuarioId = @UsuarioId;
```

### Obtener conteo de materiales por estatus para un usuario
```sql
SELECT
    em.Descripcion,
    COUNT(m.Id) AS Cantidad
FROM Materiales m
INNER JOIN Briefs b ON m.BriefId = b.Id
INNER JOIN EstatusMateriales em ON m.EstatusMaterialId = em.Id
WHERE b.UsuarioId = @UsuarioId
GROUP BY em.Id, em.Descripcion;
```

### Obtener alertas no leídas de un usuario
```sql
SELECT a.*, ta.Descripcion AS TipoAlerta
FROM Alertas a
INNER JOIN TipoAlerta ta ON a.IdTipoAlerta = ta.Id
WHERE a.IdUsuario = @UsuarioId
  AND a.lectura = 0
ORDER BY a.FechaCreacion DESC;
```

### Obtener PCNs de un material
```sql
SELECT p.*
FROM PCN p
INNER JOIN MaterialPCN mp ON p.Id = mp.PCNId
WHERE mp.MaterialId = @MaterialId;
```

### Obtener historial de un material
```sql
SELECT
    hm.*,
    u.Nombre + ' ' + u.ApellidoPaterno AS NombreUsuario,
    em.Descripcion AS Estatus
FROM HistorialMaterial hm
INNER JOIN Usuarios u ON hm.UsuarioId = u.Id
LEFT JOIN EstatusMateriales em ON hm.EstatusMaterialId = em.Id
WHERE hm.MaterialId = @MaterialId
ORDER BY hm.FechaCreacion DESC;
```

---

## BACKUP Y MANTENIMIENTO

### Estrategia de Backup
- Backup completo: Diario a las 2:00 AM
- Backup incremental: Cada 4 horas
- Retención: 30 días

### Script de Backup
```bash
# Backup desde Docker
docker exec adminproyectos-sqlserver /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P 'Operaciones.2025' -C \
    -Q "BACKUP DATABASE AdminProyectosNaturaDB TO DISK='/var/opt/mssql/backup/AdminProyectos_$(date +%Y%m%d_%H%M%S).bak'"
```

### Mantenimiento de Índices
```sql
-- Reorganizar índices fragmentados
ALTER INDEX ALL ON Materiales REORGANIZE;
ALTER INDEX ALL ON Briefs REORGANIZE;
ALTER INDEX ALL ON HistorialMaterial REORGANIZE;

-- Actualizar estadísticas
UPDATE STATISTICS Materiales;
UPDATE STATISTICS Briefs;
UPDATE STATISTICS HistorialMaterial;
```

---

## SEGURIDAD

### Políticas de Seguridad
- Contraseñas: Hash con salt (implementado en aplicación)
- Acceso a DB: Solo a través de Entity Framework
- Usuario SA: Solo para administración, no usar en aplicación
- TrustServerCertificate: true (solo para desarrollo)

### Recomendaciones para Producción
1. Crear usuario específico de aplicación (no usar SA)
2. Implementar SSL/TLS en conexión
3. Habilitar auditoría de SQL Server
4. Implementar Row-Level Security para datos sensibles

---

**FIN DEL DOCUMENTO DE BASE DE DATOS**
