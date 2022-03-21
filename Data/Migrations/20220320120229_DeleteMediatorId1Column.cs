using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class DeleteMediatorId1Column : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Cases_Mediators_MediatorId1",
				table: "Cases");

			migrationBuilder.DropIndex(
				name: "IX_Cases_MediatorId1",
				table: "Cases");

			migrationBuilder.DropColumn(
				name: "MediatorId1",
				table: "Cases");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "MediatorId1",
				table: "Cases",
				type: "int",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Cases_MediatorId1",
				table: "Cases",
				column: "MediatorId1");

			migrationBuilder.AddForeignKey(
				name: "FK_Cases_Mediators_MediatorId1",
				table: "Cases",
				column: "MediatorId1",
				principalTable: "Mediators",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}
