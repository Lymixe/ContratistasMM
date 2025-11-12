using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContratistasMM.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonalManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCompleto = table.Column<string>(type: "text", nullable: false),
                    Cargo = table.Column<string>(type: "text", nullable: false),
                    FotoUrl = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HitoPersonal",
                columns: table => new
                {
                    HitosAsignadosId = table.Column<int>(type: "integer", nullable: false),
                    PersonalAsignadoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HitoPersonal", x => new { x.HitosAsignadosId, x.PersonalAsignadoId });
                    table.ForeignKey(
                        name: "FK_HitoPersonal_Hitos_HitosAsignadosId",
                        column: x => x.HitosAsignadosId,
                        principalTable: "Hitos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HitoPersonal_Personal_PersonalAsignadoId",
                        column: x => x.PersonalAsignadoId,
                        principalTable: "Personal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HitoPersonal_PersonalAsignadoId",
                table: "HitoPersonal",
                column: "PersonalAsignadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HitoPersonal");

            migrationBuilder.DropTable(
                name: "Personal");
        }
    }
}
