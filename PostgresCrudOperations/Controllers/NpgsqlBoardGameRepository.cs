using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostgresCrudOperations.Controllers
{
    public class NpgsqlBoardGameRepository : IBoardGameRepository
    {
        private const string CONNECTION_STRING = "Host=localhost:5455;Username=postgresUser;Password=postgresPW;Database=postgresDB";

        private const string TABLE_NAME = "Games";

        private NpgsqlConnection connection;

        public NpgsqlBoardGameRepository()
        {
            connection = new NpgsqlConnection(CONNECTION_STRING);
            connection.Open();
        }

        public async Task Add(BoardGame game)
        {
            string commandText = $"INSERT INTO {TABLE_NAME} (id, Name, MinPlayers, MaxPlayers, AverageDuration) VALUES (@id, @name, @minPl, @maxPl, @avgDur)";
            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", game.Id);
                cmd.Parameters.AddWithValue("name", game.Name);
                cmd.Parameters.AddWithValue("minPl", game.MinPlayers);
                cmd.Parameters.AddWithValue("maxPl", game.MaxPlayers);
                cmd.Parameters.AddWithValue("avgDur", game.AverageDuration);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task Delete(int id)
        {
            await using (var cmd = new NpgsqlCommand($"DELETE FROM {TABLE_NAME} WHERE ID=(@p)", connection))
            {
                cmd.Parameters.AddWithValue("p", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<BoardGame> Get(int id)
        {
            await using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM {TABLE_NAME} WHERE ID = @id", connection))
            {
                cmd.Parameters.AddWithValue("id", id);

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        BoardGame game = ReadBoardGame(reader);
                        return game;
                    }
            }
            return null;
        }

        public async Task<IEnumerable<BoardGame>> GetAll()
        {
            List<BoardGame> games = new List<BoardGame>();

            await using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM {TABLE_NAME}", connection))
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    BoardGame game = ReadBoardGame(reader);
                    games.Add(game);
                }

            return games;
        }

        public async Task Update(int id, BoardGame game)
        {
            var commandText = $@"UPDATE {TABLE_NAME}
                        SET Name = @name, MinPlayers = @minPl, MaxPlayers = @maxPl, AverageDuration = @avgDur
                        WHERE id = @id";
            await using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", game.Id);
                cmd.Parameters.AddWithValue("name", game.Name);
                cmd.Parameters.AddWithValue("minPl", game.MinPlayers);
                cmd.Parameters.AddWithValue("maxPl", game.MaxPlayers);
                cmd.Parameters.AddWithValue("avgDur", game.AverageDuration);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<string> GetVersion()
        {
            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, connection);

            var versionObject = await cmd.ExecuteScalarAsync();

            return versionObject.ToString();
        }

        public async Task CreateTableIfNotExists()
        {
            await CreateTable();
        }

        private static BoardGame ReadBoardGame(NpgsqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string n = reader["name"] as string;
            short? minPlayers = reader["minplayers"] as Int16?;
            short? maxPlayers = reader["maxplayers"] as Int16?;
            short? averageDuration = reader["averageduration"] as Int16?;

            BoardGame game = new BoardGame
            {
                Id = id.Value,
                Name = n,
                MinPlayers = minPlayers.Value,
                MaxPlayers = maxPlayers.Value,
                AverageDuration = averageDuration.Value
            };
            return game;
        }

        private async Task CreateTable()
        {
            var sql = $"CREATE TABLE if not exists {TABLE_NAME}(id serial PRIMARY KEY, Name VARCHAR (200) NOT NULL, MinPlayers SMALLINT NOT NULL, MaxPlayers SMALLINT, AverageDuration SMALLINT)";

            using var cmd = new NpgsqlCommand(sql, connection);

            await cmd.ExecuteScalarAsync();
        }
    }
}