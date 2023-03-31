using System.Data.Common;
using Dapper;
using Foosball.CSharp.API.Application.Queries.Models;
using Foosball.CSharp.Domain.GameAggregateModel;

namespace Foosball.CSharp.API.Application.Queries;

public class DapperGameQueries : IGameQueries
{
    private readonly DbConnection _connection;

    private const string GetGameDetailsQuery = @$"
        SELECT
            g.id AS {nameof(GameDetails.GameId)},
            g.status AS {nameof(GameDetails.Status)},
            ta.id AS {nameof(GameDetails.TeamAId)},
            tb.id AS {nameof(GameDetails.TeamBId)},
            ta.name AS {nameof(GameDetails.TeamAName)},
            tb.name AS {nameof(GameDetails.TeamBName)},
            g.winner_team_id AS {nameof(GameDetails.WinnerTeamId)},
            tgw.name AS {nameof(GameDetails.WinnerTeamName)},
            g.started_at AS {nameof(GameDetails.StartedAt)},
            s.id AS {nameof(SetDetails.SetId)},
            s.status AS {nameof(SetDetails.Status)},
            s.team_a_score AS {nameof(SetDetails.TeamAScore)},
            s.team_b_score AS {nameof(SetDetails.TeamBScore)},
            s.finished_at AS {nameof(SetDetails.FinishedAt)},
            s.winner_team_id AS {nameof(SetDetails.WinnerTeamId)},
            tsw.name AS {nameof(SetDetails.WinnerTeamName)}
        FROM games AS g
        LEFT JOIN sets AS s ON s.game_id = g.id
        LEFT JOIN teams AS ta ON ta.id = s.team_a_id
        LEFT JOIN teams AS tb ON tb.id = s.team_b_id
        LEFT JOIN teams AS tgw ON tgw.id = g.winner_team_id
        LEFT JOIN teams AS tsw ON tsw.id = s.winner_team_id
        WHERE g.id = @GameId
        GROUP BY g.id, s.id, ta.id, tb.id, tgw.id, tsw.id
        ORDER BY s.finished_at ASC NULLS LAST";

    private const string GetGameOverviewsQuery = @$"
        SELECT
            g.id AS {nameof(GameOverview.GameId)},
            g.status AS {nameof(GameOverview.Status)},
            s.count AS {nameof(GameOverview.CurrentSet)},
            ta.id AS {nameof(GameOverview.TeamAId)},
            ta.name AS {nameof(GameOverview.TeamAName)},
            sl.team_a_score AS {nameof(GameOverview.TeamACurrentScore)},
            tb.id AS {nameof(GameOverview.TeamBId)},
            tb.name AS {nameof(GameOverview.TeamBName)},
            sl.team_b_score AS {nameof(GameOverview.TeamBCurrentScore)},
            g.winner_team_id AS {nameof(GameOverview.WinnerTeamId)},
            tgw.name AS {nameof(GameOverview.WinnerTeamName)},
            g.started_at AS {nameof(GameOverview.StartedAt)}
        FROM games AS g
        LEFT JOIN (
                SELECT DISTINCT ON (game_id) game_id, finished_at, team_a_id, team_b_id, team_a_score, team_b_score
                FROM sets
                ORDER BY game_id, finished_at DESC NULLS FIRST)
            AS sl ON sl.game_id = g.id
        LEFT JOIN (SELECT COUNT(1) AS count, game_id FROM sets GROUP BY sets.game_id) AS s ON s.game_id = g.id
        LEFT JOIN teams AS ta ON ta.id = sl.team_a_id
        LEFT JOIN teams AS tb ON tb.id = sl.team_b_id
        LEFT JOIN teams AS tgw ON tgw.id = g.winner_team_id
        GROUP BY g.id, s.game_id, ta.id, tb.id, tgw.id, sl.team_a_score, sl.team_b_score, s.count
        ORDER BY g.started_at DESC";

    public DapperGameQueries(DbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task<GameDetails?> GetGameDetailsAsync(GameId gameId)
    {
        GameDetails? gameDetails = null;

        await _connection.QueryAsync<GameDetails, SetDetails, GameDetails>(
            sql: GetGameDetailsQuery,
            map: (game, set) =>
            {
                gameDetails ??= game;

                if (set is not null)
                {
                    gameDetails.Sets.Add(set);
                }

                return game;
            },
            param: new { GameId = gameId.Value },
            splitOn: nameof(SetDetails.SetId));

        return gameDetails;
    }

    // As async stream to avoid loading much data into memory
    public async IAsyncEnumerable<GameOverview> GetGameOverviewsAsync()
    {
        using var reader = await _connection.ExecuteReaderAsync(GetGameOverviewsQuery).ConfigureAwait(false);

        var rowParser = reader.GetRowParser<GameOverview>();

        while (await reader.ReadAsync().ConfigureAwait(false))
            yield return rowParser(reader);
    }
}
