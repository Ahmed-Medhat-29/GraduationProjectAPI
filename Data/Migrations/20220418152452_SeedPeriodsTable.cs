using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class SeedPeriodsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "Periods",
				columns: new[] { "Id", "Name" },
				values: new object[] { (byte)1, "One Time" });

			migrationBuilder.InsertData(
				table: "Periods",
				columns: new[] { "Id", "Name" },
				values: new object[] { (byte)2, "Weekly" });

			migrationBuilder.InsertData(
				table: "Periods",
				columns: new[] { "Id", "Name" },
				values: new object[] { (byte)3, "Monthly" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "Periods",
				keyColumn: "Id",
				keyValue: (byte)1);

			migrationBuilder.DeleteData(
				table: "Periods",
				keyColumn: "Id",
				keyValue: (byte)2);

			migrationBuilder.DeleteData(
				table: "Periods",
				keyColumn: "Id",
				keyValue: (byte)3);
		}
	}
}
