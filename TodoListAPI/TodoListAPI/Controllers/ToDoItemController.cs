using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Data;
using TodoListAPI.Models;
using TodoListAPI.Services;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService _service;

        public ToDoItemController(IToDoItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ToDoItem>> GetAll()
        {
            var items = _service.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<ToDoItem> GetById(int id)
        {
            var item = _service.GetById(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ToDoItem> Add(ToDoItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdItem = _service.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = createdItem.id }, createdItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, ToDoItem item)
        {
            if (id != item.id)
                return BadRequest("Invalid ID!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateItem = _service.Update(id, item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
                var deleted = _service.Delete(id);
                return deleted ? NoContent() : NotFound();
        }
    }
}
