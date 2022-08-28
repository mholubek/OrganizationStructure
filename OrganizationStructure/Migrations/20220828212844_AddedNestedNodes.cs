using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationStructure.Migrations
{
    public partial class AddedNestedNodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Divisions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DivisionId",
                table: "Projects",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_CompanyId",
                table: "Divisions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ProjectId",
                table: "Departments",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Projects_ProjectId",
                table: "Departments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Companies_CompanyId",
                table: "Divisions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Divisions_DivisionId",
                table: "Projects",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Projects_ProjectId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Companies_CompanyId",
                table: "Divisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Divisions_DivisionId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_DivisionId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Divisions_CompanyId",
                table: "Divisions");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ProjectId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Departments");
        }
    }
}
