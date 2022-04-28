using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class AlterNotificationsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "DateTime",
				table: "Notifications",
				type: "datetime2(0)",
				nullable: false,
				defaultValueSql: "GETDATE()");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "DateTime",
				table: "Notifications");
		}
	}
}
