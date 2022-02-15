using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostgresCrudOperations.Repositories
{
    public class DapperBoardGameRepository : IBoardGameRepository
    {
        /*
         Install Dapper:
         */

        private const string CONNECTION_STRING = "Host=localhost:5455;" +
                    "Username=postgresUser;" +
                    "Password=postgresPW;" +
                    "Database=postgresDB";

        private const string TABLE_NAME = "Games";
        private readonly NpgsqlConnection connection;

        public DapperBoardGameRepository()
        {
            connection = new NpgsqlConnection(CONNECTION_STRING);
            connection.Open();
        }

        public async Task Add(BoardGame game)
        {
            string commandText = $"INSERT INTO {TABLE_NAME} (id, Name, MinPlayers, MaxPlayers, AverageDuration) VALUES (@id, @name, @minPl, @maxPl, @avgDur)";

            var queryArguments = new
            {
                id = game.Id,
                name = game.Name,
                minPl = game.MinPlayers,
                maxPl = game.MaxPlayers,
                avgDur = game.AverageDuration
            };

            await connection.ExecuteAsync(commandText, queryArguments);
        }

        public async Task CreateTableIfNotExists()
        {
            var sql = $"CREATE TABLE if not exists {TABLE_NAME}" +
                     $"(" +
                     $"id serial PRIMARY KEY, " +
                     $"Name VARCHAR (200) NOT NULL, " +
                     $"MinPlayers SMALLINT NOT NULL, " +
                     $"MaxPlayers SMALLINT, " +
                     $"AverageDuration SMALLINT" +
                     $")";

            await connection.ExecuteAsync(sql);
        }

        public async Task Delete(int id)
        {
            string commandText = $"DELETE FROM {TABLE_NAME} WHERE ID=(@p)";

            var queryArguments = new
            {
                p = id
            };

            await connection.ExecuteAsync(commandText, queryArguments);
        }

        public async Task<BoardGame> Get(int id)
        {
            string commandText = $"SELECT * FROM {TABLE_NAME} WHERE ID = @id";

            var queryArgs = new { Id = id };
            var game = await connection.QueryFirstAsync<BoardGame>(commandText, queryArgs);
            return game;
        }

        public async Task<IEnumerable<BoardGame>> GetAll()
        {
            string commandText = $"SELECT * FROM {TABLE_NAME}";
            var games = await connection.QueryAsync<BoardGame>(commandText);

            return games;
        }

        public async Task<string> GetVersion()
        {
            var commandText = "SELECT version()";

            var value = await connection.ExecuteScalarAsync<string>(commandText);
            return value;
        }

        public async Task Update(int id, BoardGame game)
        {
            var commandText = $@"UPDATE {TABLE_NAME}
                        SET Name = @name, MinPlayers = @minPl, MaxPlayers = @maxPl, AverageDuration = @avgDur
                        WHERE id = @id";

            var queryArgs = new
            {
                id = game.Id,
                name = game.Name,
                minPl = game.MinPlayers,
                maxPl = game.MaxPlayers,
                avgDur = game.AverageDuration
            };

            await connection.ExecuteAsync(commandText, queryArgs);
        }
    }
}