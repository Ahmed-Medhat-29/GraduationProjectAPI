using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateCaseReviewsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "CaseReviews",
				columns: table => new
				{
					CaseId = table.Column<int>(type: "int", nullable: false),
					MediatorId = table.Column<int>(type: "int", nullable: false),
					IsWorthy = table.Column<bool>(type: "bit", nullable: false),
					Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CaseReviews", x => new { x.MediatorId, x.CaseId });
					table.ForeignKey(
						name: "FK_CaseReviews_Cases_CaseId",
						column: x => x.CaseId,
						principalTable: "Cases",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_CaseReviews_Mediators_MediatorId",
						column: x => x.MediatorId,
						principalTable: "Mediators",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_CaseReviews_CaseId",
				table: "CaseReviews",
				column: "CaseId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CaseReviews");
		}
	}
}
