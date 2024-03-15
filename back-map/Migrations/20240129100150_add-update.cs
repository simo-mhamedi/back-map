using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_map.Migrations
{
    /// <inheritdoc />
    public partial class addupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoreInfo_Announcements_AnnouncementId",
                table: "MoreInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoreInfo",
                table: "MoreInfo");

            migrationBuilder.RenameTable(
                name: "MoreInfo",
                newName: "MoreInfos");

            migrationBuilder.RenameIndex(
                name: "IX_MoreInfo_AnnouncementId",
                table: "MoreInfos",
                newName: "IX_MoreInfos_AnnouncementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoreInfos",
                table: "MoreInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoreInfos_Announcements_AnnouncementId",
                table: "MoreInfos",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoreInfos_Announcements_AnnouncementId",
                table: "MoreInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoreInfos",
                table: "MoreInfos");

            migrationBuilder.RenameTable(
                name: "MoreInfos",
                newName: "MoreInfo");

            migrationBuilder.RenameIndex(
                name: "IX_MoreInfos_AnnouncementId",
                table: "MoreInfo",
                newName: "IX_MoreInfo_AnnouncementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoreInfo",
                table: "MoreInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoreInfo_Announcements_AnnouncementId",
                table: "MoreInfo",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id");
        }
    }
}
