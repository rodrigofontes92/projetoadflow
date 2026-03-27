using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddFuncionarioIdMateriais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FuncionarioId",
                table: "Materiais",
                type: "nvarchar(450)",
                /*
                 * Trocado nullable para true
                 * Isso evita futuros conflitos de FK e problemas com dados já existentes
                 */
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materiais_FuncionarioId",
                table: "Materiais",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materiais_AspNetUsers_FuncionarioId",
                table: "Materiais",
                column: "FuncionarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materiais_AspNetUsers_FuncionarioId",
                table: "Materiais");

            migrationBuilder.DropIndex(
                name: "IX_Materiais_FuncionarioId",
                table: "Materiais");

            migrationBuilder.DropColumn(
                name: "FuncionarioId",
                table: "Materiais");
        }
    }
}
