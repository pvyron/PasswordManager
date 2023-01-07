using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class favoritepasswordcolumnconstraint202301071357 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Passwords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Passwords");
        }
    }
}
