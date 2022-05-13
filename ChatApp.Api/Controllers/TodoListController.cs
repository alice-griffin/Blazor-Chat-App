using ChatApp.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoListController : Controller
    {
        private TodoListContext context;

        private List<TodoListItem> todoItems = new List<TodoListItem>()
        {
            new TodoListItem { Item = "make doc appt", Id = 1 },
            new TodoListItem { Item = "something else", Id = 2 }
        };

        public TodoListController(TodoListContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<IActionResult> GetToDos()
        {
            try
            {
                var todos = await context.TodoItems.ToListAsync();
                return Ok(todos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occured retrieving items");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todoItem = await context.TodoItems.FirstOrDefaultAsync(item => item.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(TodoListItem todoItem)
        {
            if (todoItem == null)
            {
                return NotFound();
            }

            try
            {
                context.Add(todoItem);
                await context.SaveChangesAsync();
                return Ok(todoItem.Id);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating item: {e}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, TodoListItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest($"item with id {id} does not match item to update");
            }

            if (!context.TodoItems.Contains(todoItem))
            {
                return NotFound();
            }

            context.Entry(todoItem).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new DbUpdateConcurrencyException();
            }

            return Ok(todoItem.Id);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItemToDelete = await context.TodoItems.FirstOrDefaultAsync(item => item.Id == id);

            context.Remove(todoItemToDelete);
            await context.SaveChangesAsync();
            return Ok(id);
        }
    }
}
