-- Script para actualizar nombres de EstatusMateriales
-- Fecha: 2025-11-14
-- Tarea: Corregir Reporte 2 - Estados para Producción

USE AdminProyectosNaturaDB;
GO

-- ==========================================
-- ACTUALIZAR NOMBRES DE ESTADOS
-- ==========================================

-- Estado ID 1: Mantener "En Revisión"
PRINT 'Estado 1: Mantener "En Revisión"';

-- Estado ID 2: "Falta Información" → "En diseño"
UPDATE EstatusMateriales
SET Descripcion = 'En diseño'
WHERE Id = 2;
PRINT 'Estado 2: "Falta Información" → "En diseño"';

-- Estado ID 3: "Aprobado" → "En revisión" (aprobado por solicitante)
UPDATE EstatusMateriales
SET Descripcion = 'En revisión'
WHERE Id = 3;
PRINT 'Estado 3: "Aprobado" → "En revisión"';

-- Estado ID 4: "Programado" → "Listo para publicación"
UPDATE EstatusMateriales
SET Descripcion = 'Listo para publicación'
WHERE Id = 4;
PRINT 'Estado 4: "Programado" → "Listo para publicación"';

-- Estado ID 5: "Entregado" → "En producción"
UPDATE EstatusMateriales
SET Descripcion = 'En producción'
WHERE Id = 5;
PRINT 'Estado 5: "Entregado" → "En producción"';

-- Estado ID 6: "Inicio de Ciclo" → Desactivar
UPDATE EstatusMateriales
SET Activo = 0
WHERE Id = 6;
PRINT 'Estado 6: "Inicio de Ciclo" → Desactivado';

GO

-- ==========================================
-- VERIFICAR CAMBIOS
-- ==========================================
PRINT '';
PRINT 'Estados actualizados:';
SELECT Id, Descripcion, Activo
FROM EstatusMateriales
ORDER BY Id;

GO

PRINT '';
PRINT 'Script completado exitosamente.';
PRINT 'Estados activos para Producción: 1, 2, 3, 4, 5';
PRINT 'Estados para otros roles: Configurar en Material.js';
