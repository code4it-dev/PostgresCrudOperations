using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostgresCrudOperations.Controllers
{
    public interface IBoardGameRepository
    {
        Task<IEnumerable<BoardGame>> GetAll();

        Task<BoardGame> Get(int id);

        Task Add(BoardGame value);

        Task Update(int id, BoardGame game);

        Task Delete(int id);

        Task<string> GetVersion();

        Task CreateTableIfNotExists();
    }
}