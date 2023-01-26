using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class setupfordefaultlogo20230126 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "PasswordLogos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "PasswordLogos");
        }
    }
}
