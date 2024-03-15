using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_map.Migrations
{
    /// <inheritdoc />
    public partial class add3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "MediaFileId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_MediaFileId",
                table: "Users",
                column: "MediaFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MediaFiles_MediaFileId",
                table: "Users",
                column: "MediaFileId",
                principalTable: "MediaFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_MediaFiles_MediaFileId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MediaFileId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MediaFileId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
