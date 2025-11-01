using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransferAmountAndRemoveUserBankAccountUserIdFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Transfer",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.DropForeignKey(
            name: "Fk-UserId",
            table: "UserBankAccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Transfer",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

               migrationBuilder.AddForeignKey(
               name: "Fk-UserId",
               table: "UserBankAccount",
               column: "UserId",
               principalTable: "User",
               principalColumn: "Id");
        }
    }
}
