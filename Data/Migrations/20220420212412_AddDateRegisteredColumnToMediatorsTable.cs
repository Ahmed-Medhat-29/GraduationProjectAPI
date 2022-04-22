using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class AddDateRegisteredColumnToMediatorsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "DateRegistered",
				table: "Mediators",
				type: "datetime2(0)",
				nullable: false,
				defaultValueSql: "GETDATE()");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "DateRegistered",
				table: "Mediators");
		}
	}
}
