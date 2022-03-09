using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateTriggerToDeleteCaseGeoLocation : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("CREATE TRIGGER DeleteCasesGeoLocation ON dbo.Cases AFTER DELETE AS SET NOCOUNT ON DELETE FROM dbo.GeoLocations WHERE Id IN(SELECT GeoLocationId FROM deleted)");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DROP TRIGGER DeleteCasesGeoLocation");
		}
	}
}
