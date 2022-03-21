using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class AddNotificationTokenToMediators : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "NotificationToken",
				table: "Mediators",
				type: "varchar(4000)",
				maxLength: 4000,
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "NotificationToken",
				table: "Mediators");
		}
	}
}
