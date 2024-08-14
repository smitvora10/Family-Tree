using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyTree.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInModelNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VRT04s",
                table: "VRT04s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VRT03s",
                table: "VRT03s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VRT02s",
                table: "VRT02s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VRT01s",
                table: "VRT01s");

            migrationBuilder.RenameTable(
                name: "VRT04s",
                newName: "VRT04");

            migrationBuilder.RenameTable(
                name: "VRT03s",
                newName: "VRT03");

            migrationBuilder.RenameTable(
                name: "VRT02s",
                newName: "VRT02");

            migrationBuilder.RenameTable(
                name: "VRT01s",
                newName: "VRT01");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VRT04",
                table: "VRT04",
                column: "T04F01");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VRT03",
                table: "VRT03",
                column: "T03F01");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VRT02",
                table: "VRT02",
                column: "T02F01");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VRT01",
                table: "VRT01",
                column: "T01F01");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VRT04",
                table: "VRT04");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VRT03",
                table: "VRT03");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VRT02",
                table: "VRT02");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VRT01",
                table: "VRT01");

            migrationBuilder.RenameTable(
                name: "VRT04",
                newName: "VRT04s");

            migrationBuilder.RenameTable(
                name: "VRT03",
                newName: "VRT03s");

            migrationBuilder.RenameTable(
                name: "VRT02",
                newName: "VRT02s");

            migrationBuilder.RenameTable(
                name: "VRT01",
                newName: "VRT01s");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VRT04s",
                table: "VRT04s",
                column: "T04F01");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VRT03s",
                table: "VRT03s",
                column: "T03F01");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VRT02s",
                table: "VRT02s",
                column: "T02F01");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VRT01s",
                table: "VRT01s",
                column: "T01F01");
        }
    }
}
