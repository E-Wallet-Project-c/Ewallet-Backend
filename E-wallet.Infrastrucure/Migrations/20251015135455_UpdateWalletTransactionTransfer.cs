using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWalletTransactionTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ballance",
                table: "Wallet");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultWallet",
                table: "Wallet",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Transfer",
                type: "character varying",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Transfer",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Transaction",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Transaction",
                type: "character varying",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefaultWallet",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Transaction");

            migrationBuilder.AddColumn<double>(
                name: "Ballance",
                table: "Wallet",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Transfer",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Transfer",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Transaction",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying");
        }
    }
}
