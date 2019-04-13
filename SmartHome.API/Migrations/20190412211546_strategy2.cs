using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.API.Migrations
{
    public partial class strategy2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_controllable_node_ControlStrategy_ControlStrategyId",
                table: "controllable_node");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ControlStrategy",
                table: "ControlStrategy");

            migrationBuilder.RenameTable(
                name: "ControlStrategy",
                newName: "control_strategy");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "control_strategy",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_control_strategy",
                table: "control_strategy",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_controllable_node_control_strategy_ControlStrategyId",
                table: "controllable_node",
                column: "ControlStrategyId",
                principalTable: "control_strategy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_controllable_node_control_strategy_ControlStrategyId",
                table: "controllable_node");

            migrationBuilder.DropPrimaryKey(
                name: "PK_control_strategy",
                table: "control_strategy");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "control_strategy");

            migrationBuilder.RenameTable(
                name: "control_strategy",
                newName: "ControlStrategy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ControlStrategy",
                table: "ControlStrategy",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_controllable_node_ControlStrategy_ControlStrategyId",
                table: "controllable_node",
                column: "ControlStrategyId",
                principalTable: "ControlStrategy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
