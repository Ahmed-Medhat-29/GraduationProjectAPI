using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateAdminsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Admins",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
					NationalId = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
					Email = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
					FirebaseToken = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
					ProfileImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
					NationalIdImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
					DateRegistered = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
					GenderId = table.Column<byte>(type: "tinyint", nullable: false),
					LocaleId = table.Column<byte>(type: "tinyint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Admins", x => x.Id);
					table.ForeignKey(
						name: "FK_Admins_Genders_GenderId",
						column: x => x.GenderId,
						principalTable: "Genders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Admins_Locales_LocaleId",
						column: x => x.LocaleId,
						principalTable: "Locales",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Admins_GenderId",
				table: "Admins",
				column: "GenderId");

			migrationBuilder.CreateIndex(
				name: "IX_Admins_LocaleId",
				table: "Admins",
				column: "LocaleId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Admins");
		}
	}
}
