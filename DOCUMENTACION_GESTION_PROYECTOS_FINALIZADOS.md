# Gestión de Proyectos Finalizados - Sistema AdminProyectos NATURA

**Fecha**: Noviembre 2025
**Versión**: 1.0
**Reporte**: #7

---

## 1. Situación Actual

### ¿Qué pasa con los proyectos finalizados?

Actualmente, el sistema **mantiene todos los proyectos** (Briefs y Materiales) en la base de datos de forma permanente, independientemente de su estado. Esto incluye:

- Proyectos en curso
- Proyectos finalizados
- Materiales entregados
- Materiales con estado "Listo para Publicación"

### Visualización en Calendario

Los proyectos finalizados **continúan apareciendo en el calendario** en las siguientes vistas:
- Vista mensual (dayGridMonth)
- Vista semanal (timeGridWeek)
- Vista diaria (timeGridDay)

El calendario muestra dos tipos de eventos:
- **Fechas de Entrega** (color azul - #3788d8)
- **Fechas de Publicación** (color rojo - #D9534F) - solo si están liberadas o el usuario es Admin

---

## 2. Impacto del Cúmulo de Proyectos

### Problemas Potenciales

A medida que se acumulan proyectos, pueden presentarse los siguientes desafíos:

1. **Rendimiento del Calendario**
   - Mayor tiempo de carga al consultar meses con muchos eventos
   - Visualización saturada en vistas mensuales

2. **Experiencia de Usuario**
   - Dificultad para encontrar proyectos activos entre proyectos finalizados
   - Calendario con información histórica que puede no ser relevante

3. **Tamaño de Base de Datos**
   - Crecimiento continuo sin límites definidos
   - Mayor tiempo en consultas y respaldos

---

## 3. Propuestas de Solución

### Opción A: Sistema de Archivo (Recomendada para Fase 1)

**Implementación Sugerida:**

1. **Agregar campo "Archivado" a Brief y Material**
   ```sql
   ALTER TABLE Briefs ADD Archivado BIT DEFAULT 0;
   ALTER TABLE Materiales ADD Archivado BIT DEFAULT 0;
   ```

2. **Crear función de archivo automático**
   - Archivar automáticamente proyectos finalizados después de **6 meses** de su fecha de publicación
   - Ejecutar proceso mensual mediante BackgroundService

3. **Filtros en Vistas**
   - Por defecto, mostrar solo proyectos NO archivados
   - Agregar opción "Ver Archivados" para consultas históricas
   - Calendario: filtrar eventos archivados por defecto

4. **Ventajas:**
   - No se pierde información histórica
   - Fácil recuperación si se necesita
   - Mejora inmediata de rendimiento
   - No requiere cambios drásticos en la estructura

---

### Opción B: Eliminación Programada (Para Fase Futura)

**Características:**

1. **Eliminación lógica con periodo de gracia**
   - Marcar para eliminación después de 12 meses
   - Periodo de 30 días antes de eliminación permanente
   - Notificación al administrador antes de eliminar

2. **Exportación antes de eliminación**
   - Generar reporte PDF con toda la información del proyecto
   - Almacenar en sistema de archivos o cloud storage
   - Mantener solo referencia en base de datos

3. **Ventajas:**
   - Reduce significativamente el tamaño de BD
   - Mantiene registros para auditoría
   - Mayor control sobre datos históricos

---

### Opción C: Vista de Calendario Inteligente (Mejora UI)

**Funcionalidad:**

1. **Filtros dinámicos en calendario**
   - Filtrar por rango de fechas (últimos 3, 6, 12 meses)
   - Filtrar por estado del proyecto
   - Ocultar/mostrar proyectos finalizados

2. **Vista predeterminada optimizada**
   - Cargar solo eventos de los últimos 3 meses + próximos 6 meses
   - Opción "Ver Todo" para cargar histórico completo

3. **Ventajas:**
   - No requiere cambios en BD
   - Implementación rápida
   - Mejora experiencia de usuario inmediatamente

---

## 4. Recomendación Final

### Implementación por Fases

**Fase Inmediata (Recomendada):**
- Implementar **Opción A + Opción C** combinadas
- Sistema de archivo con filtros inteligentes
- Tiempo estimado: 2-3 días de desarrollo

**Fase Futura (Opcional):**
- Evaluar necesidad de **Opción B** después de 12 meses de operación
- Basado en métricas de uso y tamaño de BD

---

## 5. Consideraciones Técnicas

### Archivos Relacionados a Modificar

```
PresentationLayer/
├── Controllers/CalendarioController.cs
├── wwwroot/js/Calendario/Calendario.js
└── Views/Calendario/Index.cshtml

BusinessLayer/
└── Concrete/BriefService.cs
└── Concrete/MaterialService.cs

DataAccessLayer/
├── Repositories/BriefRepository.cs
├── Repositories/MaterialRepository.cs
└── Migrations/[Nueva migración para campo Archivado]

EntityLayer/
├── Concrete/Brief.cs
└── Concrete/Material.cs
```

### Servicios a Crear

```csharp
// PresentationLayer/Services/ProjectArchiveService.cs
public class ProjectArchiveService : IHostedService
{
    // Ejecutar mensualmente para archivar proyectos antiguos
    // Buscar materiales con FechaPublicacion > 6 meses
    // Marcar como Archivado = true
}
```

---

## 6. Métricas y Monitoreo

Para evaluar el impacto y necesidad de estas soluciones, se recomienda monitorear:

1. **Número total de proyectos** en sistema
2. **Proyectos creados por mes**
3. **Tiempo de carga del calendario**
4. **Tamaño de base de datos**
5. **Consultas a proyectos archivados vs activos**

---

## 7. Documentación de Usuario

Una vez implementada la solución elegida, se debe capacitar a los usuarios sobre:

- Cómo acceder a proyectos archivados
- Criterios de archivo automático
- Proceso de recuperación de proyectos archivados
- Exportación de información histórica

---

**Documento elaborado por**: Sistema AdminProyectos - Equipo de Desarrollo
**Última actualización**: Noviembre 2025
**Próxima revisión**: Después de implementación
