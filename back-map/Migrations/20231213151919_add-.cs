using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_map.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Announcements_AnnouncementId",
                table: "MediaFiles");

            migrationBuilder.AlterColumn<int>(
                name: "AnnouncementId",
                table: "MediaFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Announcements_AnnouncementId",
                table: "MediaFiles",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_Announcements_AnnouncementId",
                table: "MediaFiles");

            migrationBuilder.AlterColumn<int>(
                name: "AnnouncementId",
                table: "MediaFiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_Announcements_AnnouncementId",
                table: "MediaFiles",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id");
        }
    }
}
