using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class MakeNationalIdAndPhoneNumberUniqueInCases : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_Cases_NationalId",
				table: "Cases",
				column: "NationalId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Cases_PhoneNumber",
				table: "Cases",
				column: "PhoneNumber",
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Cases_NationalId",
				table: "Cases");

			migrationBuilder.DropIndex(
				name: "IX_Cases_PhoneNumber",
				table: "Cases");
		}
	}
}
