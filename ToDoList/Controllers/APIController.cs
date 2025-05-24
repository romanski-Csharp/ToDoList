using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoService _service;

        public ToDoController(ToDoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ToDoItem>>> Get()
        {
            var username = User.Identity?.Name;
            var items = await _service.GetByUserAsync(username!);
            return items;
        }


        [HttpPost]
        public async Task<IActionResult> Create(ToDoItem item)
        {
            var username = User.Identity?.Name; // ?? отримуємо ім'я з токена
            item.UserId = username!;
            await _service.CreateAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ToDoItem item)
        {
            var existing = await _service.GetAsync(id);
            if (existing is null) return NotFound();

            item.Id = existing.Id;
            item.UserId = existing.UserId;
            await _service.UpdateAsync(id, item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _service.GetAsync(id);
            if (existing is null) return NotFound();

            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
