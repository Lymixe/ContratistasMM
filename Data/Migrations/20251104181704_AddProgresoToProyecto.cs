using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContratistasMM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProgresoToProyecto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Progreso",
                table: "Proyectos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progreso",
                table: "Proyectos");
        }
    }
}
