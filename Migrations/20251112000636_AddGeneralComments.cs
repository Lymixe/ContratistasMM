using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContratistasMM.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Hitos_HitoId",
                table: "Comentarios");

            migrationBuilder.AlterColumn<int>(
                name: "HitoId",
                table: "Comentarios",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Contenido",
                table: "Comentarios",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Hitos_HitoId",
                table: "Comentarios",
                column: "HitoId",
                principalTable: "Hitos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Hitos_HitoId",
                table: "Comentarios");

            migrationBuilder.AlterColumn<int>(
                name: "HitoId",
                table: "Comentarios",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Contenido",
                table: "Comentarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Hitos_HitoId",
                table: "Comentarios",
                column: "HitoId",
                principalTable: "Hitos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
