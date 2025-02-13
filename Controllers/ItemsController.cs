using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rest_api.Models;

namespace rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly List<Item> Items =
        [
            new Item { Id = 1, Name = "Item1", Description = "Description for Item1" },
            new Item { Id = 2, Name = "Item2", Description = "Description for Item2" },
            new Item { Id = 3, Name = "Item3", Description = "Description for Item3" }
        ];

        // GET: api/items
        // Доступно для ролей Admin та User
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public ActionResult<IEnumerable<Item>> GetItems()
        {
            return Ok(Items);
        }

        // GET: api/items/{id}
        // Доступно для ролей Admin та User
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public ActionResult<Item> GetItem(int id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        // POST: api/items
        // Доступно лише для Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Item> CreateItem([FromBody] Item newItem)
        {
            if (Items.Any(i => i.Id == newItem.Id))
                return BadRequest("Item with the same Id already exists.");

            Items.Add(newItem);

            return CreatedAtAction(nameof(GetItem), new { id = newItem.Id }, newItem);
        }

        // PATCH: api/items/{id}
        // Доступно лише для Admin
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Item> UpdateItem(int id, [FromBody] Item updatedItem)
        {
            var existingItem = Items.FirstOrDefault(i => i.Id == id);
            if (existingItem == null)
                return NotFound();

            existingItem.Name = updatedItem.Name ?? existingItem.Name;
            existingItem.Description = updatedItem.Description ?? existingItem.Description;

            return Ok(existingItem);
        }

        // DELETE: api/items/{id}
        // Доступно лише для Admin
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteItem(int id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();

            Items.Remove(item);

            return NoContent();
        }
    }
}
