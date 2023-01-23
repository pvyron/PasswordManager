using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class passwordlogoaddition20230120 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Passwords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PasswordLogos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BulkStorageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordLogos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_ImageId",
                table: "Passwords",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_PasswordLogos_ImageId",
                table: "Passwords",
                column: "ImageId",
                principalTable: "PasswordLogos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_PasswordLogos_ImageId",
                table: "Passwords");

            migrationBuilder.DropTable(
                name: "PasswordLogos");

            migrationBuilder.DropIndex(
                name: "IX_Passwords_ImageId",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Passwords");
        }
    }
}
