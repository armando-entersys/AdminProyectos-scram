using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class SeedCatalogosIniciales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { 3, true, "Campaña" }
                });

            migrationBuilder.InsertData(
                table: "TiposBrief",
                columns: new[] { "Id", "Activo", "Descripcion" },
                values: new object[] { 4, true, "Evento" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Audiencia",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Audiencia",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Audiencia",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Audiencia",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EstatusBriefs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstatusBriefs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstatusBriefs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EstatusBriefs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EstatusBriefs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EstatusBriefs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EstatusBriefs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "EstatusBriefs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "EstatusMateriales",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstatusMateriales",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstatusMateriales",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EstatusMateriales",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EstatusMateriales",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EstatusMateriales",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Formato",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Formato",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Formato",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Formato",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Formato",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PCN",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PCN",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PCN",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PCN",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Prioridad",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Prioridad",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Prioridad",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Prioridad",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TipoAlerta",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoAlerta",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TipoAlerta",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TipoAlerta",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TipoAlerta",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TiposBrief",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposBrief",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposBrief",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposBrief",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
