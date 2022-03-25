using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostgresCrudOperations.Repositories
{
    public interface IBoardGameRepository
    {
        Task<IEnumerable<BoardGame>> GetAll();

        Task<BoardGame> Get(int id);

        Task Add(BoardGame game);

        Task Update(int id, BoardGame game);

        Task Delete(int id);
    }

    public interface IAdditionalDbOperations
    {
        Task<string> GetVersion();

        Task CreateTableIfNotExists();
    }
}