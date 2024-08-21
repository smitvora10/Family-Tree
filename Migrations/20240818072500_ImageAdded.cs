using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyTree.Migrations
{
    /// <inheritdoc />
    public partial class ImageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDatetime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ModificationDatetime",
                table: "User");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Person",
                type: "longblob",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Person");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDatetime",
                table: "User",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDatetime",
                table: "User",
                type: "DATETIME",
                nullable: true);
        }
    }
}
