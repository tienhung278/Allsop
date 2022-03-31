using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Allsop.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "Vouchers",
                newName: "PercentageOff");

            migrationBuilder.RenameColumn(
                name: "ExpiredAt",
                table: "Vouchers",
                newName: "AmountOff");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Vouchers",
                newName: "AmountBought");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PercentageOff",
                table: "Vouchers",
                newName: "Percentage");

            migrationBuilder.RenameColumn(
                name: "AmountOff",
                table: "Vouchers",
                newName: "ExpiredAt");

            migrationBuilder.RenameColumn(
                name: "AmountBought",
                table: "Vouchers",
                newName: "Amount");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "OrderDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
