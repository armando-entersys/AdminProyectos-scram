-- Script para aplicar migraciones faltantes en producción
-- Fecha: 2025-11-13
-- Migraciones: AgregarCamposFechaPublicacion y CambiarAudienciaMaterialAMultiselect

USE AdminProyectosNaturaDB;
GO

-- ==========================================
-- AGREGAR COLUMNAS A TABLA MATERIALES
-- ==========================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Materiales]') AND name = 'FechaPublicacion')
BEGIN
    PRINT 'Agregando columna FechaPublicacion a Materiales...';
    ALTER TABLE [dbo].[Materiales]
    ADD [FechaPublicacion] datetime2(7) NULL;
    PRINT 'Columna FechaPublicacion agregada exitosamente a Materiales.';
END
ELSE
BEGIN
    PRINT 'La columna FechaPublicacion ya existe en Materiales.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Materiales]') AND name = 'FechaPublicacionLiberada')
BEGIN
    PRINT 'Agregando columna FechaPublicacionLiberada a Materiales...';
    ALTER TABLE [dbo].[Materiales]
    ADD [FechaPublicacionLiberada] bit NOT NULL DEFAULT 0;
    PRINT 'Columna FechaPublicacionLiberada agregada exitosamente a Materiales.';
END
ELSE
BEGIN
    PRINT 'La columna FechaPublicacionLiberada ya existe en Materiales.';
END
GO

-- ==========================================
-- AGREGAR COLUMNAS A TABLA HISTORIALMATERIALES
-- ==========================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HistorialMateriales]') AND name = 'FechaPublicacion')
BEGIN
    PRINT 'Agregando columna FechaPublicacion a HistorialMateriales...';
    ALTER TABLE [dbo].[HistorialMateriales]
    ADD [FechaPublicacion] datetime2(7) NULL;
    PRINT 'Columna FechaPublicacion agregada exitosamente a HistorialMateriales.';
END
ELSE
BEGIN
    PRINT 'La columna FechaPublicacion ya existe en HistorialMateriales.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HistorialMateriales]') AND name = 'FechaPublicacionLiberada')
BEGIN
    PRINT 'Agregando columna FechaPublicacionLiberada a HistorialMateriales...';
    ALTER TABLE [dbo].[HistorialMateriales]
    ADD [FechaPublicacionLiberada] bit NOT NULL DEFAULT 0;
    PRINT 'Columna FechaPublicacionLiberada agregada exitosamente a HistorialMateriales.';
END
ELSE
BEGIN
    PRINT 'La columna FechaPublicacionLiberada ya existe en HistorialMateriales.';
END
GO

-- Insertar registros de migraciones para mantener sincronizado EF
IF NOT EXISTS (SELECT * FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20251112135900_AgregarCamposFechaPublicacion')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251112135900_AgregarCamposFechaPublicacion', N'6.0.33');
    PRINT 'Migración AgregarCamposFechaPublicacion registrada.';
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20251112151718_CambiarAudienciaMaterialAMultiselect')
BEGIN
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251112151718_CambiarAudienciaMaterialAMultiselect', N'6.0.33');
    PRINT 'Migración CambiarAudienciaMaterialAMultiselect registrada.';
END
GO

-- Verificar las columnas agregadas
SELECT
    t.name AS Tabla,
    c.name AS Columna,
    ty.name AS TipoDato,
    c.is_nullable AS Nullable,
    dc.definition AS ValorDefault
FROM sys.columns c
INNER JOIN sys.tables t ON c.object_id = t.object_id
INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id
WHERE t.name = 'HistorialMateriales'
  AND c.name IN ('FechaPublicacion', 'FechaPublicacionLiberada')
ORDER BY c.name;
GO

PRINT 'Script de migraciones completado exitosamente.';
