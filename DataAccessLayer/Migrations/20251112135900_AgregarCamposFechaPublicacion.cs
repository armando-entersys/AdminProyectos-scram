using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AgregarCamposFechaPublicacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPublicacion",
                table: "HistorialMateriales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FechaPublicacionLiberada",
                table: "HistorialMateriales",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaPublicacion",
                table: "HistorialMateriales");

            migrationBuilder.DropColumn(
                name: "FechaPublicacionLiberada",
                table: "HistorialMateriales");
        }
    }
}
