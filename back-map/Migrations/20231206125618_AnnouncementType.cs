using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_map.Migrations
{
    /// <inheritdoc />
    public partial class AnnouncementType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnouncementType",
                table: "Announcements",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementType",
                table: "Announcements");
        }
    }
}
