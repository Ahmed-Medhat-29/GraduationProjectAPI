using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class MakeMediatorProfilePicRequired : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<byte[]>(
				name: "ProfileImage",
				table: "Mediators",
				type: "varbinary(max)",
				nullable: false,
				defaultValue: new byte[0],
				oldClrType: typeof(byte[]),
				oldType: "varbinary(max)",
				oldNullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<byte[]>(
				name: "ProfileImage",
				table: "Mediators",
				type: "varbinary(max)",
				nullable: true,
				oldClrType: typeof(byte[]),
				oldType: "varbinary(max)");
		}
	}
}
