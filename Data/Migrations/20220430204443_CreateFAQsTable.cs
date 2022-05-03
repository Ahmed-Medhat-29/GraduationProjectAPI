using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateFAQsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "FAQs",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
					Description = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FAQs", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "FAQs");
		}
	}
}
