using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonManagementSystem.Migrations
{
    public partial class data2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Employees",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
