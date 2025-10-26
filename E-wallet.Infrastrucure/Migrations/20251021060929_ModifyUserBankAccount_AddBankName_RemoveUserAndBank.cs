using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyUserBankAccount_AddBankName_RemoveUserAndBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
       
            migrationBuilder.DropForeignKey(
                name: "Fk-BankId",
                table: "UserBankAccount");

            migrationBuilder.DropTable(
                name: "VirtualBank");

            //migrationBuilder.DropIndex(
            //    name: "IX_UserBankAccount_BankId",
            //    table: "UserBankAccount");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserBankAccount",
                newName: "WalletId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_UserBankAccount_UserId",
            //    table: "UserBankAccount",
            //    newName: "IX_UserBankAccount_WalletId");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "UserBankAccount",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "Fk-WalletId",
                table: "UserBankAccount",
                column: "WalletId",
                principalTable: "Wallet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "UserBankAccount");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "UserBankAccount",
                newName: "UserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_UserBankAccount_WalletId",
            //    table: "UserBankAccount",
            //    newName: "IX_UserBankAccount_UserId");

            migrationBuilder.CreateTable(
                name: "VirtualBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    WalletId = table.Column<int>(type: "integer", nullable: false),
                    BankName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("VirtualBank_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "Fk-WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id");
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserBankAccount_BankId",
            //    table: "UserBankAccount",
            //    column: "BankId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_VirtualBank_WalletId",
            //    table: "VirtualBank",
            //    column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "Fk-BankId",
                table: "UserBankAccount",
                column: "BankId",
                principalTable: "VirtualBank",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk-UserId",
                table: "UserBankAccount",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
