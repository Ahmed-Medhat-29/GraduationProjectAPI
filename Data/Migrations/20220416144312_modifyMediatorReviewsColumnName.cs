using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class modifyMediatorReviewsColumnName : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_MediatorReviews_Mediators_ReviewedId",
				table: "MediatorReviews");

			migrationBuilder.RenameColumn(
				name: "ReviewedId",
				table: "MediatorReviews",
				newName: "RevieweeId");

			migrationBuilder.RenameIndex(
				name: "IX_MediatorReviews_ReviewedId",
				table: "MediatorReviews",
				newName: "IX_MediatorReviews_RevieweeId");

			migrationBuilder.AddForeignKey(
				name: "FK_MediatorReviews_Mediators_RevieweeId",
				table: "MediatorReviews",
				column: "RevieweeId",
				principalTable: "Mediators",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_MediatorReviews_Mediators_RevieweeId",
				table: "MediatorReviews");

			migrationBuilder.RenameColumn(
				name: "RevieweeId",
				table: "MediatorReviews",
				newName: "ReviewedId");

			migrationBuilder.RenameIndex(
				name: "IX_MediatorReviews_RevieweeId",
				table: "MediatorReviews",
				newName: "IX_MediatorReviews_ReviewedId");

			migrationBuilder.AddForeignKey(
				name: "FK_MediatorReviews_Mediators_ReviewedId",
				table: "MediatorReviews",
				column: "ReviewedId",
				principalTable: "Mediators",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}
