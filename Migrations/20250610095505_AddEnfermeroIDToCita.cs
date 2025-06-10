using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto.Migrations
{
    /// <inheritdoc />
    public partial class AddEnfermeroIDToCita : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnfermeroID",
                table: "Citas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_EnfermeroID",
                table: "Citas",
                column: "EnfermeroID");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Enfermeros_EnfermeroID",
                table: "Citas",
                column: "EnfermeroID",
                principalTable: "Enfermeros",
                principalColumn: "EnfermeroID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Enfermeros_EnfermeroID",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_EnfermeroID",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "EnfermeroID",
                table: "Citas");
        }
    }
}
