-- =====================================================
-- SEED DATA - AdminProyectos Natura
-- Versión Final para Producción
-- Generado: 2025-11-20
-- =====================================================

-- Limpiar todas las tablas en orden correcto
DELETE FROM HistorialMateriales;
DELETE FROM MaterialAudiencia;
DELETE FROM MaterialPCN;
DELETE FROM RetrasoMateriales;
DELETE FROM Materiales;
DELETE FROM Participantes;
DELETE FROM Briefs;
DELETE FROM Alertas;
DELETE FROM Usuarios;
DELETE FROM Menus;
DELETE FROM PCN;
DELETE FROM Audiencia;
DELETE FROM Formato;
DELETE FROM Prioridad;
DELETE FROM TipoAlerta;
DELETE FROM EstatusMateriales;
DELETE FROM EstatusBriefs;
DELETE FROM TiposBrief;
DELETE FROM Roles;

-- Reset identity columns
DBCC CHECKIDENT ('Usuarios', RESEED, 0);
DBCC CHECKIDENT ('Briefs', RESEED, 0);
DBCC CHECKIDENT ('Materiales', RESEED, 0);
DBCC CHECKIDENT ('Alertas', RESEED, 0);

-- =====================================================
-- ROLES
-- =====================================================
SET IDENTITY_INSERT Roles ON;
INSERT INTO Roles (Id, Descripcion) VALUES
(1, 'Administrador'),
(2, 'Usuario'),
(3, 'Producción');
SET IDENTITY_INSERT Roles OFF;

-- =====================================================
-- TIPOS DE BRIEF
-- =====================================================
SET IDENTITY_INSERT TiposBrief ON;
INSERT INTO TiposBrief (Id, Descripcion, Activo) VALUES
(1, 'Ciclal', 1),
(2, 'Emergencial', 1);
SET IDENTITY_INSERT TiposBrief OFF;

-- =====================================================
-- ESTATUS DE BRIEFS
-- =====================================================
SET IDENTITY_INSERT EstatusBriefs ON;
INSERT INTO EstatusBriefs (Id, Descripcion, Activo) VALUES
(1, 'En revisión', 1),
(2, 'Producción', 1),
(3, 'Falta información', 1),
(4, 'Programado', 1),
(6, 'Finalizado', 1);
SET IDENTITY_INSERT EstatusBriefs OFF;

-- =====================================================
-- ESTATUS DE MATERIALES
-- =====================================================
SET IDENTITY_INSERT EstatusMateriales ON;
INSERT INTO EstatusMateriales (Id, Descripcion, Activo) VALUES
(1, 'Pendiente', 1),
(2, 'En diseño', 1),
(3, 'En revisión', 1),
(4, 'Listo para publicación', 1),
(5, 'En producción', 1),
(6, 'Entregado', 0),
(7, 'Rechazado', 1),
(8, 'No se compartio', 1);
SET IDENTITY_INSERT EstatusMateriales OFF;

-- =====================================================
-- TIPOS DE ALERTA
-- =====================================================
SET IDENTITY_INSERT TipoAlerta ON;
INSERT INTO TipoAlerta (Id, Descripcion, Activo) VALUES
(1, 'Información', 1),
(2, 'Advertencia', 1),
(3, 'Actualización', 1),
(4, 'Notificación', 1),
(5, 'Material Entregado', 1);
SET IDENTITY_INSERT TipoAlerta OFF;

-- =====================================================
-- FORMATOS
-- =====================================================
SET IDENTITY_INSERT Formato ON;
INSERT INTO Formato (Id, Descripcion, Activo) VALUES
(1, 'WhatsApp', 1),
(2, 'Story', 1),
(3, 'Video', 1),
(4, 'Texto', 1),
(5, 'Card', 1),
(6, 'Banner home', 1),
(7, 'Comunicado', 1),
(8, 'Marco para story', 1),
(9, 'Infografía', 1),
(10, 'Albúm', 1),
(11, 'Placa', 1),
(12, 'Mailing', 1),
(13, 'Guía interactiva', 1),
(14, 'PDF', 1),
(15, 'Reel', 1),
(16, 'Video corto (vertical)', 1),
(17, 'Historia destacada IG', 1),
(18, 'Post', 1),
(19, 'Carrusel', 1),
(20, 'Placa animada', 1),
(21, 'Impresos', 1),
(22, 'Stickers', 1),
(23, 'Diada', 1),
(24, 'Pop up', 1),
(25, 'Ícono interactivo', 1),
(26, 'Banner', 1),
(27, 'Linktree', 1),
(28, 'PDF con link', 1);
SET IDENTITY_INSERT Formato OFF;

-- =====================================================
-- AUDIENCIAS
-- =====================================================
SET IDENTITY_INSERT Audiencia ON;
INSERT INTO Audiencia (Id, Descripcion, Activo) VALUES
(1, 'GV', 1),
(2, 'GNs y Líderes', 1),
(3, 'Base específica', 1),
(4, 'Consultor', 1),
(6, 'Zafiro y Diamante', 1),
(7, 'Todo el canal', 1),
(8, 'Activas', 1),
(9, 'Disponibles', 1),
(10, 'CF', 1),
(11, 'INA 1 y 2', 1),
(12, 'Zafiro, Oro y Diamante', 1),
(13, 'GV1-2', 1),
(14, 'Solo Avon', 1),
(15, 'Diamantes', 1),
(16, 'CND', 1),
(17, 'GV3-16', 1);
SET IDENTITY_INSERT Audiencia OFF;

-- =====================================================
-- PRIORIDADES
-- =====================================================
SET IDENTITY_INSERT Prioridad ON;
INSERT INTO Prioridad (Id, Descripcion, Activo) VALUES
(1, 'Baja', 1),
(2, 'Mediana', 1),
(3, 'Grande', 1),
(4, 'Urgente', 1);
SET IDENTITY_INSERT Prioridad OFF;

-- =====================================================
-- PCN (Puntos de Contacto)
-- =====================================================
SET IDENTITY_INSERT PCN ON;
INSERT INTO PCN (Id, Descripcion, Activo) VALUES
(1, 'Mi Negocio', 1),
(2, 'Facebook - Consultoría de Belleza Natura y Avon', 1),
(3, 'Instagram - Consultoría de Belleza Natura y Avon', 1),
(4, 'WhatsApp Estrategia', 1),
(5, 'WhatsApp GNs', 1),
(6, 'SMS', 1),
(7, 'Instagram - Natura México', 1),
(8, 'Facebook - Natura México', 1),
(9, 'WhatsApp Consultor', 1),
(10, 'Mailing', 1),
(11, 'Instagram - Avon México', 1),
(12, 'Facebook - Avon México', 1),
(13, 'Espacios tiendas Natura', 1),
(14, 'Revista', 1),
(15, 'Revista digital', 1),
(16, 'Sitio web Natura CF', 1),
(17, 'Canal YouTube Escuela Natura y Avon', 1),
(18, 'WhatsApp Líder', 1),
(19, 'Mensaje IVR', 1),
(20, 'Linktree', 1);
SET IDENTITY_INSERT PCN OFF;

-- =====================================================
-- MENUS
-- =====================================================
SET IDENTITY_INSERT Menus ON;
-- Menú Administrador (RolId = 1)
INSERT INTO Menus (Id, Nombre, Icono, Ruta, RolId) VALUES
(1, 'Home', 'lni lni-home', '/Home/Index', 1),
(2, 'Briefs', 'lni lni-files', '/Brief/Index', 1),
(3, 'Calendario', 'lni lni-calendar', '/Calendario/Index', 1),
(4, 'Materiales', 'lni lni-gallery', '/Materiales/Index', 1),
(5, 'Alertas', 'lni lni-alarm', '/Alertas/Index', 1),
(6, 'Participantes', 'lni lni-users', '/Usuarios/Index', 1),
(7, 'Invitaciones', 'lni lni-envelope', '/Invitaciones/Index', 1),
(8, 'Catálogos', 'lni lni-list', '/Catalogos/Index', 1);

-- Menú Usuario (RolId = 2)
INSERT INTO Menus (Id, Nombre, Icono, Ruta, RolId) VALUES
(9, 'Home', 'lni lni-home', '/Home/Index', 2),
(10, 'Briefs', 'lni lni-files', '/Brief/Index', 2),
(11, 'Materiales', 'lni lni-gallery', '/Materiales/Index', 2),
(12, 'Calendario', 'lni lni-calendar', '/Calendario/Index', 2),
(13, 'Alertas', 'lni lni-alarm', '/Alertas/Index', 2);

-- Menú Producción (RolId = 3)
INSERT INTO Menus (Id, Nombre, Icono, Ruta, RolId) VALUES
(14, 'Home', 'lni lni-home', '/Home/Index', 3),
(15, 'Materiales', 'lni lni-gallery', '/Materiales/Index', 3),
(16, 'Calendario', 'lni lni-calendar', '/Calendario/Index', 3),
(17, 'Alertas', 'lni lni-alarm', '/Alertas/Index', 3);
SET IDENTITY_INSERT Menus OFF;

-- =====================================================
-- USUARIO ADMINISTRADOR
-- =====================================================
SET IDENTITY_INSERT Usuarios ON;
INSERT INTO Usuarios (Id, Nombre, ApellidoPaterno, ApellidoMaterno, Correo, Contrasena, RolId, Estatus, CambioContrasena) VALUES
(1, 'Admin', 'Sistema', 'Test', 'ajcortest@gmail.com', 'Operaciones.2025', 1, 1, 0);
SET IDENTITY_INSERT Usuarios OFF;

-- =====================================================
-- FIN DEL SEED
-- =====================================================
PRINT 'Seed completado exitosamente';
PRINT 'Usuario administrador: ajcortest@gmail.com';
PRINT 'Contraseña: Operaciones.2025';
