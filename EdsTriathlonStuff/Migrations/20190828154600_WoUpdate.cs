using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EdsTriathlonStuff.Migrations
{
    public partial class WoUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AthleteName",
                table: "Workouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachName",
                table: "Workouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Workouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Distance",
                table: "SwimSets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "SwimSets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rest",
                table: "SwimSets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "SwimSets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkoutSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Index = table.Column<int>(nullable: false),
                    SetId = table.Column<int>(nullable: false),
                    WorkoutId = table.Column<int>(nullable: false),
                    WorkoutSection = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutSets");

            migrationBuilder.DropColumn(
                name: "AthleteName",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "CoachName",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "SwimSets");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "SwimSets");

            migrationBuilder.DropColumn(
                name: "Rest",
                table: "SwimSets");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "SwimSets");
        }
    }
}
