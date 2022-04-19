using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateNotificationTypesTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "NotificationTypes",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_NotificationTypes", x => x.Id);
				});

			migrationBuilder.InsertData(
				table: "NotificationTypes",
				columns: new[] { "Id", "Name" },
				values: new object[] { (byte)1, "MediatorReview" });

			migrationBuilder.InsertData(
				table: "NotificationTypes",
				columns: new[] { "Id", "Name" },
				values: new object[] { (byte)2, "CaseReview" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "NotificationTypes");
		}
	}
}
