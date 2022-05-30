using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateDonatorsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Donators",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
					FirebaseToken = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
					DateRegistered = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
					LocaleId = table.Column<byte>(type: "tinyint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Donators", x => x.Id);
					table.ForeignKey(
						name: "FK_Donators_Locales_LocaleId",
						column: x => x.LocaleId,
						principalTable: "Locales",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Donators_LocaleId",
				table: "Donators",
				column: "LocaleId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Donators");
		}
	}
}
