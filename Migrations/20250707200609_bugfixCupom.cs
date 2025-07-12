using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotemPWA.Migrations
{
    /// <inheritdoc />
    public partial class bugfixCupom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItensCombos_CupomId",
                table: "ItensCombos");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCombos_CupomId",
                table: "ItensCombos",
                column: "CupomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItensCombos_CupomId",
                table: "ItensCombos");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCombos_CupomId",
                table: "ItensCombos",
                column: "CupomId",
                unique: true);
        }
    }
}
