using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreboardApp.Infrastructure.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HabitTrackers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitTrackers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompletionHabits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    HabitTrackerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletionHabits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletionHabits_HabitTrackers_HabitTrackerId",
                        column: x => x.HabitTrackerId,
                        principalTable: "HabitTrackers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EffortHabits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AverageGoal = table.Column<double>(type: "float", nullable: true),
                    Subtype = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    HabitTrackerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EffortHabits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EffortHabits_HabitTrackers_HabitTrackerId",
                        column: x => x.HabitTrackerId,
                        principalTable: "HabitTrackers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletionHabitEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Completion = table.Column<bool>(type: "bit", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "date", nullable: false),
                    HabitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletionHabitEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletionHabitEntries_CompletionHabits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "CompletionHabits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EffortHabitEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Effort = table.Column<double>(type: "float", nullable: false),
                    SessionGoal = table.Column<double>(type: "float", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "date", nullable: false),
                    HabitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EffortHabitEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EffortHabitEntries_EffortHabits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "EffortHabits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletionHabitEntries_HabitId",
                table: "CompletionHabitEntries",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionHabits_HabitTrackerId",
                table: "CompletionHabits",
                column: "HabitTrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_EffortHabitEntries_HabitId",
                table: "EffortHabitEntries",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_EffortHabits_HabitTrackerId",
                table: "EffortHabits",
                column: "HabitTrackerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletionHabitEntries");

            migrationBuilder.DropTable(
                name: "EffortHabitEntries");

            migrationBuilder.DropTable(
                name: "CompletionHabits");

            migrationBuilder.DropTable(
                name: "EffortHabits");

            migrationBuilder.DropTable(
                name: "HabitTrackers");
        }
    }
}
