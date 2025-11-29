using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    /// <inheritdoc />
    public partial class LastUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Balance column to Wallet
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Wallet",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            // Remove Type column from Notification
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notification");

            // Change default value of IsDefaultWallet (true → false)
            migrationBuilder.AlterColumn<bool>(
                name: "IsDefaultWallet",
                table: "Wallet",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove Balance column
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Wallet");

            // Restore Type column
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Notification",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            // Restore default value of IsDefaultWallet (false → true)
            migrationBuilder.AlterColumn<bool>(
                name: "IsDefaultWallet",
                table: "Wallet",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
