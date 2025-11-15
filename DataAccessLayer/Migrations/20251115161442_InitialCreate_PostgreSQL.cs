using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class InitialCreate_PostgreSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audiencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audiencia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstatusBriefs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusBriefs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstatusMateriales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstatusMateriales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Formato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formato", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PCN",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCN", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prioridad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prioridad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoAlerta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoAlerta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposBrief",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposBrief", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Ruta = table.Column<string>(type: "text", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Icono = table.Column<string>(type: "text", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "text", nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "text", nullable: false),
                    Correo = table.Column<string>(type: "text", nullable: false),
                    Contrasena = table.Column<string>(type: "text", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    Estatus = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CambioContrasena = table.Column<bool>(type: "boolean", nullable: false),
                    SolicitudRegistro = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    lectura = table.Column<bool>(type: "boolean", nullable: false),
                    Accion = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdTipoAlerta = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_TipoAlerta_IdTipoAlerta",
                        column: x => x.IdTipoAlerta,
                        principalTable: "TipoAlerta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Briefs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Objetivo = table.Column<string>(type: "text", nullable: true),
                    DirigidoA = table.Column<string>(type: "text", nullable: false),
                    Comentario = table.Column<string>(type: "text", nullable: false),
                    RutaArchivo = table.Column<string>(type: "text", nullable: true),
                    LinksReferencias = table.Column<string>(type: "text", nullable: true),
                    PlanComunicacion = table.Column<bool>(type: "boolean", nullable: false),
                    DeterminarEstadoEstadoId = table.Column<int>(type: "integer", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstatusBriefId = table.Column<int>(type: "integer", nullable: false),
                    TipoBriefId = table.Column<int>(type: "integer", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Briefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Briefs_EstatusBriefs_EstatusBriefId",
                        column: x => x.EstatusBriefId,
                        principalTable: "EstatusBriefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Briefs_TiposBrief_TipoBriefId",
                        column: x => x.TipoBriefId,
                        principalTable: "TiposBrief",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Briefs_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materiales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Mensaje = table.Column<string>(type: "text", nullable: false),
                    PrioridadId = table.Column<int>(type: "integer", nullable: false),
                    Ciclo = table.Column<string>(type: "text", nullable: false),
                    FormatoId = table.Column<int>(type: "integer", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaPublicacionLiberada = table.Column<bool>(type: "boolean", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: false),
                    Area = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BriefId = table.Column<int>(type: "integer", nullable: false),
                    EstatusMaterialId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materiales_Briefs_BriefId",
                        column: x => x.BriefId,
                        principalTable: "Briefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materiales_EstatusMateriales_EstatusMaterialId",
                        column: x => x.EstatusMaterialId,
                        principalTable: "EstatusMateriales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materiales_Formato_FormatoId",
                        column: x => x.FormatoId,
                        principalTable: "Formato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Materiales_Prioridad_PrioridadId",
                        column: x => x.PrioridadId,
                        principalTable: "Prioridad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BriefId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participantes_Briefs_BriefId",
                        column: x => x.BriefId,
                        principalTable: "Briefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BriefId = table.Column<int>(type: "integer", nullable: false),
                    Comentario = table.Column<string>(type: "text", nullable: false),
                    RequierePlan = table.Column<bool>(type: "boolean", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proyectos_Briefs_BriefId",
                        column: x => x.BriefId,
                        principalTable: "Briefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialMateriales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Comentarios = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaPublicacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaPublicacionLiberada = table.Column<bool>(type: "boolean", nullable: false),
                    EstatusMaterialId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    MaterialId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialMateriales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialMateriales_Materiales_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialMateriales_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialAudiencia",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    AudienciaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialAudiencia", x => new { x.MaterialId, x.AudienciaId });
                    table.ForeignKey(
                        name: "FK_MaterialAudiencia_Audiencia_AudienciaId",
                        column: x => x.AudienciaId,
                        principalTable: "Audiencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialAudiencia_Materiales_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialPCN",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "integer", nullable: false),
                    PCNId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialPCN", x => new { x.MaterialId, x.PCNId });
                    table.ForeignKey(
                        name: "FK_MaterialPCN_Materiales_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialPCN_PCN_PCNId",
                        column: x => x.PCNId,
                        principalTable: "PCN",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RetrasoMateriales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MotivoId = table.Column<int>(type: "integer", nullable: false),
                    Comentario = table.Column<string>(type: "text", nullable: false),
                    MaterialId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetrasoMateriales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetrasoMateriales_Materiales_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Audiencia",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[,]
                {
                    { 1, true, "Público General" },
                    { 2, true, "Consultoras" },
                    { 3, true, "Colaboradores" },
                    { 4, true, "Medios" }
                });

            migrationBuilder.InsertData(
                table: "EstatusBriefs",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[,]
                {
                    { 1, true, "En Revisión" },
                    { 2, true, "Aprobado" },
                    { 3, true, "Rechazado" },
                    { 4, true, "Programado" },
                    { 5, true, "Entregado" },
                    { 6, true, "Finalizado" },
                    { 7, true, "En Pausa" },
                    { 8, true, "Cancelado" }
                });

            migrationBuilder.InsertData(
                table: "EstatusMateriales",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[,]
                {
                    { 1, true, "En Revisión" },
                    { 2, true, "Falta Información" },
                    { 3, true, "Aprobado" },
                    { 4, true, "Programado" },
                    { 5, true, "Entregado" },
                    { 6, true, "Inicio de Ciclo" }
                });

            migrationBuilder.InsertData(
                table: "Formato",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[,]
                {
                    { 1, true, "Digital" },
                    { 2, true, "Impreso" },
                    { 3, true, "Video" },
                    { 4, true, "Audio" },
                    { 5, true, "Redes Sociales" }
                });

            migrationBuilder.InsertData(
                table: "PCN",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[,]
                {
                    { 1, true, "Lanzamiento" },
                    { 2, true, "Continuidad" },
                    { 3, true, "Promoción" },
                    { 4, true, "Institucional" }
                });

            migrationBuilder.InsertData(
                table: "Prioridad",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[,]
                {
                    { 1, true, "Baja" },
                    { 2, true, "Media" },
                    { 3, true, "Alta" },
                    { 4, true, "Urgente" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Solicitante" },
                    { 3, "Productor" }
                });

            migrationBuilder.InsertData(
                table: "TipoAlerta",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[,]
                {
                    { 1, true, "Nuevo Proyecto" },
                    { 2, true, "Cambio de Estado" },
                    { 3, true, "Actualización" },
                    { 4, true, "Nuevo Material" },
                    { 5, true, "Material Entregado" }
                });

            migrationBuilder.InsertData(
                table: "TiposBrief",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[,]
                {
                    { 1, true, "Plan de Comunicación" },
                    { 2, true, "Materiales" },
                    { 3, true, "Campaña" },
                    { 4, true, "Evento" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_IdTipoAlerta",
                table: "Alertas",
                column: "IdTipoAlerta");

            migrationBuilder.CreateIndex(
                name: "IX_Briefs_EstatusBriefId",
                table: "Briefs",
                column: "EstatusBriefId");

            migrationBuilder.CreateIndex(
                name: "IX_Briefs_TipoBriefId",
                table: "Briefs",
                column: "TipoBriefId");

            migrationBuilder.CreateIndex(
                name: "IX_Briefs_UsuarioId",
                table: "Briefs",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMateriales_MaterialId",
                table: "HistorialMateriales",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialMateriales_UsuarioId",
                table: "HistorialMateriales",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAudiencia_AudienciaId",
                table: "MaterialAudiencia",
                column: "AudienciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_BriefId",
                table: "Materiales",
                column: "BriefId");

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_EstatusMaterialId",
                table: "Materiales",
                column: "EstatusMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_FormatoId",
                table: "Materiales",
                column: "FormatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_PrioridadId",
                table: "Materiales",
                column: "PrioridadId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialPCN_PCNId",
                table: "MaterialPCN",
                column: "PCNId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RolId",
                table: "Menus",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_BriefId",
                table: "Participantes",
                column: "BriefId");

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_UsuarioId",
                table: "Participantes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_BriefId",
                table: "Proyectos",
                column: "BriefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RetrasoMateriales_MaterialId",
                table: "RetrasoMateriales",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "HistorialMateriales");

            migrationBuilder.DropTable(
                name: "MaterialAudiencia");

            migrationBuilder.DropTable(
                name: "MaterialPCN");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Participantes");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "RetrasoMateriales");

            migrationBuilder.DropTable(
                name: "TipoAlerta");

            migrationBuilder.DropTable(
                name: "Audiencia");

            migrationBuilder.DropTable(
                name: "PCN");

            migrationBuilder.DropTable(
                name: "Materiales");

            migrationBuilder.DropTable(
                name: "Briefs");

            migrationBuilder.DropTable(
                name: "EstatusMateriales");

            migrationBuilder.DropTable(
                name: "Formato");

            migrationBuilder.DropTable(
                name: "Prioridad");

            migrationBuilder.DropTable(
                name: "EstatusBriefs");

            migrationBuilder.DropTable(
                name: "TiposBrief");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
