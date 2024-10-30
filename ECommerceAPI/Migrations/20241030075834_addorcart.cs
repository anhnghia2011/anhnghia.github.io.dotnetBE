using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class addorcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "Size",
                table: "CartItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems");

            migrationBuilder.AlterColumn<string>(
                name: "Size",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
