using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommece.EF.Migrations
{
    /// <inheritdoc />
    public partial class order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductItemId",
                table: "OrderItems",
                newName: "ProductVariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "OrderItems",
                newName: "ProductItemId");
        }
    }
}
