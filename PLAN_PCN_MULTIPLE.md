# Plan de Implementación: PCN Múltiple

## 📋 Descripción
Cambiar la relación Material-PCN de uno-a-uno a muchos-a-muchos, permitiendo que un material pueda tener múltiples PCN asignados.

## 🎯 Objetivo
Permitir seleccionar múltiples valores de PCN para cada material en lugar de solo uno.

## 📊 Cambios en el Modelo de Datos

### 1. Crear Tabla Intermedia

**Nueva entidad: `MaterialPCN.cs`**
```csharp
namespace EntityLayer.Concrete
{
    public class MaterialPCN
    {
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public int PCNId { get; set; }
        public PCN PCN { get; set; }
    }
}
```

### 2. Modificar Material.cs

**Cambios:**
- ❌ Eliminar: `public int PCNId { get; set; }`
- ❌ Eliminar: `public PCN PCN { get; set; }`
- ✅ Agregar: `public ICollection<MaterialPCN> MaterialPCNs { get; set; } = new List<MaterialPCN>();`

**Código modificado:**
```csharp
public class Material
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Mensaje { get; set; }
    public int PrioridadId { get; set; }
    public Prioridad Prioridad { get; set; }

    public string Ciclo { get; set; }

    // ❌ ELIMINAR ESTAS LÍNEAS:
    // public int PCNId { get; set; }
    // public PCN PCN { get; set; }

    // ✅ AGREGAR ESTA LÍNEA:
    public ICollection<MaterialPCN> MaterialPCNs { get; set; } = new List<MaterialPCN>();

    // ... resto del código
}
```

### 3. Modificar PCN.cs

**Agregar:**
```csharp
public ICollection<MaterialPCN> MaterialPCNs { get; set; } = new List<MaterialPCN>();
```

### 4. Configurar en DbContext

**En `AdminContext.cs` dentro de `OnModelCreating`:**
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // ... código existente ...

    // Configurar relación muchos a muchos Material-PCN
    modelBuilder.Entity<MaterialPCN>()
        .HasKey(mp => new { mp.MaterialId, mp.PCNId });

    modelBuilder.Entity<MaterialPCN>()
        .HasOne(mp => mp.Material)
        .WithMany(m => m.MaterialPCNs)
        .HasForeignKey(mp => mp.MaterialId);

    modelBuilder.Entity<MaterialPCN>()
        .HasOne(mp => mp.PCN)
        .WithMany(p => p.MaterialPCNs)
        .HasForeignKey(mp => mp.PCNId);
}
```

## 🗄️ Migración de Base de Datos

### Opción A: Crear nueva migración (EF Core)

```bash
# Desde PresentationLayer
dotnet ef migrations add AddMaterialPCNManyToMany
dotnet ef database update
```

### Opción B: Script SQL Manual

**Para migrar datos existentes:**

```sql
-- 1. Crear tabla intermedia
CREATE TABLE MaterialPCN (
    MaterialId INT NOT NULL,
    PCNId INT NOT NULL,
    PRIMARY KEY (MaterialId, PCNId),
    FOREIGN KEY (MaterialId) REFERENCES Materiales(Id) ON DELETE CASCADE,
    FOREIGN KEY (PCNId) REFERENCES PCN(Id) ON DELETE CASCADE
);

-- 2. Migrar datos existentes (copiar PCN actual de cada material)
INSERT INTO MaterialPCN (MaterialId, PCNId)
SELECT Id, PCNId
FROM Materiales
WHERE PCNId IS NOT NULL;

-- 3. Eliminar columna antigua (CUIDADO: backup primero!)
-- ALTER TABLE Materiales DROP COLUMN PCNId;
```

**⚠️ IMPORTANTE:**
- Hacer backup completo de la BD antes de ejecutar
- El paso 3 es irreversible
- Considerar mantener `PCNId` temporalmente para rollback

## 💻 Cambios en la Interfaz de Usuario

### 1. Vista Brief/IndexAdmin.cshtml

**Cambiar de select simple a multi-select:**

```html
<!-- ANTES (línea ~297): -->
<select class="form-control"
    data-bind="options: catPCN, value: pcn,
    optionsText: 'descripcion', optionsCaption: 'Seleccione'">
</select>

<!-- DESPUÉS: -->
<select multiple class="form-control" size="5"
    data-bind="options: catPCN, selectedOptions: pcnsSeleccionados,
    optionsText: 'descripcion'">
</select>
<small class="text-muted">Mantén Ctrl para seleccionar múltiples</small>
```

**O usar checkboxes (mejor UX):**
```html
<div data-bind="foreach: catPCN">
    <div class="form-check">
        <input class="form-check-input" type="checkbox"
            data-bind="checkedValue: $data, checked: $root.pcnsSeleccionados,
            attr: { id: 'pcn_' + id }">
        <label class="form-check-label" data-bind="text: descripcion,
            attr: { for: 'pcn_' + id }"></label>
    </div>
</div>
```

### 2. ViewModel Knockout (BriefAdmin.js)

**Modificar:**
```javascript
// ANTES:
self.pcn = ko.observable();

// DESPUÉS:
self.pcnsSeleccionados = ko.observableArray([]);
```

**Al guardar material:**
```javascript
self.GuardarMaterial = function () {
    var material = {
        // ... otros campos ...
        PCNIds: self.pcnsSeleccionados().map(pcn => pcn.id) // Array de IDs
    };

    $.ajax({
        url: "Brief/AgregarMaterial",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(material),
        // ...
    });
};
```

**Al editar material (cargar PCNs existentes):**
```javascript
self.EditarMaterial = function(material) {
    // ... otros campos ...

    // Cargar PCNs seleccionados
    var pcnsExistentes = material.materialPCNs.map(mp => {
        return self.catPCN().find(p => p.id === mp.pcnId);
    });
    self.pcnsSeleccionados(pcnsExistentes);
};
```

## 🔧 Cambios en el Backend

### 1. Crear DTO para Material

**`AgregarMaterialRequest.cs`:**
```csharp
public class AgregarMaterialRequest
{
    public string Nombre { get; set; }
    public string Mensaje { get; set; }
    public string Ciclo { get; set; }
    public int PrioridadId { get; set; }
    public List<int> PCNIds { get; set; } // Lista de PCNs
    public int AudienciaId { get; set; }
    public int FormatoId { get; set; }
    public DateTime FechaEntrega { get; set; }
    public string Responsable { get; set; }
    public string Area { get; set; }
    public int BriefId { get; set; }
}
```

### 2. Modificar BriefService

**Método para agregar material:**
```csharp
public void AgregarMaterial(AgregarMaterialRequest request)
{
    var material = new Material
    {
        Nombre = request.Nombre,
        Mensaje = request.Mensaje,
        Ciclo = request.Ciclo,
        PrioridadId = request.PrioridadId,
        // NO asignar PCNId aquí
        AudienciaId = request.AudienciaId,
        FormatoId = request.FormatoId,
        FechaEntrega = request.FechaEntrega,
        Responsable = request.Responsable,
        Area = request.Area,
        BriefId = request.BriefId,
        EstatusMaterialId = 1, // Por defecto
        FechaRegistro = DateTime.Now
    };

    // Agregar material primero
    _context.Materiales.Add(material);
    _context.SaveChanges();

    // Luego agregar las relaciones MaterialPCN
    foreach (var pcnId in request.PCNIds)
    {
        _context.Set<MaterialPCN>().Add(new MaterialPCN
        {
            MaterialId = material.Id,
            PCNId = pcnId
        });
    }

    _context.SaveChanges();
}
```

**Método para obtener materiales (incluir PCNs):**
```csharp
public List<Material> GetMateriales()
{
    return _context.Materiales
        .Include(m => m.MaterialPCNs)
            .ThenInclude(mp => mp.PCN)
        .Include(m => m.Prioridad)
        .Include(m => m.Audiencia)
        .Include(m => m.Formato)
        .Include(m => m.EstatusMaterial)
        .Include(m => m.Brief)
        .ToList();
}
```

## 📝 Cambios en Vistas de Solo Lectura

### Materiales/Index.cshtml (Vista de materiales)

**Mostrar múltiples PCN:**
```html
<!-- ANTES: -->
<span data-bind="text: pcn?.descripcion"></span>

<!-- DESPUÉS: -->
<span data-bind="text: materialPCNs.map(mp => mp.pcn.descripcion).join(', ')"></span>
```

**O con badges:**
```html
<div data-bind="foreach: materialPCNs">
    <span class="badge bg-info me-1" data-bind="text: pcn.descripcion"></span>
</div>
```

## ✅ Checklist de Implementación

### Fase 1: Preparación
- [ ] Hacer backup completo de la base de datos
- [ ] Documentar estructura actual
- [ ] Crear rama Git para los cambios: `git checkout -b feature/multiple-pcn`

### Fase 2: Modelo de Datos
- [ ] Crear entidad `MaterialPCN.cs`
- [ ] Modificar `Material.cs` (agregar colección)
- [ ] Modificar `PCN.cs` (agregar colección)
- [ ] Configurar relación en `AdminContext.cs`
- [ ] Verificar que compila

### Fase 3: Migración BD
- [ ] Crear migración: `dotnet ef migrations add AddMaterialPCNManyToMany`
- [ ] Revisar script SQL generado
- [ ] Aplicar en base de datos de prueba
- [ ] Verificar que datos se migraron correctamente
- [ ] Aplicar en producción

### Fase 4: Backend
- [ ] Crear `AgregarMaterialRequest.cs` con `List<int> PCNIds`
- [ ] Modificar `BriefService.AgregarMaterial()`
- [ ] Modificar `BriefService.ActualizarMaterial()`
- [ ] Modificar `BriefService.GetMateriales()` para incluir `MaterialPCNs`
- [ ] Actualizar controladores que usan materiales

### Fase 5: Frontend
- [ ] Modificar `BriefAdmin.js`: cambiar `pcn` observable por `pcnsSeleccionados` observableArray
- [ ] Actualizar `IndexAdmin.cshtml`: cambiar select por multi-select o checkboxes
- [ ] Modificar función `GuardarMaterial` para enviar array de IDs
- [ ] Modificar función `EditarMaterial` para cargar PCNs múltiples
- [ ] Actualizar vista de solo lectura en `Materiales/Index.cshtml`

### Fase 6: Testing
- [ ] Crear material con 1 PCN
- [ ] Crear material con múltiples PCN
- [ ] Editar material y cambiar PCNs
- [ ] Verificar que se muestran correctamente en lista
- [ ] Verificar que no se rompieron funcionalidades existentes
- [ ] Probar eliminación de materiales (cascade debe funcionar)

### Fase 7: Deployment
- [ ] Merge a master
- [ ] Push a repositorio
- [ ] Deployment a staging
- [ ] Testing en staging
- [ ] Deployment a producción

## ⚠️ Riesgos y Consideraciones

### Riesgos:
1. **Pérdida de datos**: Si no se hace backup antes de la migración
2. **Downtime**: Durante la migración en producción
3. **Performance**: Las consultas con múltiples joins pueden ser más lentas
4. **Complejidad**: Más código para mantener

### Mitigaciones:
1. Backup automático antes de migración
2. Migración en horario de bajo tráfico
3. Agregar índices en tabla `MaterialPCN`
4. Tests exhaustivos antes de producción

## 🔄 Plan de Rollback

Si algo sale mal:

```sql
-- 1. Restaurar backup
RESTORE DATABASE AdminProyectosNaturaDB
FROM DISK = '/backup/AdminProyectos_backup.bak'
WITH REPLACE;

-- 2. O si solo hay que revertir cambios de tabla:
DROP TABLE MaterialPCN;

ALTER TABLE Materiales ADD PCNId INT;

UPDATE Materiales
SET PCNId = (
    SELECT TOP 1 PCNId
    FROM MaterialPCN_BACKUP
    WHERE MaterialId = Materiales.Id
);
```

## 📊 Estimación de Esfuerzo

- **Desarrollo**: 4-6 horas
- **Testing**: 2-3 horas
- **Deployment**: 1 hora
- **Total**: 7-10 horas

## 🎓 Referencias

- [EF Core Many-to-Many](https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many)
- [Knockout Observable Arrays](https://knockoutjs.com/documentation/observableArrays.html)
- [SQL Server Migrations Best Practices](https://www.red-gate.com/hub/product-learning/sql-change-automation/database-migration-best-practices)

---

**Fecha de creación**: 2025-11-08
**Autor**: Claude Code
**Estado**: Pendiente de aprobación
