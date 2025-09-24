using Microsoft.AspNetCore.Mvc;
using UsersCRUD.Models;
using UsersCRUD.Services;

namespace UsersCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user is null) return NotFound();

            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            var created = await _service.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(string id, User user)
        {
            if (id != user.Id) return BadRequest("El ID de la URL no coincide con el del objeto");

            var updated = await _service.UpdateAsync(user);
            if (updated is null) return NotFound();

            return Ok(updated);
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
