using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyTree.Migrations
{
    /// <inheritdoc />
    public partial class RequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDatetime",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "ModificationDatetime",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "CreationDatetime",
                table: "RelationType");

            migrationBuilder.DropColumn(
                name: "ModificationDatetime",
                table: "RelationType");

            migrationBuilder.DropColumn(
                name: "CreationDatetime",
                table: "RelationMapping");

            migrationBuilder.DropColumn(
                name: "ModificationDatetime",
                table: "RelationMapping");

            migrationBuilder.DropColumn(
                name: "CreationDatetime",
                table: "Qualification");

            migrationBuilder.DropColumn(
                name: "ModificationDatetime",
                table: "Qualification");

            migrationBuilder.DropColumn(
                name: "CreationDatetime",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "ModificationDatetime",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "CreationDatetime",
                table: "Occupation");

            migrationBuilder.DropColumn(
                name: "ModificationDatetime",
                table: "Occupation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDatetime",
                table: "UserRole",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDatetime",
                table: "UserRole",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDatetime",
                table: "RelationType",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDatetime",
                table: "RelationType",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDatetime",
                table: "RelationMapping",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDatetime",
                table: "RelationMapping",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDatetime",
                table: "Qualification",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDatetime",
                table: "Qualification",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDatetime",
                table: "Person",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDatetime",
                table: "Person",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDatetime",
                table: "Occupation",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDatetime",
                table: "Occupation",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
