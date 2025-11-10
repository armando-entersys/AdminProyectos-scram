-- Script para corregir TipoAlerta con descripción "error"
-- Este script verifica si existe un TipoAlerta con descripción "error" y lo actualiza

USE AdminProyectos;
GO

-- Verificar si existe el registro problemático
SELECT * FROM dbo.TipoAlerta WHERE Descripcion LIKE '%error%';
GO

-- Si existe, necesitamos determinar a qué módulo corresponde basándose en las alertas que lo usan
-- Primero, veamos qué alertas están usando este TipoAlerta
SELECT a.Id, a.Nombre, a.Descripcion, a.Accion, t.Descripcion as TipoAlerta
FROM dbo.Alertas a
INNER JOIN dbo.TipoAlerta t ON a.IdTipoAlerta = t.Id
WHERE t.Descripcion LIKE '%error%';
GO

-- Los tipos de alerta correctos según el código son:
-- 1 = Nuevo Proyecto
-- 2 = Cambio de Estado
-- 3 = Actualización
-- 4 = Nuevo Material
-- 5 = Material Entregado

-- Si hay un TipoAlerta con "error" que no corresponde a ninguno de estos,
-- podemos eliminarlo SI NO HAY alertas que lo referencien, o actualizar su descripción

-- Opción 1: Si NO hay alertas usando este tipo, eliminarlo
-- DELETE FROM dbo.TipoAlerta WHERE Descripcion LIKE '%error%';

-- Opción 2: Si HAY alertas usando este tipo, actualizarlo a un tipo válido
-- Por ejemplo, si parece ser una actualización:
-- UPDATE dbo.TipoAlerta SET Descripcion = 'Actualización', Activo = 1 WHERE Descripcion LIKE '%error%';

-- IMPORTANTE: Ejecutar primero las consultas SELECT para verificar el estado actual
-- antes de ejecutar DELETE o UPDATE
