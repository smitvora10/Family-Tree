using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace FamilyTree.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonImageField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Person");

            migrationBuilder.AddColumn<byte[]>(
                name: "PersonImage",
                table: "Person",
                type: "longblob",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonImage",
                table: "Person");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Person",
                type: "longblob",
                nullable: false,
                defaultValue: Array.Empty<byte>());
        }
    }
}
