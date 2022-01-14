using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostgresCrudOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardGameController : ControllerBase
    {
        private readonly IBoardGameRepository _boardGameRepository;

        public BoardGameController(IBoardGameRepository boardGameRepository)
        {
            _boardGameRepository = boardGameRepository;

            _boardGameRepository.CreateTableIfNotExists().GetAwaiter();
            var version = _boardGameRepository.GetVersion().GetAwaiter().GetResult();
        }

        // GET: api/<BoardGameController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardGame>>> Get()
        {
            var allGames = await _boardGameRepository.GetAll();
            return Ok(allGames);
        }

        // GET api/<BoardGameController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardGame>> Get(int id)
        {
            var game = await _boardGameRepository.Get(id);
            if (game != null)
                return Ok(game);
            else
                return NotFound();
        }

        // POST api/<BoardGameController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BoardGame game)
        {
            await _boardGameRepository.Add(game);
            return Ok();
        }

        // PUT api/<BoardGameController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] BoardGame game)
        {
            await _boardGameRepository.Update(id, game);
            return Ok();
        }

        // DELETE api/<BoardGameController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _boardGameRepository.Delete(id);
            return Ok();
        }
    }
}