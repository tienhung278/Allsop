using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Allsop.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVoucher2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AmountBought",
                table: "Vouchers",
                newName: "SpentAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpentAmount",
                table: "Vouchers",
                newName: "AmountBought");
        }
    }
}
