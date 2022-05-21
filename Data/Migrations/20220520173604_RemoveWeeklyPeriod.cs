using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
    public partial class RemoveWeeklyPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Periods",
                keyColumn: "Id",
                keyValue: (byte)3);

            migrationBuilder.UpdateData(
                table: "Periods",
                keyColumn: "Id",
                keyValue: (byte)2,
                column: "Name",
                value: "Monthly");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Periods",
                keyColumn: "Id",
                keyValue: (byte)2,
                column: "Name",
                value: "Weekly");

            migrationBuilder.InsertData(
                table: "Periods",
                columns: new[] { "Id", "Name" },
                values: new object[] { (byte)3, "Monthly" });
        }
    }
}
