using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.API.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dictionary_value_dictionary_DictionaryId1",
                table: "dictionary_value");

            migrationBuilder.DropIndex(
                name: "IX_dictionary_value_DictionaryId1",
                table: "dictionary_value");

            migrationBuilder.DropColumn(
                name: "DictionaryId1",
                table: "dictionary_value");

            migrationBuilder.AlterColumn<int>(
                name: "DictionaryId",
                table: "dictionary_value",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dictionary_value_DictionaryId",
                table: "dictionary_value",
                column: "DictionaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_dictionary_value_dictionary_DictionaryId",
                table: "dictionary_value",
                column: "DictionaryId",
                principalTable: "dictionary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dictionary_value_dictionary_DictionaryId",
                table: "dictionary_value");

            migrationBuilder.DropIndex(
                name: "IX_dictionary_value_DictionaryId",
                table: "dictionary_value");

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryId",
                table: "dictionary_value",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DictionaryId1",
                table: "dictionary_value",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dictionary_value_DictionaryId1",
                table: "dictionary_value",
                column: "DictionaryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_dictionary_value_dictionary_DictionaryId1",
                table: "dictionary_value",
                column: "DictionaryId1",
                principalTable: "dictionary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
