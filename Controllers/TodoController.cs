using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo.Models;
using todo.Services;
using todo.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace todo.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly ITodoService _service;
    public TodoController(ITodoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos([FromQuery] TodoQueryParameters queryParameters)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized("User ID tidak ditemukan");
        var userId = int.Parse(userIdString);

        var todos = await _service.GetAllTodosAsync(userId, queryParameters);
        return Ok(todos);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> GetTodo(Guid id)
    {
        var todo = await _service.GetTodoByIdAsync(id);
        if (todo == null) return NotFound();

        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> PostTodo([FromBody] CreateTodoDto createTodoDto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized("User ID tidak ditemukan");
        var userId = int.Parse(userIdString);

        var newTodo = await _service.CreateTodoAsync(createTodoDto, userId);

        var result = CreatedAtAction(nameof(GetTodo), new { id = newTodo.Id }, newTodo);
        return result;
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<TodoDto>> PatchTodo(Guid id, [FromBody] UpdateTodoDto patchDto)
    {
        var wasUpdated = await _service.PatchTodoAsync(id, patchDto);
        if (wasUpdated == null) return NotFound("ID Todo tidak ditemukan!");

        return Ok(wasUpdated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        var wasDeleted = await _service.DeleteTodoAsync(id);
        if (!wasDeleted) return NotFound("ID tidak ditemukan");

        return Ok("Todo berhasil dihapus");
    }
}