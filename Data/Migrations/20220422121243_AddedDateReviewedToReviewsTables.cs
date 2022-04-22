using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
    public partial class AddedDateReviewedToReviewsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReviewed",
                table: "MediatorReviews",
                type: "date",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReviewed",
                table: "CaseReviews",
                type: "date",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReviewed",
                table: "MediatorReviews");

            migrationBuilder.DropColumn(
                name: "DateReviewed",
                table: "CaseReviews");
        }
    }
}
