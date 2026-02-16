using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cases_CaseFileNumber",
                table: "Cases",
                column: "CaseFileNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_IsDeleted_CreatedAt",
                table: "Cases",
                columns: new[] { "IsDeleted", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cases_CaseFileNumber",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_IsDeleted_CreatedAt",
                table: "Cases");
        }
    }
}
