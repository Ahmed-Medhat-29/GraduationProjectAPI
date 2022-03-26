using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class ChangeNotificationTokenColumnNameToFirebaseToken : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "NotificationToken",
				table: "Mediators",
				newName: "FirebaseToken");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "FirebaseToken",
				table: "Mediators",
				newName: "NotificationToken");
		}
	}
}
