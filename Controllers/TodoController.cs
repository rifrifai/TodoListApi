using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo.Models;
using todo.Services;
using todo.Dtos;

namespace todo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _service;
    public TodoController(ITodoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos([FromQuery] TodoQueryParameters queryParameters)
    {
        var todos = await _service.GetAllTodosAsync(queryParameters);
        return Ok(todos);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodo(Guid id)
    {
        var todo = await _service.GetTodoByIdAsync(id);
        if (todo == null) return NotFound();

        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodo(CreateTodoDto todoDto)
    {
        var todoItem = new TodoItem
        {
            Title = todoDto.Title,
            Desc = todoDto.Desc,
            DueDate = todoDto.DueDate,
            Priority = todoDto.Priority
        };

        await _service.CreateTodoAsync(todoItem);

        return CreatedAtAction(nameof(GetTodo), new { id = todoItem.Id }, todoItem);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTodo(Guid id, [FromBody] UpdateTodoDto patchDto)
    {
        var wasUpdated = await _service.PatchTodoAsync(id, patchDto);
        if (!wasUpdated) return NotFound("ID Todo tidak ditemukan!");

        return Ok("Todo berhasil diupdate sebagian (patch).");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodo(Guid id, CreateTodoDto todoDto)
    {
        var todoItem = new TodoItem
        {
            Id = id,
            Title = todoDto.Title,
            Desc = todoDto.Desc,
            DueDate = todoDto.DueDate,
            Priority = todoDto.Priority,
            IsCompleted = false
        };

        var wasUpdated = await _service.UpdateTodoAsync(id, todoItem);

        if (!wasUpdated) return NotFound("ID tidak ditemukan");

        return Ok(todoItem);
    }
    // public async Task<IActionResult> PutTodo(Guid id, TodoItem todoItem)
    // {
    //     if (id != todoItem.Id) return BadRequest("ID tidak ditemukan");

    //     await _service.UpdateTodoAsync(id, todoItem);

    //     return Ok("Todo berhasil di ubah!");
    // }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        var wasDeleted = await _service.DeleteTodoAsync(id);
        if (!wasDeleted) return NotFound("ID tidak ditemukan");

        return Ok("Todo berhasil dihapus");
    }
}