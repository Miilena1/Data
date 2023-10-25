using DataAPI.DataBase.Repositories;
using DataAPI.DataBase.Tables;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HumanController : ControllerBase
    {
        private readonly HumanRepository _humanRepository;

        public HumanController(HumanRepository humanRepository)
        {
            _humanRepository = humanRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var humans = _humanRepository.GetAll();
            return Ok(humans);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var human = _humanRepository.GetById(id);

            if (human == null)
                return NotFound();

            return Ok(human);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Human human)
        {
            if (human == null)
                return BadRequest();

            _humanRepository.Insert(human);
            return CreatedAtAction(nameof(GetById), new { id = human.Id }, human);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _humanRepository.Delete(id);
            return NoContent();
        }
    }
}