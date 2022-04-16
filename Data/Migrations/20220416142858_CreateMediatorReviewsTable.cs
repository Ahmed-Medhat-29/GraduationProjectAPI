using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateMediatorReviewsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "MediatorReviews",
				columns: table => new
				{
					ReviewedId = table.Column<int>(type: "int", nullable: false),
					ReviewerId = table.Column<int>(type: "int", nullable: false),
					IsWorthy = table.Column<bool>(type: "bit", nullable: false),
					Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MediatorReviews", x => new { x.ReviewedId, x.ReviewerId });
					table.ForeignKey(
						name: "FK_MediatorReviews_Mediators_ReviewedId",
						column: x => x.ReviewedId,
						principalTable: "Mediators",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_MediatorReviews_Mediators_ReviewerId",
						column: x => x.ReviewerId,
						principalTable: "Mediators",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_MediatorReviews_ReviewedId",
				table: "MediatorReviews",
				column: "ReviewedId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_MediatorReviews_ReviewerId",
				table: "MediatorReviews",
				column: "ReviewerId",
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "MediatorReviews");
		}
	}
}
