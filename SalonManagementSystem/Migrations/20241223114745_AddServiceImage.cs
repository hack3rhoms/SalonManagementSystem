using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonManagementSystem.Migrations
{
    public partial class AddServiceImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Services",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "Services",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Services");
        }
    }
}
