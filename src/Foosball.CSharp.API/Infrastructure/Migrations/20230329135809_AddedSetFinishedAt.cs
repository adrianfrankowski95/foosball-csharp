using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foosball.CSharp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedSetFinishedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_team_team_temp_id",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "fk_sets_team_team_temp_id",
                table: "sets");

            migrationBuilder.DropForeignKey(
                name: "fk_sets_team_team_temp_id1",
                table: "sets");

            migrationBuilder.DropForeignKey(
                name: "fk_sets_team_team_temp_id2",
                table: "sets");

            migrationBuilder.AddColumn<DateTime>(
                name: "finished_at",
                table: "sets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_games_teams_team_temp_id",
                table: "games",
                column: "winner_team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sets_teams_team_temp_id",
                table: "sets",
                column: "team_a_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sets_teams_team_temp_id1",
                table: "sets",
                column: "team_b_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sets_teams_team_temp_id2",
                table: "sets",
                column: "winner_team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_teams_team_temp_id",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "fk_sets_teams_team_temp_id",
                table: "sets");

            migrationBuilder.DropForeignKey(
                name: "fk_sets_teams_team_temp_id1",
                table: "sets");

            migrationBuilder.DropForeignKey(
                name: "fk_sets_teams_team_temp_id2",
                table: "sets");

            migrationBuilder.DropColumn(
                name: "finished_at",
                table: "sets");

            migrationBuilder.AddForeignKey(
                name: "fk_games_team_team_temp_id",
                table: "games",
                column: "winner_team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sets_team_team_temp_id",
                table: "sets",
                column: "team_a_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sets_team_team_temp_id1",
                table: "sets",
                column: "team_b_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sets_team_team_temp_id2",
                table: "sets",
                column: "winner_team_id",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
