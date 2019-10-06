using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EdsTriathlonStuff.Migrations
{
    public partial class saveready : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalMeters",
                table: "Workouts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalYards",
                table: "Workouts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalMeters",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "TotalYards",
                table: "Workouts");
        }
    }
}
