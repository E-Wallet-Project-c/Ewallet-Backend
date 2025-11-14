using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace E_wallet.Infrastrucure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationTableandprofiletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Wallet",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Profile",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Profile",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailNotifications",
                table: "Profile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Profile",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SMSNotifications",
                table: "Profile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Profile",
                type: "time with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Profile",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            // 👇 هنا تعديل جدول Notification لجعل Id = Identity
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Notification",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "EmailNotifications",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "SMSNotifications",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Profile");

            // 👇 إرجاع Id لحالته السابقة بدون Identity
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
