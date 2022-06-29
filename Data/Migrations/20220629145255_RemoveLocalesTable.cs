using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProjectAPI.Migrations
{
    public partial class RemoveLocalesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Locales_LocaleId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Donators_Locales_LocaleId",
                table: "Donators");

            migrationBuilder.DropForeignKey(
                name: "FK_Mediators_Locales_LocaleId",
                table: "Mediators");

            migrationBuilder.DropTable(
                name: "Locales");

            migrationBuilder.DropIndex(
                name: "IX_Mediators_LocaleId",
                table: "Mediators");

            migrationBuilder.DropIndex(
                name: "IX_Donators_LocaleId",
                table: "Donators");

            migrationBuilder.DropIndex(
                name: "IX_Admins_LocaleId",
                table: "Admins");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "LocaleId",
                table: "Mediators");

            migrationBuilder.DropColumn(
                name: "LocaleId",
                table: "Donators");

            migrationBuilder.DropColumn(
                name: "LocaleId",
                table: "Admins");

            migrationBuilder.AddColumn<string>(
                name: "Name_AR",
                table: "Regions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_AR",
                table: "Governorates",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_AR",
                table: "Cities",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Categories",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name_AR",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name_AR_CityId",
                table: "Regions",
                columns: new[] { "Name_AR", "CityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_Name_AR",
                table: "Governorates",
                column: "Name_AR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name_AR_GovernorateId",
                table: "Cities",
                columns: new[] { "Name_AR", "GovernorateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_AR",
                table: "Categories",
                column: "Name_AR",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Regions_Name_AR_CityId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Governorates_Name_AR",
                table: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name_AR_GovernorateId",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name_AR",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Name_AR",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Name_AR",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "Name_AR",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Name_AR",
                table: "Categories");

            migrationBuilder.AddColumn<byte>(
                name: "LocaleId",
                table: "Mediators",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "LocaleId",
                table: "Donators",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Categories",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<byte>(
                name: "LocaleId",
                table: "Admins",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "Locales",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locales", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[,]
                {
                    { 1, null, "Medical" },
                    { 2, null, "Poverty" }
                });

            migrationBuilder.InsertData(
                table: "Locales",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (byte)1, "EN" },
                    { (byte)2, "AR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mediators_LocaleId",
                table: "Mediators",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Donators_LocaleId",
                table: "Donators",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_LocaleId",
                table: "Admins",
                column: "LocaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Locales_LocaleId",
                table: "Admins",
                column: "LocaleId",
                principalTable: "Locales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donators_Locales_LocaleId",
                table: "Donators",
                column: "LocaleId",
                principalTable: "Locales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mediators_Locales_LocaleId",
                table: "Mediators",
                column: "LocaleId",
                principalTable: "Locales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
