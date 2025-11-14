using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePuropseTypetoEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Purpos",
                table: "Beneficiary ",
                type: "character varying",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Purpos",
                table: "Beneficiary ",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying",
                oldNullable: true);
        }
    }
}
