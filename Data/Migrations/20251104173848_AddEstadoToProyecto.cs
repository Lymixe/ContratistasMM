using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContratistasMM.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEstadoToProyecto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Proyectos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Proyectos");
        }
    }
}
