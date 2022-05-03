using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class AddChatNotificationtype : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "NotificationTypes",
				columns: new[] { "Id", "Name" },
				values: new object[] { (byte)5, "Chat" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "NotificationTypes",
				keyColumn: "Id",
				keyValue: (byte)5);
		}
	}
}
