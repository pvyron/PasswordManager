using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class imagedbmodification20230123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BulkStorageId",
                table: "PasswordLogos");

            migrationBuilder.RenameColumn(
                name: "Thumbnail",
                table: "PasswordLogos",
                newName: "ThumbnailUrl");

            migrationBuilder.AddColumn<string>(
                name: "BulkStorageImageName",
                table: "PasswordLogos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BulkStorageThumbnailName",
                table: "PasswordLogos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PasswordLogos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BulkStorageImageName",
                table: "PasswordLogos");

            migrationBuilder.DropColumn(
                name: "BulkStorageThumbnailName",
                table: "PasswordLogos");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PasswordLogos");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "PasswordLogos",
                newName: "Thumbnail");

            migrationBuilder.AddColumn<Guid>(
                name: "BulkStorageId",
                table: "PasswordLogos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
