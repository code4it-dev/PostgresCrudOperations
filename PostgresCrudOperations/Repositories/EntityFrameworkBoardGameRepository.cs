using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostgresCrudOperations.Repositories
{

    public class BoardGamesContext : DbContext
    {
        public DbSet<BoardGame> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(CONNECTION_STRING);
        private const string CONNECTION_STRING = "Host=localhost:5455;" +
                 "Username=postgresUser;" +
                 "Password=postgresPW;" +
                 "Database=postgresDB";
    }

    public class EntityFrameworkBoardGameRepository : IBoardGameRepository
    {
        //Npgsql.EntityFrameworkCore.PostgreSQL (usa versione 5.0.10)


        //private const string CONNECTION_STRING = "Host=localhost:5455;" +
        //         "Username=postgresUser;" +
        //         "Password=postgresPW;" +
        //         "Database=postgresDB";

        private const string TABLE_NAME = "Games";
        public async Task Add(BoardGame game)
        {
            using (var db = new BoardGamesContext())
            {
                await db.Games.AddAsync(game);
            }
        }

        public Task CreateTableIfNotExists()
        {
            return Task.CompletedTask;
        }

        public async Task Delete(int id)
        {
            using (var db = new BoardGamesContext())
            {
                var game = await db.Games.FindAsync(id);
                if (game == null) return;

                db.Games.Remove(game);
            }
        }

        public async Task<BoardGame> Get(int id)
        {
            using (var db = new BoardGamesContext())
            {
                return await db.Games.FindAsync(id);
            }
        }

        public async Task<IEnumerable<BoardGame>> GetAll()
        {
            using (var db = new BoardGamesContext())
            {
                return await db.Games.ToListAsync();
            }
        }

        public Task<string> GetVersion()
        {
            return Task.FromResult("");

        }

        public async Task Update(int id, BoardGame game)
        {
            using (var db = new BoardGamesContext())
            {
                db.Games.Update(game);
            }
        }
    }
}