-- =====================================================
-- SEED DATA - AdminProyectos Natura (PostgreSQL)
-- Versión Final para Producción
-- Generado: 2025-11-20
-- =====================================================

-- Limpiar todas las tablas en orden correcto
DELETE FROM "HistorialMateriales";
DELETE FROM "MaterialAudiencia";
DELETE FROM "MaterialPCN";
DELETE FROM "RetrasoMateriales";
DELETE FROM "Materiales";
DELETE FROM "Participantes";
DELETE FROM "Briefs";
DELETE FROM "Alertas";
DELETE FROM "Usuarios";
DELETE FROM "Menus";
DELETE FROM "PCN";
DELETE FROM "Audiencia";
DELETE FROM "Formato";
DELETE FROM "Prioridad";
DELETE FROM "TipoAlerta";
DELETE FROM "EstatusMateriales";
DELETE FROM "EstatusBriefs";
DELETE FROM "TiposBrief";
DELETE FROM "Roles";

-- =====================================================
-- ROLES
-- =====================================================
INSERT INTO "Roles" ("Id", "Descripcion") VALUES
(1, 'Administrador'),
(2, 'Usuario'),
(3, 'Producción');
SELECT setval('"Roles_Id_seq"', 3);

-- =====================================================
-- TIPOS DE BRIEF
-- =====================================================
INSERT INTO "TiposBrief" ("Id", "Descripcion", "Activo") VALUES
(1, 'Ciclal', true),
(2, 'Emergencial', true);
SELECT setval('"TiposBrief_Id_seq"', 2);

-- =====================================================
-- ESTATUS DE BRIEFS
-- =====================================================
INSERT INTO "EstatusBriefs" ("Id", "Descripcion", "Activo") VALUES
(1, 'En revisión', true),
(2, 'Producción', true),
(3, 'Falta información', true),
(4, 'Programado', true),
(6, 'Finalizado', true);
SELECT setval('"EstatusBriefs_Id_seq"', 6);

-- =====================================================
-- ESTATUS DE MATERIALES
-- =====================================================
INSERT INTO "EstatusMateriales" ("Id", "Descripcion", "Activo") VALUES
(1, 'Pendiente', true),
(2, 'En diseño', true),
(3, 'En revisión', true),
(4, 'Listo para publicación', true),
(5, 'En producción', true),
(6, 'Entregado', false),
(7, 'Rechazado', true),
(8, 'No se compartio', true);
SELECT setval('"EstatusMateriales_Id_seq"', 8);

-- =====================================================
-- TIPOS DE ALERTA
-- =====================================================
INSERT INTO "TipoAlerta" ("Id", "Descripcion", "Activo") VALUES
(1, 'Información', true),
(2, 'Advertencia', true),
(3, 'Actualización', true),
(4, 'Notificación', true),
(5, 'Material Entregado', true);
SELECT setval('"TipoAlerta_Id_seq"', 5);

-- =====================================================
-- FORMATOS
-- =====================================================
INSERT INTO "Formato" ("Id", "Descripcion", "Activo") VALUES
(1, 'WhatsApp', true),
(2, 'Story', true),
(3, 'Video', true),
(4, 'Texto', true),
(5, 'Card', true),
(6, 'Banner home', true),
(7, 'Comunicado', true),
(8, 'Marco para story', true),
(9, 'Infografía', true),
(10, 'Albúm', true),
(11, 'Placa', true),
(12, 'Mailing', true),
(13, 'Guía interactiva', true),
(14, 'PDF', true),
(15, 'Reel', true),
(16, 'Video corto (vertical)', true),
(17, 'Historia destacada IG', true),
(18, 'Post', true),
(19, 'Carrusel', true),
(20, 'Placa animada', true),
(21, 'Impresos', true),
(22, 'Stickers', true),
(23, 'Diada', true),
(24, 'Pop up', true),
(25, 'Ícono interactivo', true),
(26, 'Banner', true),
(27, 'Linktree', true),
(28, 'PDF con link', true);
SELECT setval('"Formato_Id_seq"', 28);

-- =====================================================
-- AUDIENCIAS
-- =====================================================
INSERT INTO "Audiencia" ("Id", "Descripcion", "Activo") VALUES
(1, 'GV', true),
(2, 'GNs y Líderes', true),
(3, 'Base específica', true),
(4, 'Consultor', true),
(6, 'Zafiro y Diamante', true),
(7, 'Todo el canal', true),
(8, 'Activas', true),
(9, 'Disponibles', true),
(10, 'CF', true),
(11, 'INA 1 y 2', true),
(12, 'Zafiro, Oro y Diamante', true),
(13, 'GV1-2', true),
(14, 'Solo Avon', true),
(15, 'Diamantes', true),
(16, 'CND', true),
(17, 'GV3-16', true);
SELECT setval('"Audiencia_Id_seq"', 17);

-- =====================================================
-- PRIORIDADES
-- =====================================================
INSERT INTO "Prioridad" ("Id", "Descripcion", "Activo") VALUES
(1, 'Baja', true),
(2, 'Mediana', true),
(3, 'Grande', true),
(4, 'Urgente', true);
SELECT setval('"Prioridad_Id_seq"', 4);

-- =====================================================
-- PCN (Puntos de Contacto)
-- =====================================================
INSERT INTO "PCN" ("Id", "Descripcion", "Activo") VALUES
(1, 'Mi Negocio', true),
(2, 'Facebook - Consultoría de Belleza Natura y Avon', true),
(3, 'Instagram - Consultoría de Belleza Natura y Avon', true),
(4, 'WhatsApp Estrategia', true),
(5, 'WhatsApp GNs', true),
(6, 'SMS', true),
(7, 'Instagram - Natura México', true),
(8, 'Facebook - Natura México', true),
(9, 'WhatsApp Consultor', true),
(10, 'Mailing', true),
(11, 'Instagram - Avon México', true),
(12, 'Facebook - Avon México', true),
(13, 'Espacios tiendas Natura', true),
(14, 'Revista', true),
(15, 'Revista digital', true),
(16, 'Sitio web Natura CF', true),
(17, 'Canal YouTube Escuela Natura y Avon', true),
(18, 'WhatsApp Líder', true),
(19, 'Mensaje IVR', true),
(20, 'Linktree', true);
SELECT setval('"PCN_Id_seq"', 20);

-- =====================================================
-- MENUS
-- =====================================================
-- Menú Administrador (RolId = 1)
INSERT INTO "Menus" ("Id", "Nombre", "Icono", "Ruta", "Orden", "RolId") VALUES
(1, 'Home', 'lni lni-home', '/Home/Index', 1, 1),
(2, 'Briefs', 'lni lni-files', '/Brief/Index', 2, 1),
(3, 'Calendario', 'lni lni-calendar', '/Calendario/Index', 3, 1),
(4, 'Materiales', 'lni lni-gallery', '/Materiales/Index', 4, 1),
(5, 'Alertas', 'lni lni-alarm', '/Alertas/Index', 5, 1),
(6, 'Participantes', 'lni lni-users', '/Usuarios/Index', 6, 1),
(7, 'Invitaciones', 'lni lni-envelope', '/Invitaciones/Index', 7, 1),
(8, 'Catálogos', 'lni lni-list', '/Catalogos/Index', 8, 1);

-- Menú Usuario (RolId = 2)
INSERT INTO "Menus" ("Id", "Nombre", "Icono", "Ruta", "Orden", "RolId") VALUES
(9, 'Home', 'lni lni-home', '/Home/Index', 1, 2),
(10, 'Briefs', 'lni lni-files', '/Brief/Index', 2, 2),
(11, 'Materiales', 'lni lni-gallery', '/Materiales/Index', 3, 2),
(12, 'Calendario', 'lni lni-calendar', '/Calendario/Index', 4, 2),
(13, 'Alertas', 'lni lni-alarm', '/Alertas/Index', 5, 2);

-- Menú Producción (RolId = 3)
INSERT INTO "Menus" ("Id", "Nombre", "Icono", "Ruta", "Orden", "RolId") VALUES
(14, 'Home', 'lni lni-home', '/Home/Index', 1, 3),
(15, 'Materiales', 'lni lni-gallery', '/Materiales/Index', 2, 3),
(16, 'Calendario', 'lni lni-calendar', '/Calendario/Index', 3, 3),
(17, 'Alertas', 'lni lni-alarm', '/Alertas/Index', 4, 3);
SELECT setval('"Menus_Id_seq"', 17);

-- =====================================================
-- USUARIO ADMINISTRADOR
-- =====================================================
INSERT INTO "Usuarios" ("Id", "Nombre", "ApellidoPaterno", "ApellidoMaterno", "Correo", "Contrasena", "RolId", "Estatus", "FechaRegistro", "FechaModificacion", "CambioContrasena", "SolicitudRegistro") VALUES
(1, 'Admin', 'Sistema', 'Test', 'ajcortest@gmail.com', 'Operaciones.2025', 1, true, NOW(), NOW(), false, false);
SELECT setval('"Usuarios_Id_seq"', 1);

-- Reset sequences for empty tables
SELECT setval('"Briefs_Id_seq"', 1, false);
SELECT setval('"Materiales_Id_seq"', 1, false);
SELECT setval('"Alertas_Id_seq"', 1, false);

-- =====================================================
-- FIN DEL SEED
-- =====================================================
-- Usuario administrador: ajcortest@gmail.com
-- Contraseña: Operaciones.2025
