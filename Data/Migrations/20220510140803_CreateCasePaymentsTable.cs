using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class CreateCasePaymentsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "CasePayments",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					AdminId = table.Column<int>(type: "int", nullable: false),
					MediatorId = table.Column<int>(type: "int", nullable: false),
					CaseId = table.Column<int>(type: "int", nullable: false),
					Amount = table.Column<int>(type: "int", nullable: false),
					DateSubmitted = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
					DateDelivered = table.Column<DateTime>(type: "datetime2(0)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CasePayments", x => x.Id);
					table.ForeignKey(
						name: "FK_CasePayments_Cases_CaseId",
						column: x => x.CaseId,
						principalTable: "Cases",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_CasePayments_Mediators_MediatorId",
						column: x => x.MediatorId,
						principalTable: "Mediators",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_CasePayments_CaseId",
				table: "CasePayments",
				column: "CaseId");

			migrationBuilder.CreateIndex(
				name: "IX_CasePayments_MediatorId",
				table: "CasePayments",
				column: "MediatorId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CasePayments");
		}
	}
}
