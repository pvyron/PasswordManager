using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class passwordlogoaddition202301201332 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "PasswordLogos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "PasswordLogos");
        }
    }
}
