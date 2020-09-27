using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItem
        [HttpGet]
        //Using DTO
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItems.Select(todo => new TodoItemDTO
            {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete

            }).ToListAsync();
        }


        //public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(){

        //    return await _context.TodoItems.ToListAsync();
        //}

        

        // GET: api/TodoItem/5
        [HttpGet("{id}")]
        //Using DTO
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(int id)
        {
            TodoItem todo = await _context.TodoItems.FirstOrDefaultAsync(c => c.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            TodoItemDTO itemDTO = new TodoItemDTO
            {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete,
            };

            return itemDTO;

            
        }


        //public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        //{
        //   TodoItem item= await   _context.TodoItems.FirstOrDefaultAsync(c => c.Id == id);

        //    if (item == null)
        //    {
        //        return NotFound();
        //    }
        //    return item;
        //}

       

        // PUT: api/TodoItem/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItem
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]

        //Using DTO

        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO itemDTO)
        {
            TodoItem todo = new TodoItem
            {
                Id = itemDTO.Id,
                Name = itemDTO.Name,
                IsComplete = itemDTO.IsComplete,

            };
            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItems), new { id = todo.Id }, todo);
        }
       

        //public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        //{
        //    _context.TodoItems.Add(todoItem);
        //  await  _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetTodoItems), new {id=todoItem.Id },todoItem);
        //}

       

        // DELETE: api/TodoItem/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
