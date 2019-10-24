using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Core.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_dictionary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ReadOnly = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_dictionary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_physical_property",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsComplex = table.Column<bool>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    Magnitude = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_physical_property", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_scheduling_job_status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_scheduling_job_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_scheduling_job_type",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FullyQualifiedName = table.Column<string>(maxLength: 255, nullable: false),
                    AssemblyName = table.Column<string>(maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_scheduling_job_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_scheduling_schedule_type",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    FullyQualifiedName = table.Column<string>(maxLength: 255, nullable: false),
                    AssemblyName = table.Column<string>(maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_scheduling_schedule_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ActivatedById = table.Column<int>(nullable: true),
                    ActivationDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_user_tbl_user_ActivatedById",
                        column: x => x.ActivatedById,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_dictionary_value",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DisplayValue = table.Column<string>(nullable: true),
                    InternalValue = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: true),
                    Metadata = table.Column<string>(nullable: true),
                    DictionaryId = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_dictionary_value", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_dictionary_value_tbl_dictionary_DictionaryId",
                        column: x => x.DictionaryId,
                        principalTable: "tbl_dictionary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_tbl_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tbl_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_tbl_user_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_tbl_user_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_tbl_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tbl_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_tbl_user_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_tbl_user_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_control_strategy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Connector = table.Column<string>(maxLength: 250, nullable: false),
                    ContractAssembly = table.Column<string>(maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<int>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_control_strategy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_control_strategy_tbl_user_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_control_strategy_tbl_user_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_control_strategy_tbl_user_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_scheduling_schedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    JobName = table.Column<string>(nullable: false),
                    JobGroup = table.Column<string>(nullable: true),
                    JobStatusEntityId = table.Column<int>(nullable: false),
                    JobTypeId = table.Column<int>(nullable: false),
                    ScheduleTypeid = table.Column<int>(nullable: false),
                    SerializedJobSchedule = table.Column<string>(nullable: true),
                    CronExpression = table.Column<string>(maxLength: 20, nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<int>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_scheduling_schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_scheduling_schedules_tbl_user_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_scheduling_schedules_tbl_scheduling_job_status_JobStatus~",
                        column: x => x.JobStatusEntityId,
                        principalTable: "tbl_scheduling_job_status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_scheduling_schedules_tbl_scheduling_job_type_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "tbl_scheduling_job_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_scheduling_schedules_tbl_scheduling_schedule_type_Schedu~",
                        column: x => x.ScheduleTypeid,
                        principalTable: "tbl_scheduling_schedule_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_scheduling_schedules_tbl_user_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ui_configuration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ui_configuration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_ui_configuration_tbl_user_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_node",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    UriSchema = table.Column<string>(maxLength: 10, nullable: true),
                    IpAddress = table.Column<string>(maxLength: 20, nullable: true),
                    Port = table.Column<int>(nullable: false),
                    GatewayIpAddress = table.Column<string>(maxLength: 20, nullable: true),
                    Login = table.Column<string>(maxLength: 40, nullable: true),
                    Password = table.Column<string>(maxLength: 40, nullable: true),
                    ApiKey = table.Column<string>(maxLength: 30, nullable: true),
                    BaseTopic = table.Column<string>(maxLength: 100, nullable: true),
                    ClientId = table.Column<string>(maxLength: 100, nullable: true),
                    ConfigMetadata = table.Column<string>(maxLength: 2147483647, nullable: true),
                    ControlStrategyId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<int>(nullable: false),
                    UpdatedById = table.Column<int>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(nullable: true),
                    AppUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_node", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_node_tbl_user_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_node_tbl_control_strategy_ControlStrategyId",
                        column: x => x.ControlStrategyId,
                        principalTable: "tbl_control_strategy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_node_tbl_user_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_node_tbl_user_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_physicalproperty_controlstrategy_link",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PhysicalPropertyId = table.Column<int>(nullable: false),
                    ControlStrategyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_physicalproperty_controlstrategy_link", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_physicalproperty_controlstrategy_link_tbl_control_strate~",
                        column: x => x.ControlStrategyId,
                        principalTable: "tbl_control_strategy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_physicalproperty_controlstrategy_link_tbl_physical_prope~",
                        column: x => x.PhysicalPropertyId,
                        principalTable: "tbl_physical_property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_node_data",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    NodeId = table.Column<int>(nullable: false),
                    PhysicalPropertyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_node_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_node_data_tbl_node_NodeId",
                        column: x => x.NodeId,
                        principalTable: "tbl_node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_node_data_tbl_physical_property_PhysicalPropertyId",
                        column: x => x.PhysicalPropertyId,
                        principalTable: "tbl_physical_property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_node_link",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NodeId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_node_link", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_user_node_link_tbl_node_NodeId",
                        column: x => x.NodeId,
                        principalTable: "tbl_node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_user_node_link_tbl_user_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_control_strategy_AppUserId",
                table: "tbl_control_strategy",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_control_strategy_CreatedById",
                table: "tbl_control_strategy",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_control_strategy_UpdatedById",
                table: "tbl_control_strategy",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_dictionary_value_DictionaryId",
                table: "tbl_dictionary_value",
                column: "DictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_node_AppUserId",
                table: "tbl_node",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_node_ClientId",
                table: "tbl_node",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_node_ControlStrategyId",
                table: "tbl_node",
                column: "ControlStrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_node_CreatedById",
                table: "tbl_node",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_node_UpdatedById",
                table: "tbl_node",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_node_data_NodeId",
                table: "tbl_node_data",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_node_data_PhysicalPropertyId",
                table: "tbl_node_data",
                column: "PhysicalPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_physicalproperty_controlstrategy_link_ControlStrategyId",
                table: "tbl_physicalproperty_controlstrategy_link",
                column: "ControlStrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_physicalproperty_controlstrategy_link_PhysicalPropertyId",
                table: "tbl_physicalproperty_controlstrategy_link",
                column: "PhysicalPropertyId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "tbl_role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_scheduling_schedules_CreatedById",
                table: "tbl_scheduling_schedules",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_scheduling_schedules_JobStatusEntityId",
                table: "tbl_scheduling_schedules",
                column: "JobStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_scheduling_schedules_JobTypeId",
                table: "tbl_scheduling_schedules",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_scheduling_schedules_ScheduleTypeid",
                table: "tbl_scheduling_schedules",
                column: "ScheduleTypeid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_scheduling_schedules_UpdatedById",
                table: "tbl_scheduling_schedules",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ui_configuration_UserId",
                table: "tbl_ui_configuration",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_ActivatedById",
                table: "tbl_user",
                column: "ActivatedById");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "tbl_user",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "tbl_user",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_node_link_NodeId",
                table: "tbl_user_node_link",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_node_link_UserId",
                table: "tbl_user_node_link",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "tbl_dictionary_value");

            migrationBuilder.DropTable(
                name: "tbl_node_data");

            migrationBuilder.DropTable(
                name: "tbl_physicalproperty_controlstrategy_link");

            migrationBuilder.DropTable(
                name: "tbl_scheduling_schedules");

            migrationBuilder.DropTable(
                name: "tbl_ui_configuration");

            migrationBuilder.DropTable(
                name: "tbl_user_node_link");

            migrationBuilder.DropTable(
                name: "tbl_role");

            migrationBuilder.DropTable(
                name: "tbl_dictionary");

            migrationBuilder.DropTable(
                name: "tbl_physical_property");

            migrationBuilder.DropTable(
                name: "tbl_scheduling_job_status");

            migrationBuilder.DropTable(
                name: "tbl_scheduling_job_type");

            migrationBuilder.DropTable(
                name: "tbl_scheduling_schedule_type");

            migrationBuilder.DropTable(
                name: "tbl_node");

            migrationBuilder.DropTable(
                name: "tbl_control_strategy");

            migrationBuilder.DropTable(
                name: "tbl_user");
        }
    }
}
