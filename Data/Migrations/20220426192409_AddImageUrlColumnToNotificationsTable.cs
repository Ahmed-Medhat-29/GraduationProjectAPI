using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class AddImageUrlColumnToNotificationsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "ImageUrl",
				table: "Notifications",
				type: "varchar(4000)",
				nullable: false,
				defaultValue: "");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ImageUrl",
				table: "Notifications");
		}
	}
}
