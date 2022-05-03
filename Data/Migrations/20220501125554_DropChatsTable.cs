﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class DropChatsTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Chats");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Chats",
				columns: table => new
				{
					DateTime = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
					Id = table.Column<int>(type: "int", nullable: false),
					MediatorId = table.Column<int>(type: "int", nullable: false),
					Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
					MessageTypeId = table.Column<byte>(type: "tinyint", nullable: false)
				},
				constraints: table =>
				{
					table.ForeignKey(
						name: "FK_Chats_Mediators_MediatorId",
						column: x => x.MediatorId,
						principalTable: "Mediators",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Chats_MessageType_MessageTypeId",
						column: x => x.MessageTypeId,
						principalTable: "MessageType",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Chats_Id",
				table: "Chats",
				column: "Id");

			migrationBuilder.CreateIndex(
				name: "IX_Chats_MediatorId",
				table: "Chats",
				column: "MediatorId");

			migrationBuilder.CreateIndex(
				name: "IX_Chats_MessageTypeId",
				table: "Chats",
				column: "MessageTypeId");
		}
	}
}