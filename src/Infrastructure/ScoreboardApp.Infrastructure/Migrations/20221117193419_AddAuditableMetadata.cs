using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreboardApp.Infrastructure.Migrations
{
    public partial class AddAuditableMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "HabitTrackers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "HabitTrackers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "HabitTrackers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "HabitTrackers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "EffortHabits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EffortHabits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "EffortHabits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "EffortHabits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "EffortHabitEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "EffortHabitEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "EffortHabitEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "EffortHabitEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CompletionHabits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CompletionHabits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CompletionHabits",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CompletionHabits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CompletionHabitEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CompletionHabitEntries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CompletionHabitEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CompletionHabitEntries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "HabitTrackers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "HabitTrackers");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "HabitTrackers");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "HabitTrackers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "EffortHabits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EffortHabits");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "EffortHabits");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "EffortHabits");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "EffortHabitEntries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EffortHabitEntries");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "EffortHabitEntries");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "EffortHabitEntries");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CompletionHabits");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CompletionHabits");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CompletionHabits");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CompletionHabits");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CompletionHabitEntries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CompletionHabitEntries");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CompletionHabitEntries");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CompletionHabitEntries");
        }
    }
}
