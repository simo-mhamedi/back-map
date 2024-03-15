using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace back_map.Migrations
{
    /// <inheritdoc />
    public partial class addupdate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoreInfos_Announcements_AnnouncementId",
                table: "MoreInfos");

            migrationBuilder.DropIndex(
                name: "IX_MoreInfos_AnnouncementId",
                table: "MoreInfos");

            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "MoreInfos");

            migrationBuilder.DropColumn(
                name: "MoreInfosIds",
                table: "Announcements");

            migrationBuilder.CreateTable(
                name: "AnnoucementInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MoreInfoId = table.Column<int>(type: "integer", nullable: false),
                    AnnouncementId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnoucementInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnoucementInfos_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnoucementInfos_MoreInfos_MoreInfoId",
                        column: x => x.MoreInfoId,
                        principalTable: "MoreInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnoucementInfos_AnnouncementId",
                table: "AnnoucementInfos",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnoucementInfos_MoreInfoId",
                table: "AnnoucementInfos",
                column: "MoreInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnoucementInfos");

            migrationBuilder.AddColumn<int>(
                name: "AnnouncementId",
                table: "MoreInfos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<List<int>>(
                name: "MoreInfosIds",
                table: "Announcements",
                type: "integer[]",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoreInfos_AnnouncementId",
                table: "MoreInfos",
                column: "AnnouncementId");

            migrationBuilder.AddForeignKey(
                name: "FK_MoreInfos_Announcements_AnnouncementId",
                table: "MoreInfos",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id");
        }
    }
}
