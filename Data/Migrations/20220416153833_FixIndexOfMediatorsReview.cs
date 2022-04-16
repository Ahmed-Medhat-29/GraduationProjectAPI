using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class FixIndexOfMediatorsReview : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_MediatorReviews_RevieweeId",
				table: "MediatorReviews");

			migrationBuilder.DropIndex(
				name: "IX_MediatorReviews_ReviewerId",
				table: "MediatorReviews");

			migrationBuilder.CreateIndex(
				name: "IX_MediatorReviews_ReviewerId",
				table: "MediatorReviews",
				column: "ReviewerId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_MediatorReviews_ReviewerId",
				table: "MediatorReviews");

			migrationBuilder.CreateIndex(
				name: "IX_MediatorReviews_RevieweeId",
				table: "MediatorReviews",
				column: "RevieweeId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_MediatorReviews_ReviewerId",
				table: "MediatorReviews",
				column: "ReviewerId",
				unique: true);
		}
	}
}
