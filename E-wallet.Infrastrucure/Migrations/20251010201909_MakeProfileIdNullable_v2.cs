using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    /// <inheritdoc />
    public partial class MakeProfileIdNullable_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
             name: "ProfileId",
             table: "User",
             type: "integer",
             nullable: true,
             oldClrType: typeof(int),
             oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                          name: "ProfileId",
                          table: "User",
                          type: "integer",
                          nullable: false,
                          defaultValue: 0,
                          oldClrType: typeof(int),
                          oldType: "integer",
                          oldNullable: true);
        }
    }
}
