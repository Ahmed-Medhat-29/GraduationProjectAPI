using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateLocalesTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<byte>(
				name: "LocaleId",
				table: "Mediators",
				type: "tinyint",
				nullable: true);

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

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_LocaleId",
				table: "Mediators",
				column: "LocaleId");

			migrationBuilder.AddForeignKey(
				name: "FK_Mediators_Locales_LocaleId",
				table: "Mediators",
				column: "LocaleId",
				principalTable: "Locales",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Mediators_Locales_LocaleId",
				table: "Mediators");

			migrationBuilder.DropTable(
				name: "Locales");

			migrationBuilder.DropIndex(
				name: "IX_Mediators_LocaleId",
				table: "Mediators");

			migrationBuilder.DropColumn(
				name: "LocaleId",
				table: "Mediators");
		}
	}
}
