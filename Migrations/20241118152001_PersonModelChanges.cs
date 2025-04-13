using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyTree.Migrations
{
    /// <inheritdoc />
    public partial class PersonModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDeath",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "OccupationId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "QualificationId",
                table: "Person");

            migrationBuilder.AddColumn<string>(
                name: "IsMainPerson",
                table: "Person",
                type: "char(1)",
                maxLength: 1,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Person",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OfficeAddress",
                table: "Person",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Qualification",
                table: "Person",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMainPerson",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "OfficeAddress",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Qualification",
                table: "Person");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeath",
                table: "Person",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OccupationId",
                table: "Person",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QualificationId",
                table: "Person",
                type: "int",
                nullable: true);
        }
    }
}
