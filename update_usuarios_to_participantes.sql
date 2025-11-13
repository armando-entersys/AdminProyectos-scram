-- Script para actualizar "Usuarios" a "Participantes" en el menú
-- Reporte 3: Renombrar "Usuarios" → "Participantes" + color rojo

USE AdminProyectos;
GO

-- Actualizar el nombre del menú de "Usuarios" a "Participantes"
UPDATE Menu
SET Nombre = 'Participantes'
WHERE Nombre = 'Usuarios'
  AND Ruta = '/Usuarios/Index';
GO

-- Verificar los cambios
SELECT Id, Nombre, Ruta, Orden, Icono, RolId
FROM Menu
WHERE Ruta = '/Usuarios/Index';
GO
