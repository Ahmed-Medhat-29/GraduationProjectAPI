using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
    public partial class ModifyCasesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateLimit",
                table: "Cases");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDay",
                table: "Cases",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "PeriodId",
                table: "Cases",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_PeriodId",
                table: "Cases",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Periods_PeriodId",
                table: "Cases",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Periods_PeriodId",
                table: "Cases");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Cases_PeriodId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "PaymentDay",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "Cases");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLimit",
                table: "Cases",
                type: "date",
                nullable: true);
        }
    }
}
