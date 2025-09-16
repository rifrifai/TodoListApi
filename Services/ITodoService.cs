using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos;
using todo.Models;

namespace todo.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoDto>> GetAllTodosAsync(int? userId, TodoQueryParameters queryParameters);
    Task<TodoDto?> GetTodoByIdAsync(Guid id);
    Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto, int userId);
    Task<TodoDto?> PatchTodoAsync(Guid id, UpdateTodoDto updateTodoDto);
    Task<bool> DeleteTodoAsync(Guid id);
}
