using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public TodoItemsController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            //return await _context.TodoItems.Select(x=> ItemToDTO(x)).ToListAsync();
            // return await _context.TodoItems.Select(x => _mapper.Map<TodoItemDTO>(x)).ToListAsync();
            var res = await _context.TodoItems.ToListAsync();
            return Ok(_mapper.Map<List<TodoItemDTO>>(res)); 
        }

        //private static TodoItemDTO ItemToDTO(TodoItem x)
        //{
        //    return new TodoItemDTO
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        IsComplete = x.IsComplete
        //    };
        //}

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            //return Ok(ItemToDTO(todoItem));
            // var todoItemDTO = _mapper.Map<TodoItem, TodoItemDTO>(todoItem);
            var todoItemDTO = _mapper.Map<TodoItemDTO>(todoItem);
            return Ok(todoItemDTO);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
        {
            if (id != todoDTO.Id)  {
                return BadRequest();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            //todoItem.Name = todoDTO.Name;
            //todoItem.IsComplete = todoDTO.IsComplete;
            _mapper.Map<TodoItemDTO, TodoItem>(todoDTO, todoItem);
            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoDTO)
        {
            //var todoItem = new TodoItem()
            //{
            //    Id = todoDTO.Id,
            //    Name = todoDTO.Name,
            //    IsComplete = todoDTO.IsComplete
            //};
            var todoItem = _mapper.Map<TodoItem>(todoDTO);
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            todoItem = await _context.TodoItems.FindAsync(todoDTO.Id);
            if (todoItem == null)
            {
                return NotFound("Todo item not found!");
            }
            //return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ItemToDTO(todoItem));
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, _mapper.Map<TodoItemDTO>(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
