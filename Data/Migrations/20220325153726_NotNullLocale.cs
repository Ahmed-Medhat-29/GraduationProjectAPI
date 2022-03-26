using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class NotNullLocale : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<byte>(
				name: "LocaleId",
				table: "Mediators",
				type: "tinyint",
				nullable: false,
				defaultValue: (byte)0,
				oldClrType: typeof(byte),
				oldType: "tinyint",
				oldNullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<byte>(
				name: "LocaleId",
				table: "Mediators",
				type: "tinyint",
				nullable: true,
				oldClrType: typeof(byte),
				oldType: "tinyint");
		}
	}
}
