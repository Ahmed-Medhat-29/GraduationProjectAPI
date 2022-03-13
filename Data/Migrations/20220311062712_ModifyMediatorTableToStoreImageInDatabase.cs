using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class ModifyMediatorTableToStoreImageInDatabase : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "NationalIdImageName",
				table: "Mediators");

			migrationBuilder.DropColumn(
				name: "ProfileImageName",
				table: "Mediators");

			migrationBuilder.AddColumn<byte[]>(
				name: "NationalIdImage",
				table: "Mediators",
				type: "varbinary(max)",
				nullable: true);

			migrationBuilder.AddColumn<byte[]>(
				name: "ProfileImage",
				table: "Mediators",
				type: "varbinary(max)",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "NationalIdImage",
				table: "Mediators");

			migrationBuilder.DropColumn(
				name: "ProfileImage",
				table: "Mediators");

			migrationBuilder.AddColumn<string>(
				name: "NationalIdImageName",
				table: "Mediators",
				type: "nvarchar(4000)",
				maxLength: 4000,
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "ProfileImageName",
				table: "Mediators",
				type: "nvarchar(4000)",
				maxLength: 4000,
				nullable: true);
		}
	}
}
