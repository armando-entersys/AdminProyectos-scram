-- ================================================
-- SCRIPT DE MIGRACIÓN DE DATOS
-- SQL Server -> PostgreSQL
-- AdminProyectos Natura
-- ================================================
--
-- INSTRUCCIONES DE USO:
--
-- 1. EXPORTAR DATOS DE SQL SERVER:
--    Ejecutar este script en SQL Server para generar los INSERTs
--
-- 2. IMPORTAR A POSTGRESQL:
--    Ejecutar los INSERT generados en PostgreSQL
--
-- NOTA: Este script debe ejecutarse en el servidor de producción
--       donde está corriendo SQL Server actualmente
-- ================================================

-- ================================================
-- PASO 1: EXPORTAR DATOS DE CATÁLOGOS
-- ================================================

-- Tabla: Roles
SELECT 'INSERT INTO "Roles" ("Id", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Descripcion, '''', '''''') + ''');'
FROM AdminProyectos.dbo.Roles;

-- Tabla: TipoAlerta
SELECT 'INSERT INTO "TipoAlerta" ("Id", "Nombre", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Nombre, '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Descripcion, ''), '''', '''''') + ''');'
FROM AdminProyectos.dbo.TipoAlerta;

-- Tabla: TipoBrief
SELECT 'INSERT INTO "TipoBrief" ("Id", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Descripcion, '''', '''''') + ''');'
FROM AdminProyectos.dbo.TipoBrief;

-- Tabla: EstatusBrief
SELECT 'INSERT INTO "EstatusBrief" ("Id", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Descripcion, '''', '''''') + ''');'
FROM AdminProyectos.dbo.EstatusBrief;

-- Tabla: Prioridad
SELECT 'INSERT INTO "Prioridad" ("Id", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Descripcion, '''', '''''') + ''');'
FROM AdminProyectos.dbo.Prioridad;

-- Tabla: PCN
SELECT 'INSERT INTO "PCN" ("Id", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Descripcion, '''', '''''') + ''');'
FROM AdminProyectos.dbo.PCN;

-- Tabla: Audiencia
SELECT 'INSERT INTO "Audiencia" ("Id", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Descripcion, '''', '''''') + ''');'
FROM AdminProyectos.dbo.Audiencia;

-- Tabla: Formato
SELECT 'INSERT INTO "Formato" ("Id", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Descripcion, '''', '''''') + ''');'
FROM AdminProyectos.dbo.Formato;

-- Tabla: EstatusMateriales
SELECT 'INSERT INTO "EstatusMateriales" ("Id", "Descripcion") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Descripcion, '''', '''''') + ''');'
FROM AdminProyectos.dbo.EstatusMateriales;

-- ================================================
-- PASO 2: EXPORTAR DATOS DE USUARIOS
-- ================================================

SELECT 'INSERT INTO "Usuarios" ("Id", "Nombre", "ApellidoPaterno", "ApellidoMaterno", "Correo", "Contrasena", "RolId", "Estatus", "FechaRegistro", "FechaModificacion", "CambioContrasena", "SolicitudRegistro") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Nombre, '''', '''''') + ''', ''' +
       REPLACE(ISNULL(ApellidoPaterno, ''), '''', '''''') + ''', ''' +
       REPLACE(ISNULL(ApellidoMaterno, ''), '''', '''''') + ''', ''' +
       REPLACE(Correo, '''', '''''') + ''', ''' +
       REPLACE(Contrasena, '''', '''''') + ''', ' +
       CAST(RolId AS VARCHAR) + ', ' +
       CASE WHEN Estatus = 1 THEN 'true' ELSE 'false' END + ', ''' +
       CONVERT(VARCHAR, FechaRegistro, 120) + ''', ' +
       CASE WHEN FechaModificacion IS NULL THEN 'NULL'
            ELSE '''' + CONVERT(VARCHAR, FechaModificacion, 120) + '''' END + ', ' +
       CASE WHEN CambioContrasena = 1 THEN 'true' ELSE 'false' END + ', ' +
       CASE WHEN SolicitudRegistro = 1 THEN 'true' ELSE 'false' END + ');'
FROM AdminProyectos.dbo.Usuarios;

-- ================================================
-- PASO 3: EXPORTAR DATOS DE BRIEFS
-- ================================================

SELECT 'INSERT INTO "Briefs" ("Id", "Nombre", "Ciclo", "IdTipoBrief", "PCNId", "AudienciaId", "FechaPublicacion", "Marca", "Categoria", "Objetivo", "Entregable", "Periodo", "Alcance", "IdUsuarioCreacion", "FechaCreacion", "IdUsuarioModificacion", "FechaModificacion", "BreafCreador", "IdEstatusBrief", "IdPrioridad") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Nombre, '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Ciclo, ''), '''', '''''') + ''', ' +
       CAST(ISNULL(IdTipoBrief, 0) AS VARCHAR) + ', ' +
       CAST(ISNULL(PCNId, 0) AS VARCHAR) + ', ' +
       CAST(ISNULL(AudienciaId, 0) AS VARCHAR) + ', ' +
       CASE WHEN FechaPublicacion IS NULL THEN 'NULL'
            ELSE '''' + CONVERT(VARCHAR, FechaPublicacion, 120) + '''' END + ', ''' +
       REPLACE(ISNULL(Marca, ''), '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Categoria, ''), '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Objetivo, ''), '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Entregable, ''), '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Periodo, ''), '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Alcance, ''), '''', '''''') + ''', ' +
       CAST(IdUsuarioCreacion AS VARCHAR) + ', ''' +
       CONVERT(VARCHAR, FechaCreacion, 120) + ''', ' +
       CAST(ISNULL(IdUsuarioModificacion, 0) AS VARCHAR) + ', ' +
       CASE WHEN FechaModificacion IS NULL THEN 'NULL'
            ELSE '''' + CONVERT(VARCHAR, FechaModificacion, 120) + '''' END + ', ''' +
       REPLACE(ISNULL(BreafCreador, ''), '''', '''''') + ''', ' +
       CAST(IdEstatusBrief AS VARCHAR) + ', ' +
       CAST(IdPrioridad AS VARCHAR) + ');'
FROM AdminProyectos.dbo.Briefs;

-- ================================================
-- PASO 4: EXPORTAR PARTICIPANTES
-- ================================================

SELECT 'INSERT INTO "Participantes" ("Id", "UsuarioId", "BriefId") VALUES (' +
       CAST(Id AS VARCHAR) + ', ' +
       CAST(UsuarioId AS VARCHAR) + ', ' +
       CAST(BriefId AS VARCHAR) + ');'
FROM AdminProyectos.dbo.Participantes;

-- ================================================
-- PASO 5: EXPORTAR MATERIALES
-- ================================================

SELECT 'INSERT INTO "Materiales" ("Id", "Nombre", "Descripcion", "ArchivoURL", "FechaCreacion", "FechaModificacion", "IdUsuarioCreacion", "BriefId", "FormatoId", "IdEstatusMaterial") VALUES (' +
       CAST(Id AS VARCHAR) + ', ''' +
       REPLACE(Nombre, '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Descripcion, ''), '''', '''''') + ''', ''' +
       REPLACE(ISNULL(ArchivoURL, ''), '''', '''''') + ''', ''' +
       CONVERT(VARCHAR, FechaCreacion, 120) + ''', ' +
       CASE WHEN FechaModificacion IS NULL THEN 'NULL'
            ELSE '''' + CONVERT(VARCHAR, FechaModificacion, 120) + '''' END + ', ' +
       CAST(IdUsuarioCreacion AS VARCHAR) + ', ' +
       CAST(BriefId AS VARCHAR) + ', ' +
       CAST(ISNULL(FormatoId, 0) AS VARCHAR) + ', ' +
       CAST(IdEstatusMaterial AS VARCHAR) + ');'
FROM AdminProyectos.dbo.Materiales;

-- ================================================
-- PASO 6: EXPORTAR RELACIÓN MATERIAL-PCN
-- ================================================

SELECT 'INSERT INTO "MaterialPCN" ("MaterialId", "PCNId") VALUES (' +
       CAST(MaterialId AS VARCHAR) + ', ' +
       CAST(PCNId AS VARCHAR) + ');'
FROM AdminProyectos.dbo.MaterialPCN;

-- ================================================
-- PASO 7: EXPORTAR ALERTAS
-- ================================================

SELECT 'INSERT INTO "Alertas" ("Id", "IdUsuario", "Nombre", "Descripcion", "lectura", "Accion", "FechaCreacion", "IdTipoAlerta") VALUES (' +
       CAST(Id AS VARCHAR) + ', ' +
       CAST(IdUsuario AS VARCHAR) + ', ''' +
       REPLACE(Nombre, '''', '''''') + ''', ''' +
       REPLACE(ISNULL(Descripcion, ''), '''', '''''') + ''', ' +
       CASE WHEN lectura = 1 THEN 'true' ELSE 'false' END + ', ''' +
       REPLACE(ISNULL(Accion, ''), '''', '''''') + ''', ''' +
       CONVERT(VARCHAR, FechaCreacion, 120) + ''', ' +
       CAST(IdTipoAlerta AS VARCHAR) + ');'
FROM AdminProyectos.dbo.Alertas;

-- ================================================
-- PASO 8: EXPORTAR COMENTARIOS
-- ================================================

SELECT 'INSERT INTO "Comentarios" ("Id", "IdUsuario", "IdMaterial", "Descripcion", "FechaCreacion", "Usuario") VALUES (' +
       CAST(Id AS VARCHAR) + ', ' +
       CAST(IdUsuario AS VARCHAR) + ', ' +
       CAST(IdMaterial AS VARCHAR) + ', ''' +
       REPLACE(ISNULL(Descripcion, ''), '''', '''''') + ''', ''' +
       CONVERT(VARCHAR, FechaCreacion, 120) + ''', ''' +
       REPLACE(ISNULL(Usuario, ''), '''', '''''') + ''');'
FROM AdminProyectos.dbo.Comentarios;

-- ================================================
-- PASO 9: AJUSTAR SECUENCIAS EN POSTGRESQL
-- ================================================
-- Después de importar todos los datos, ejecutar en PostgreSQL:
/*
SELECT setval('"Roles_Id_seq"', (SELECT MAX("Id") FROM "Roles"));
SELECT setval('"TipoAlerta_Id_seq"', (SELECT MAX("Id") FROM "TipoAlerta"));
SELECT setval('"TipoBrief_Id_seq"', (SELECT MAX("Id") FROM "TipoBrief"));
SELECT setval('"EstatusBrief_Id_seq"', (SELECT MAX("Id") FROM "EstatusBrief"));
SELECT setval('"Prioridad_Id_seq"', (SELECT MAX("Id") FROM "Prioridad"));
SELECT setval('"PCN_Id_seq"', (SELECT MAX("Id") FROM "PCN"));
SELECT setval('"Audiencia_Id_seq"', (SELECT MAX("Id") FROM "Audiencia"));
SELECT setval('"Formato_Id_seq"', (SELECT MAX("Id") FROM "Formato"));
SELECT setval('"EstatusMateriales_Id_seq"', (SELECT MAX("Id") FROM "EstatusMateriales"));
SELECT setval('"Usuarios_Id_seq"', (SELECT MAX("Id") FROM "Usuarios"));
SELECT setval('"Briefs_Id_seq"', (SELECT MAX("Id") FROM "Briefs"));
SELECT setval('"Participantes_Id_seq"', (SELECT MAX("Id") FROM "Participantes"));
SELECT setval('"Materiales_Id_seq"', (SELECT MAX("Id") FROM "Materiales"));
SELECT setval('"Alertas_Id_seq"', (SELECT MAX("Id") FROM "Alertas"));
SELECT setval('"Comentarios_Id_seq"', (SELECT MAX("Id") FROM "Comentarios"));
*/
