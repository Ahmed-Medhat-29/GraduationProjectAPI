using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class OptimizeMediators : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropUniqueConstraint(
				name: "AK_Mediators_NationalId",
				table: "Mediators");

			migrationBuilder.DropUniqueConstraint(
				name: "AK_Mediators_PhoneNumber",
				table: "Mediators");

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_NationalId",
				table: "Mediators",
				column: "NationalId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_PhoneNumber",
				table: "Mediators",
				column: "PhoneNumber",
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_Mediators_NationalId",
				table: "Mediators");

			migrationBuilder.DropIndex(
				name: "IX_Mediators_PhoneNumber",
				table: "Mediators");

			migrationBuilder.AddUniqueConstraint(
				name: "AK_Mediators_NationalId",
				table: "Mediators",
				column: "NationalId");

			migrationBuilder.AddUniqueConstraint(
				name: "AK_Mediators_PhoneNumber",
				table: "Mediators",
				column: "PhoneNumber");
		}
	}
}
