using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class SeedLocalesTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "Locales",
				columns: new[] { "Id", "Name" },
				values: new object[] { (byte)1, "EN" });

			migrationBuilder.InsertData(
				table: "Locales",
				columns: new[] { "Id", "Name" },
				values: new object[] { (byte)2, "AR" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "Locales",
				keyColumn: "Id",
				keyValue: (byte)1);

			migrationBuilder.DeleteData(
				table: "Locales",
				keyColumn: "Id",
				keyValue: (byte)2);
		}
	}
}
