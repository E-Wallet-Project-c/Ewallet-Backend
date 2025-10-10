using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToProfile2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
             name: "UserId",
             table: "Profile",
             type: "integer",
             nullable: false,
             defaultValue: 0); // إذا جدولك موجود فيه بيانات، حدد default أو خلي nullable true مؤقتاً

            // أنشئ علاقة One-to-One بين Profile و User
            migrationBuilder.CreateIndex(
                name: "IX_Profile_UserId",
                table: "Profile",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_User_UserId",
                table: "Profile",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // احذف ProfileId من جدول User لأنه ما عاد موجود
            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
             name: "ProfileId",
             table: "User",
             type: "integer",
             nullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_Profile_User_UserId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_UserId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Profile");
        }
    }
}
