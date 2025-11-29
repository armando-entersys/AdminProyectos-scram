using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AddObjetivoNegocioToBrief : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materiales_Audiencia_AudienciaId",
                table: "Materiales");

            migrationBuilder.DropIndex(
                name: "IX_Materiales_AudienciaId",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "AudienciaId",
                table: "Materiales");

            migrationBuilder.AddColumn<string>(
                name: "ObjetivoNegocio",
                table: "Briefs",
                type: "text",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_MaterialAudiencia_AudienciaId",
                table: "MaterialAudiencia",
                column: "AudienciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialAudiencia");

            migrationBuilder.DropColumn(
                name: "ObjetivoNegocio",
                table: "Briefs");

            migrationBuilder.AddColumn<int>(
                name: "AudienciaId",
                table: "Materiales",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_AudienciaId",
                table: "Materiales",
                column: "AudienciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materiales_Audiencia_AudienciaId",
                table: "Materiales",
                column: "AudienciaId",
                principalTable: "Audiencia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
