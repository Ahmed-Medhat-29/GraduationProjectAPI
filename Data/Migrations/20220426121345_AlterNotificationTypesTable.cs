using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class AlterNotificationTypesTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Title",
				table: "Notifications",
				type: "nvarchar(250)",
				maxLength: 250,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(4000)",
				oldMaxLength: 4000);

			migrationBuilder.UpdateData(
				table: "NotificationTypes",
				keyColumn: "Id",
				keyValue: (byte)1,
				column: "Name",
				value: "General");

			migrationBuilder.UpdateData(
				table: "NotificationTypes",
				keyColumn: "Id",
				keyValue: (byte)2,
				column: "Name",
				value: "Mediator");

			migrationBuilder.InsertData(
				table: "NotificationTypes",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)3, "Case" },
					{ (byte)4, "Payment" }
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "NotificationTypes",
				keyColumn: "Id",
				keyValue: (byte)3);

			migrationBuilder.DeleteData(
				table: "NotificationTypes",
				keyColumn: "Id",
				keyValue: (byte)4);

			migrationBuilder.AlterColumn<string>(
				name: "Title",
				table: "Notifications",
				type: "nvarchar(4000)",
				maxLength: 4000,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(250)",
				oldMaxLength: 250);

			migrationBuilder.UpdateData(
				table: "NotificationTypes",
				keyColumn: "Id",
				keyValue: (byte)1,
				column: "Name",
				value: "MediatorReview");

			migrationBuilder.UpdateData(
				table: "NotificationTypes",
				keyColumn: "Id",
				keyValue: (byte)2,
				column: "Name",
				value: "CaseReview");
		}
	}
}
