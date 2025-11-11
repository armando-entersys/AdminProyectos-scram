# Guía de Migración: SQL Server → PostgreSQL

Esta guía detalla el proceso completo de migración de la base de datos AdminProyectos de SQL Server a PostgreSQL.

## 📋 Requisitos Previos

- Docker y Docker Compose instalados
- Acceso al servidor de producción con SQL Server actual
- Backup completo de la base de datos SQL Server
- Permisos para ejecutar scripts en ambas bases de datos

## 🔄 Proceso de Migración

### Fase 1: Preparación Local (Desarrollo)

#### 1.1 Cambiar a la rama de migración
```bash
git checkout postgresql-migration
```

#### 1.2 Revisar los cambios realizados
Los siguientes archivos han sido modificados para soportar PostgreSQL:

- ✅ `docker-compose.yml` - Reemplaza SQL Server con PostgreSQL
- ✅ `DataAccessLayer/DataAccessLayer.csproj` - Usa Npgsql en lugar de SqlServer
- ✅ `PresentationLayer/Program.cs` - Configurado para UseNpgsql
- ✅ `PresentationLayer/appsettings.json` - Connection string de PostgreSQL
- ✅ `DataAccessLayer/Migrations/` - Nuevas migraciones para PostgreSQL

#### 1.3 Probar localmente con base de datos vacía

```bash
# Levantar contenedores
docker-compose up -d

# Aplicar migraciones
dotnet ef database update --project DataAccessLayer --startup-project PresentationLayer

# Verificar que la aplicación funciona
# Abrir: http://localhost:8080
```

### Fase 2: Exportación de Datos (Producción)

#### 2.1 Conectarse al servidor de producción

```bash
gcloud compute ssh dev-server --zone=us-central1-c
cd /srv/servicios/natura-adminproyectos
```

#### 2.2 Crear backup de SQL Server

```bash
# Crear directorio para backups si no existe
mkdir -p backups

# Backup de la base de datos
docker exec adminproyectos-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Natura2024$' -C \
  -Q "BACKUP DATABASE AdminProyectos TO DISK = '/var/opt/mssql/backups/AdminProyectos_$(date +%Y%m%d_%H%M%S).bak'"
```

#### 2.3 Exportar datos usando el script

```bash
# Copiar el script de migración al servidor
# Desde tu máquina local:
scp migration-scripts/migrate-data.sql dev-server:/tmp/

# En el servidor, ejecutar el script de exportación
docker exec adminproyectos-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Natura2024$' -C \
  -i /tmp/migrate-data.sql \
  -o /tmp/migration-inserts.sql
```

### Fase 3: Preparación de PostgreSQL (Producción)

#### 3.1 Detener contenedores actuales

```bash
cd /srv/servicios/natura-adminproyectos
docker-compose down
```

#### 3.2 Hacer pull de la rama postgresql-migration

```bash
git fetch origin
git checkout postgresql-migration
git pull origin postgresql-migration
```

#### 3.3 Levantar PostgreSQL

```bash
# Solo levantar PostgreSQL primero
docker-compose up -d adminproyectos-postgres

# Esperar a que PostgreSQL esté listo
docker-compose logs -f adminproyectos-postgres
# Esperar mensaje: "database system is ready to accept connections"
```

#### 3.4 Aplicar migraciones

```bash
# Desde el directorio del proyecto
docker-compose exec adminproyectos-web \
  dotnet ef database update --project DataAccessLayer --startup-project PresentationLayer
```

### Fase 4: Importación de Datos

#### 4.1 Importar datos a PostgreSQL

```bash
# Copiar archivo de inserts al contenedor de PostgreSQL
docker cp /tmp/migration-inserts.sql adminproyectos-postgres:/tmp/

# Ejecutar los inserts en PostgreSQL
docker exec -i adminproyectos-postgres psql \
  -U adminuser -d AdminProyectosNaturaDB \
  -f /tmp/migration-inserts.sql
```

#### 4.2 Ajustar secuencias de IDs

```bash
docker exec -i adminproyectos-postgres psql -U adminuser -d AdminProyectosNaturaDB <<EOF
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
EOF
```

### Fase 5: Verificación y Pruebas

#### 5.1 Verificar conteo de registros

```bash
docker exec -i adminproyectos-postgres psql -U adminuser -d AdminProyectosNaturaDB <<EOF
SELECT 'Usuarios' AS Tabla, COUNT(*) AS Total FROM "Usuarios"
UNION ALL SELECT 'Briefs', COUNT(*) FROM "Briefs"
UNION ALL SELECT 'Materiales', COUNT(*) FROM "Materiales"
UNION ALL SELECT 'Participantes', COUNT(*) FROM "Participantes"
UNION ALL SELECT 'Alertas', COUNT(*) FROM "Alertas"
UNION ALL SELECT 'Comentarios', COUNT(*) FROM "Comentarios";
EOF
```

Comparar estos números con los de SQL Server:

```bash
docker exec adminproyectos-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Natura2024$' -C -Q "
SELECT 'Usuarios' AS Tabla, COUNT(*) AS Total FROM AdminProyectos.dbo.Usuarios
UNION ALL SELECT 'Briefs', COUNT(*) FROM AdminProyectos.dbo.Briefs
UNION ALL SELECT 'Materiales', COUNT(*) FROM AdminProyectos.dbo.Materiales
UNION ALL SELECT 'Participantes', COUNT(*) FROM AdminProyectos.dbo.Participantes
UNION ALL SELECT 'Alertas', COUNT(*) FROM AdminProyectos.dbo.Alertas
UNION ALL SELECT 'Comentarios', COUNT(*) FROM AdminProyectos.dbo.Comentarios
"
```

#### 5.2 Levantar la aplicación completa

```bash
docker-compose up -d
```

#### 5.3 Verificar logs de la aplicación

```bash
docker-compose logs -f adminproyectos-web
```

#### 5.4 Probar funcionalidad

1. Abrir https://adminproyectos.entersys.mx
2. Iniciar sesión con credenciales existentes
3. Verificar que se cargan correctamente:
   - Dashboard
   - Lista de briefs
   - Materiales
   - Alertas
4. Crear un brief de prueba
5. Agregar participantes
6. Crear un material
7. Verificar que todo funciona correctamente

### Fase 6: Rollback (Si es necesario)

Si algo sale mal, puedes volver a SQL Server:

```bash
# Detener contenedores
docker-compose down

# Volver a la rama master
git checkout master
git pull origin master

# Levantar con SQL Server
docker-compose up -d
```

## 📊 Comparación de Recursos

| Métrica | SQL Server | PostgreSQL | Ahorro |
|---------|-----------|------------|--------|
| Memoria RAM | ~800 MB | ~400 MB | ~50% |
| Tamaño contenedor | ~1.5 GB | ~200 MB | ~87% |
| Licencia producción | $209+/mes | $0 | 100% |

## ⚠️ Notas Importantes

1. **Archivos subidos**: Los archivos en `/app/wwwroot/uploads` se mantienen en el volumen `uploads-data` y no se ven afectados por la migración.

2. **Contraseñas**: Las contraseñas ya están hasheadas en la base de datos, por lo que no hay problemas de seguridad al migrarlas.

3. **Conexión externa**: Si necesitas conectarte a PostgreSQL desde fuera del contenedor:
   - Puerto: 5432
   - Host: localhost (desarrollo) o IP del servidor (producción)
   - Usuario: adminuser
   - Contraseña: Operaciones.2025
   - Base de datos: AdminProyectosNaturaDB

4. **Downtime**: Se estima un downtime de aproximadamente 10-15 minutos durante la migración.

## 🔧 Troubleshooting

### Error: "relation does not exist"
```bash
# Verificar que las migraciones se aplicaron
docker-compose exec adminproyectos-web \
  dotnet ef migrations list --project DataAccessLayer --startup-project PresentationLayer
```

### Error: "could not connect to server"
```bash
# Verificar que PostgreSQL está corriendo
docker-compose ps
docker-compose logs adminproyectos-postgres
```

### Error de secuencias (duplicate key)
```bash
# Re-ejecutar el ajuste de secuencias (Fase 4.2)
```

## 📞 Soporte

En caso de problemas durante la migración, contactar al equipo de desarrollo con:
- Logs completos: `docker-compose logs > migration-logs.txt`
- Estado de contenedores: `docker-compose ps`
- Versión de la aplicación: `git log -1 --oneline`
