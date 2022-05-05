using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace GraduationProjectAPI.Migrations
{
	public partial class InitDatabase : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "FAQs",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
					Description = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FAQs", x => x.Id);
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
					Location = table.Column<Point>(type: "geography", nullable: false),
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
				name: "Locales",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Locales", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "MessageType",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MessageType", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "NotificationTypes",
				columns: table => new
				{
					Id = table.Column<byte>(type: "tinyint", nullable: false),
					Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_NotificationTypes", x => x.Id);
				});

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
					BirthDate = table.Column<DateTime>(type: "date", nullable: true),
					Bio = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
					Job = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
					Address = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
					FirebaseToken = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
					Completed = table.Column<bool>(type: "bit", nullable: false),
					ProfileImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
					NationalIdImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
					DateRegistered = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETDATE()"),
					GeoLocationId = table.Column<int>(type: "int", nullable: false),
					RegionId = table.Column<int>(type: "int", nullable: true),
					GenderId = table.Column<byte>(type: "tinyint", nullable: false),
					SocialStatusId = table.Column<byte>(type: "tinyint", nullable: false),
					StatusId = table.Column<byte>(type: "tinyint", nullable: false),
					LocaleId = table.Column<byte>(type: "tinyint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Mediators", x => x.Id);
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
						name: "FK_Mediators_Locales_LocaleId",
						column: x => x.LocaleId,
						principalTable: "Locales",
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
					PaymentDate = table.Column<DateTime>(type: "date", nullable: false),
					DateRequested = table.Column<DateTime>(type: "date", nullable: false),
					NationalIdImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
					Address = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
					Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					Story = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
					PeriodId = table.Column<byte>(type: "tinyint", nullable: false),
					MediatorId = table.Column<int>(type: "int", nullable: false),
					CategoryId = table.Column<int>(type: "int", nullable: false),
					RelationshipId = table.Column<byte>(type: "tinyint", nullable: false),
					PriorityId = table.Column<byte>(type: "tinyint", nullable: false),
					GenderId = table.Column<byte>(type: "tinyint", nullable: false),
					GeoLocationId = table.Column<int>(type: "int", nullable: false),
					SocialStatusId = table.Column<byte>(type: "tinyint", nullable: false),
					RegionId = table.Column<int>(type: "int", nullable: false),
					StatusId = table.Column<byte>(type: "tinyint", nullable: false)
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
						name: "FK_Cases_Periods_PeriodId",
						column: x => x.PeriodId,
						principalTable: "Periods",
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
				name: "Chats",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
					DateTime = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
					MessageTypeId = table.Column<byte>(type: "tinyint", nullable: false),
					MediatorId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Chats", x => x.Id);
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

			migrationBuilder.CreateTable(
				name: "MediatorReviews",
				columns: table => new
				{
					RevieweeId = table.Column<int>(type: "int", nullable: false),
					ReviewerId = table.Column<int>(type: "int", nullable: false),
					IsWorthy = table.Column<bool>(type: "bit", nullable: false),
					Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
					DateReviewed = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "GETDATE()")
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MediatorReviews", x => new { x.RevieweeId, x.ReviewerId });
					table.ForeignKey(
						name: "FK_MediatorReviews_Mediators_RevieweeId",
						column: x => x.RevieweeId,
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

			migrationBuilder.CreateTable(
				name: "Notifications",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					Body = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
					IsRead = table.Column<bool>(type: "bit", nullable: false),
					DateTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "GETDATE()"),
					ImageUrl = table.Column<string>(type: "varchar(4000)", nullable: false),
					TaskId = table.Column<int>(type: "int", nullable: false),
					TypeId = table.Column<byte>(type: "tinyint", nullable: false),
					MediatorId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Notifications", x => x.Id);
					table.ForeignKey(
						name: "FK_Notifications_Mediators_MediatorId",
						column: x => x.MediatorId,
						principalTable: "Mediators",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Notifications_NotificationTypes_TypeId",
						column: x => x.TypeId,
						principalTable: "NotificationTypes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "CaseReviews",
				columns: table => new
				{
					CaseId = table.Column<int>(type: "int", nullable: false),
					MediatorId = table.Column<int>(type: "int", nullable: false),
					IsWorthy = table.Column<bool>(type: "bit", nullable: false),
					Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
					DateReviewed = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "GETDATE()")
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
					{ 1, "Medical" },
					{ 2, "Poverty" }
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
				table: "Locales",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)2, "AR" },
					{ (byte)1, "EN" }
				});

			migrationBuilder.InsertData(
				table: "MessageType",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)2, "Received" },
					{ (byte)1, "Sent" }
				});

			migrationBuilder.InsertData(
				table: "NotificationTypes",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)5, "Chat" },
					{ (byte)4, "Payment" },
					{ (byte)3, "Case" },
					{ (byte)1, "General" },
					{ (byte)2, "Mediator" }
				});

			migrationBuilder.InsertData(
				table: "Periods",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)1, "One Time" },
					{ (byte)2, "Weekly" },
					{ (byte)3, "Monthly" }
				});

			migrationBuilder.InsertData(
				table: "Priorities",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)3, "Normal" },
					{ (byte)2, "High" },
					{ (byte)1, "Urgent" }
				});

			migrationBuilder.InsertData(
				table: "Relationships",
				columns: new[] { "Id", "Name" },
				values: new object[,]
				{
					{ (byte)3, "Neighbor" },
					{ (byte)2, "Family" },
					{ (byte)1, "Self" }
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
				name: "IX_CaseReviews_CaseId",
				table: "CaseReviews",
				column: "CaseId");

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
				name: "IX_Cases_NationalId",
				table: "Cases",
				column: "NationalId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Cases_PeriodId",
				table: "Cases",
				column: "PeriodId");

			migrationBuilder.CreateIndex(
				name: "IX_Cases_PhoneNumber",
				table: "Cases",
				column: "PhoneNumber",
				unique: true);

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
				name: "IX_Chats_MediatorId",
				table: "Chats",
				column: "MediatorId");

			migrationBuilder.CreateIndex(
				name: "IX_Chats_MessageTypeId",
				table: "Chats",
				column: "MessageTypeId");

			migrationBuilder.CreateIndex(
				name: "IX_Cities_GovernorateId",
				table: "Cities",
				column: "GovernorateId");

			migrationBuilder.CreateIndex(
				name: "IX_Cities_Name_GovernorateId",
				table: "Cities",
				columns: new[] { "Name", "GovernorateId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Governorates_Name",
				table: "Governorates",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Images_CaseId",
				table: "Images",
				column: "CaseId");

			migrationBuilder.CreateIndex(
				name: "IX_MediatorReviews_ReviewerId",
				table: "MediatorReviews",
				column: "ReviewerId");

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
				name: "IX_Mediators_LocaleId",
				table: "Mediators",
				column: "LocaleId");

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_NationalId",
				table: "Mediators",
				column: "NationalId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Mediators_PhoneNumber",
				table: "Mediators",
				column: "PhoneNumber",
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
				name: "IX_Notifications_MediatorId",
				table: "Notifications",
				column: "MediatorId");

			migrationBuilder.CreateIndex(
				name: "IX_Notifications_TypeId",
				table: "Notifications",
				column: "TypeId");

			migrationBuilder.CreateIndex(
				name: "IX_Regions_CityId",
				table: "Regions",
				column: "CityId");

			migrationBuilder.CreateIndex(
				name: "IX_Regions_Name_CityId",
				table: "Regions",
				columns: new[] { "Name", "CityId" },
				unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "CaseReviews");

			migrationBuilder.DropTable(
				name: "Chats");

			migrationBuilder.DropTable(
				name: "FAQs");

			migrationBuilder.DropTable(
				name: "Images");

			migrationBuilder.DropTable(
				name: "MediatorReviews");

			migrationBuilder.DropTable(
				name: "Notifications");

			migrationBuilder.DropTable(
				name: "MessageType");

			migrationBuilder.DropTable(
				name: "Cases");

			migrationBuilder.DropTable(
				name: "NotificationTypes");

			migrationBuilder.DropTable(
				name: "Categories");

			migrationBuilder.DropTable(
				name: "Mediators");

			migrationBuilder.DropTable(
				name: "Periods");

			migrationBuilder.DropTable(
				name: "Priorities");

			migrationBuilder.DropTable(
				name: "Relationships");

			migrationBuilder.DropTable(
				name: "Genders");

			migrationBuilder.DropTable(
				name: "GeoLocations");

			migrationBuilder.DropTable(
				name: "Locales");

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
