using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Core.Migrations
{
    public partial class sec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Namespace",
                table: "control_strategy",
                newName: "ExecutorClassNamespace");

            migrationBuilder.RenameColumn(
                name: "Namespace",
                table: "command",
                newName: "ExecutorClassName");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "control_strategy",
                maxLength: 20,
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "control_strategy");

            migrationBuilder.RenameColumn(
                name: "ExecutorClassNamespace",
                table: "control_strategy",
                newName: "Namespace");

            migrationBuilder.RenameColumn(
                name: "ExecutorClassName",
                table: "command",
                newName: "Namespace");
        }
    }
}
