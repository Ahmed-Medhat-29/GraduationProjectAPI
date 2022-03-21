using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraduationProjectAPI.Migrations
{
	public partial class InitialDatabase : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Complains",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
					Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Complains", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Genders",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Genders", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "GeoLocations",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
					Latitude = table.Column<decimal>(type: "decimal(8,6)", nullable: false),
					Details = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_GeoLocations", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Governorates",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Governorates", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Priorities",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Priorities", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Relationships",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Relationships", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "SocialStatus",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SocialStatus", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Status",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Status", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Cities",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
					GovernorateId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Cities", x => x.Id);
					table.ForeignKey(
						name: "FK_Cities_Governorates_GovernorateId",
						column: x => x.GovernorateId,
						principalTable: "Governorates",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Regions",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
					CityId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Regions", x => x.Id);
					table.ForeignKey(
						name: "FK_Regions_Cities_CityId",
						column: x => x.CityId,
						principalTable: "Cities",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Mediators",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
					NationalId = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
					Job = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
					Address = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
					BirthDate = table.Column<DateTime>(type: "date", nullable: true),
					Bio = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
					NationalIdImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
					ProfileImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
					GeoLocationId = table.Column<int>(type: "int", nullable: false),
					RegionId = table.Column<int>(type: "int", nullable: true),
					GenderId = table.Column<byte>(type: "tinyint", nullable: false),
					SocialStatusId = table.Column<byte>(type: "tinyint", nullable: false),
					StatusId = table.Column<byte>(type: "tinyint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Mediators", x => x.Id);
					table.UniqueConstraint("AK_Mediators_NationalId", x => x.NationalId);
					table.UniqueConstraint("AK_Mediators_PhoneNumber", x => x.PhoneNumber);
					table.ForeignKey(
						name: "FK_Mediators_Genders_GenderId",
						column: x => x.GenderId,
						principalTable: "Genders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Mediators_GeoLocations_GeoLocationId",
						column: x => x.GeoLocationId,
						principalTable: "GeoLocations",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Mediators_Regions_RegionId",
						column: x => x.RegionId,
						principalTable: "Regions",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Mediators_SocialStatus_SocialStatusId",
						column: x => x.SocialStatusId,
						principalTable: "SocialStatus",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Mediators_Status_StatusId",
						column: x => x.StatusId,
						principalTable: "Status",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Cases",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
					NationalId = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
					BirthDate = table.Column<DateTime>(type: "date", nullable: false),
					Adults = table.Column<byte>(type: "tinyint", nullable: false),
					Children = table.Column<byte>(type: "tinyint", nullable: false),
					NeededMoneyAmount = table.Column<int>(type: "int", nullable: false),
					DateLimit = table.Column<DateTime>(type: "date", nullable: true),
					DateRequested = table.Column<DateTime>(type: "date", nullable: false),
					NationalIdImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
					Address = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
					Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					Story = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
					MediatorId = table.Column<int>(type: "int", nullable: false),
					CategoryId = table.Column<byte>(type: "tinyint", nullable: false),
					RelationshipId = table.Column<byte>(type: "tinyint", nullable: false),
					PriorityId = table.Column<byte>(type: "tinyint", nullable: false),
					GenderId = table.Column<byte>(type: "tinyint", nullable: false),
					GeoLocationId = table.Column<int>(type: "int", nullable: false),
					SocialStatusId = table.Column<byte>(type: "tinyint", nullable: false),
					RegionId = table.Column<int>(type: "int", nullable: false),
					StatusId = table.Column<byte>(type: "tinyint", nullable: false),
					MediatorId1 = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Cases", x => x.Id);
					table.ForeignKey(
						name: "FK_Cases_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_Genders_GenderId",
						column: x => x.GenderId,
						principalTable: "Genders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_GeoLocations_GeoLocationId",
						column: x => x.GeoLocationId,
						principalTable: "GeoLocations",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_Mediators_MediatorId",
						column: x => x.MediatorId,
						principalTable: "Mediators",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_Mediators_MediatorId1",
						column: x => x.MediatorId1,
						principalTable: "Mediators",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_Priorities_PriorityId",
						column: x => x.PriorityId,
						principalTable: "Priorities",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_Regions_RegionId",
						column: x => x.RegionId,
						principalTable: "Regions",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_Relationships_RelationshipId",
						column: x => x.RelationshipId,
						principalTable: "Relationships",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_SocialStatus_SocialStatusId",
						column: x => x.SocialStatusId,
						principalTable: "SocialStatus",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Cases_Status_StatusId",
						column: x => x.StatusId,
						principalTable: "Status",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Images",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
					CaseId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Images", x => x.Id);
					table.ForeignKey(
						name: "FK_Images_Cases_CaseId",
						column: x => x.CaseId,
						principalTable: "Cases",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.InsertData(
				table: "Categories",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)1, "Medical" },
					{ (byte)2, "Poverty" }
				});

			migrationBuilder.InsertData(
				table: "Genders",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)2, "Female" },
					{ (byte)1, "Male" }
				});

			migrationBuilder.InsertData(
				table: "Priorities",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)3, "Normal" },
					{ (byte)1, "Urgent" },
					{ (byte)2, "High" }
				});

			migrationBuilder.InsertData(
				table: "Relationships",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)1, "Self" },
					{ (byte)2, "Family" },
					{ (byte)3, "Neighbor" }
				});

			migrationBuilder.InsertData(
				table: "SocialStatus",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)1, "Single" },
					{ (byte)2, "Married" },
					{ (byte)3, "Divorced" },
					{ (byte)4, "Widow" }
				});

			migrationBuilder.InsertData(
				table: "Status",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)3, "Rejected" },
					{ (byte)1, "Pending" },
					{ (byte)2, "Accepted" },
					{ (byte)4, "Submitted" }
				});

			migrationBuilder.CreateIndex(
				name: "IX_Cases_CategoryId",
				table: "Cases",
				column: "CategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_GenderId",
				table: "Cases",
				column: "GenderId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_GeoLocationId",
				table: "Cases",
				column: "GeoLocationId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Cases_MediatorId",
				table: "Cases",
				column: "MediatorId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_MediatorId1",
				table: "Cases",
				column: "MediatorId1");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_PriorityId",
				table: "Cases",
				column: "PriorityId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_RegionId",
				table: "Cases",
				column: "RegionId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_RelationshipId",
				table: "Cases",
				column: "RelationshipId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_SocialStatusId",
				table: "Cases",
				column: "SocialStatusId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_StatusId",
				table: "Cases",
				column: "StatusId");

			migrationBuilder.CreateIndex(
				name: "IX_Cities_GovernorateId",
				table: "Cities",
				column: "GovernorateId");

			migrationBuilder.CreateIndex(
				name: "IX_Images_CaseId",
				table: "Images",
				column: "CaseId");

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_GenderId",
				table: "Mediators",
				column: "GenderId");

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_GeoLocationId",
				table: "Mediators",
				column: "GeoLocationId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_RegionId",
				table: "Mediators",
				column: "RegionId");

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_SocialStatusId",
				table: "Mediators",
				column: "SocialStatusId");

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_StatusId",
				table: "Mediators",
				column: "StatusId");

			migrationBuilder.CreateIndex(
				name: "IX_Regions_CityId",
				table: "Regions",
				column: "CityId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Complains");

			migrationBuilder.DropTable(
				name: "Images");

			migrationBuilder.DropTable(
				name: "Cases");

			migrationBuilder.DropTable(
				name: "Categories");

			migrationBuilder.DropTable(
				name: "Mediators");

			migrationBuilder.DropTable(
				name: "Priorities");

			migrationBuilder.DropTable(
				name: "Relationships");

			migrationBuilder.DropTable(
				name: "Genders");

			migrationBuilder.DropTable(
				name: "GeoLocations");

			migrationBuilder.DropTable(
				name: "Regions");

			migrationBuilder.DropTable(
				name: "SocialStatus");

			migrationBuilder.DropTable(
				name: "Status");

			migrationBuilder.DropTable(
				name: "Cities");

			migrationBuilder.DropTable(
				name: "Governorates");
		}
	}
}
