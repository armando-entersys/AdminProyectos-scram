-- ========================================
-- MIGRACIÓN: PCN Múltiple
-- Cambiar relación Material-PCN de uno-a-uno a muchos-a-muchos
-- ========================================
-- IMPORTANTE: Hacer backup de la base de datos ANTES de ejecutar este script
-- ========================================

USE AdminProyectosNaturaDB;
GO

-- Paso 1: Crear tabla intermedia MaterialPCN
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MaterialPCN]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[MaterialPCN](
        [MaterialId] INT NOT NULL,
        [PCNId] INT NOT NULL,
        CONSTRAINT [PK_MaterialPCN] PRIMARY KEY CLUSTERED ([MaterialId] ASC, [PCNId] ASC),
        CONSTRAINT [FK_MaterialPCN_Materiales] FOREIGN KEY([MaterialId])
            REFERENCES [dbo].[Materiales] ([Id])
            ON DELETE CASCADE,
        CONSTRAINT [FK_MaterialPCN_PCN] FOREIGN KEY([PCNId])
            REFERENCES [dbo].[PCN] ([Id])
            ON DELETE CASCADE
    )
    PRINT 'Tabla MaterialPCN creada exitosamente.'
END
ELSE
BEGIN
    PRINT 'Tabla MaterialPCN ya existe.'
END
GO

-- Paso 2: Migrar datos existentes de Materiales.PCNId a MaterialPCN
-- Solo si la columna PCNId existe en la tabla Materiales
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Materiales]') AND name = 'PCNId')
BEGIN
    -- Insertar registros existentes en la tabla intermedia
    INSERT INTO [dbo].[MaterialPCN] (MaterialId, PCNId)
    SELECT Id, PCNId
    FROM [dbo].[Materiales]
    WHERE PCNId IS NOT NULL;

    DECLARE @RowCount INT = @@ROWCOUNT;
    PRINT CAST(@RowCount AS NVARCHAR(10)) + ' registros migrados de Materiales.PCNId a MaterialPCN.';

    -- Paso 3: Eliminar la columna PCNId de la tabla Materiales
    -- NOTA: Esto es irreversible. Asegúrate de tener un backup.
    ALTER TABLE [dbo].[Materiales] DROP CONSTRAINT IF EXISTS [FK_Materiales_PCN_PCNId];
    ALTER TABLE [dbo].[Materiales] DROP COLUMN [PCNId];
    PRINT 'Columna PCNId eliminada de la tabla Materiales.';
END
ELSE
BEGIN
    PRINT 'La columna PCNId no existe en Materiales. Migración ya completada o no necesaria.';
END
GO

-- Paso 4: Verificación
SELECT
    'Materiales' AS Tabla,
    COUNT(*) AS TotalRegistros
FROM [dbo].[Materiales]
UNION ALL
SELECT
    'MaterialPCN' AS Tabla,
    COUNT(*) AS TotalRegistros
FROM [dbo].[MaterialPCN]
UNION ALL
SELECT
    'PCN' AS Tabla,
    COUNT(*) AS TotalRegistros
FROM [dbo].[PCN];
GO

-- Consulta de ejemplo: Materiales con sus PCNs
SELECT TOP 10
    m.Id AS MaterialId,
    m.Nombre AS Material,
    STRING_AGG(p.Descripcion, ', ') AS PCNs
FROM [dbo].[Materiales] m
LEFT JOIN [dbo].[MaterialPCN] mp ON m.Id = mp.MaterialId
LEFT JOIN [dbo].[PCN] p ON mp.PCNId = p.Id
GROUP BY m.Id, m.Nombre
ORDER BY m.Id;
GO

PRINT '========================================';
PRINT 'Migración completada exitosamente.';
PRINT 'Verifica que los datos se hayan migrado correctamente antes de continuar.';
PRINT '========================================';
