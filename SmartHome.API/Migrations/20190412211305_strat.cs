using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.API.Migrations
{
    public partial class strat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ControlStrategyId",
                table: "controllable_node",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ControlStrategy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Strategy = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlStrategy", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_controllable_node_ControlStrategyId",
                table: "controllable_node",
                column: "ControlStrategyId");

            migrationBuilder.AddForeignKey(
                name: "FK_controllable_node_ControlStrategy_ControlStrategyId",
                table: "controllable_node",
                column: "ControlStrategyId",
                principalTable: "ControlStrategy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_controllable_node_ControlStrategy_ControlStrategyId",
                table: "controllable_node");

            migrationBuilder.DropTable(
                name: "ControlStrategy");

            migrationBuilder.DropIndex(
                name: "IX_controllable_node_ControlStrategyId",
                table: "controllable_node");

            migrationBuilder.DropColumn(
                name: "ControlStrategyId",
                table: "controllable_node");
        }
    }
}
