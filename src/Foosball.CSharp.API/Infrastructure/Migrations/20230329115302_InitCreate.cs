using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foosball.CSharp.API.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    first_player_id = table.Column<Guid>(type: "uuid", nullable: true),
                    second_player_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teams", x => x.id);
                    table.ForeignKey(
                        name: "fk_teams_players_first_player_id",
                        column: x => x.first_player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_teams_players_player_id",
                        column: x => x.first_player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teams_players_player_id1",
                        column: x => x.second_player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    winner_team_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                    table.ForeignKey(
                        name: "fk_games_team_team_temp_id",
                        column: x => x.winner_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_a_id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_b_id = table.Column<Guid>(type: "uuid", nullable: false),
                    team_a_score = table.Column<int>(type: "integer", nullable: false),
                    team_b_score = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    winner_team_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sets", x => x.id);
                    table.ForeignKey(
                        name: "fk_set_games_finished_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sets_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sets_team_team_temp_id",
                        column: x => x.team_a_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sets_team_team_temp_id1",
                        column: x => x.team_b_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sets_team_team_temp_id2",
                        column: x => x.winner_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_games_started_at",
                table: "games",
                column: "started_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "ix_games_winner_team_id",
                table: "games",
                column: "winner_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_sets_game_id",
                table: "sets",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_sets_team_a_id",
                table: "sets",
                column: "team_a_id");

            migrationBuilder.CreateIndex(
                name: "ix_sets_team_b_id",
                table: "sets",
                column: "team_b_id");

            migrationBuilder.CreateIndex(
                name: "ix_sets_winner_team_id",
                table: "sets",
                column: "winner_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_teams_first_player_id",
                table: "teams",
                column: "first_player_id");

            migrationBuilder.CreateIndex(
                name: "ix_teams_name",
                table: "teams",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teams_second_player_id",
                table: "teams",
                column: "second_player_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sets");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
