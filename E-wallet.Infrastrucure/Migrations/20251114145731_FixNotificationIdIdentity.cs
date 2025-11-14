using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    public partial class FixNotificationIdIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // تعديل عمود Id ليصبح Identity
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Notification",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // التراجع عن التعديل إذا تم Rollback
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Notification",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
