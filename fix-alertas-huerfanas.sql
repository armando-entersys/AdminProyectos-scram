-- Script para corregir alertas con IdTipoAlerta inválido
-- NOTA: Cambiar AdminProyectosNaturaDB por AdminProyectos si es necesario
USE AdminProyectosNaturaDB;
GO

-- 1. Verificar todos los TipoAlerta que existen actualmente
SELECT '=== TIPOS DE ALERTA EXISTENTES ===' AS Info;
SELECT * FROM dbo.TipoAlerta ORDER BY Id;
GO

-- 2. Verificar alertas con IdTipoAlerta que no existen en TipoAlerta
SELECT '=== ALERTAS HUÉRFANAS (sin TipoAlerta válido) ===' AS Info;
SELECT a.Id, a.Nombre, a.Descripcion, a.IdTipoAlerta, a.IdUsuario, a.FechaCreacion
FROM dbo.Alertas a
LEFT JOIN dbo.TipoAlerta t ON a.IdTipoAlerta = t.Id
WHERE t.Id IS NULL;
GO

-- 3. Ver estadísticas de IdTipoAlerta usados en Alertas
SELECT '=== ESTADÍSTICAS DE IdTipoAlerta EN ALERTAS ===' AS Info;
SELECT
    a.IdTipoAlerta,
    t.Descripcion AS TipoAlertaDescripcion,
    COUNT(*) AS CantidadAlertas,
    CASE WHEN t.Id IS NULL THEN 'HUÉRFANA' ELSE 'VÁLIDA' END AS Estado
FROM dbo.Alertas a
LEFT JOIN dbo.TipoAlerta t ON a.IdTipoAlerta = t.Id
GROUP BY a.IdTipoAlerta, t.Descripcion, t.Id
ORDER BY a.IdTipoAlerta;
GO

-- 4. Script para INSERTAR los tipos de alerta faltantes (si es necesario)
-- Los tipos según el código son:
-- 1 = Nuevo Proyecto
-- 2 = Cambio de Estado (Usuarios)
-- 3 = Actualización
-- 4 = Nuevo Material
-- 5 = Material Entregado

-- Primero verificamos cuáles faltan
IF NOT EXISTS (SELECT 1 FROM dbo.TipoAlerta WHERE Id = 1)
BEGIN
    SET IDENTITY_INSERT dbo.TipoAlerta ON;
    INSERT INTO dbo.TipoAlerta (Id, Descripcion, Activo) VALUES (1, 'Nuevo Proyecto', 1);
    SET IDENTITY_INSERT dbo.TipoAlerta OFF;
    PRINT 'Se insertó TipoAlerta Id=1 (Nuevo Proyecto)';
END

IF NOT EXISTS (SELECT 1 FROM dbo.TipoAlerta WHERE Id = 2)
BEGIN
    SET IDENTITY_INSERT dbo.TipoAlerta ON;
    INSERT INTO dbo.TipoAlerta (Id, Descripcion, Activo) VALUES (2, 'Cambio de Estado', 1);
    SET IDENTITY_INSERT dbo.TipoAlerta OFF;
    PRINT 'Se insertó TipoAlerta Id=2 (Cambio de Estado)';
END

IF NOT EXISTS (SELECT 1 FROM dbo.TipoAlerta WHERE Id = 3)
BEGIN
    SET IDENTITY_INSERT dbo.TipoAlerta ON;
    INSERT INTO dbo.TipoAlerta (Id, Descripcion, Activo) VALUES (3, 'Actualización', 1);
    SET IDENTITY_INSERT dbo.TipoAlerta OFF;
    PRINT 'Se insertó TipoAlerta Id=3 (Actualización)';
END

IF NOT EXISTS (SELECT 1 FROM dbo.TipoAlerta WHERE Id = 4)
BEGIN
    SET IDENTITY_INSERT dbo.TipoAlerta ON;
    INSERT INTO dbo.TipoAlerta (Id, Descripcion, Activo) VALUES (4, 'Nuevo Material', 1);
    SET IDENTITY_INSERT dbo.TipoAlerta OFF;
    PRINT 'Se insertó TipoAlerta Id=4 (Nuevo Material)';
END

IF NOT EXISTS (SELECT 1 FROM dbo.TipoAlerta WHERE Id = 5)
BEGIN
    SET IDENTITY_INSERT dbo.TipoAlerta ON;
    INSERT INTO dbo.TipoAlerta (Id, Descripcion, Activo) VALUES (5, 'Material Entregado', 1);
    SET IDENTITY_INSERT dbo.TipoAlerta OFF;
    PRINT 'Se insertó TipoAlerta Id=5 (Material Entregado)';
END
GO

-- 5. Verificar el resultado final
SELECT '=== TIPOS DE ALERTA DESPUÉS DEL FIX ===' AS Info;
SELECT * FROM dbo.TipoAlerta ORDER BY Id;
GO

-- 6. Verificar que ya no haya alertas huérfanas
SELECT '=== VERIFICACIÓN FINAL - ALERTAS HUÉRFANAS ===' AS Info;
SELECT COUNT(*) AS AlertasHuerfanas
FROM dbo.Alertas a
LEFT JOIN dbo.TipoAlerta t ON a.IdTipoAlerta = t.Id
WHERE t.Id IS NULL;
GO
