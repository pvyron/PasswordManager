using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class logomodification20230125 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BulkStorageThumbnailName",
                table: "PasswordLogos");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "PasswordLogos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BulkStorageThumbnailName",
                table: "PasswordLogos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "PasswordLogos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
