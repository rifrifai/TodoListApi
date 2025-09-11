using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos;
using todo.Models;

namespace todo.Services;

public interface ITodoService
{
    Task<IEnumerable<TodoItem>> GetAllTodosAsync(TodoQueryParameters queryParameters);
    Task<TodoItem?> GetTodoByIdAsync(Guid id);
    Task CreateTodoAsync(TodoItem item);
    Task<bool> PatchTodoAsync(Guid id, UpdateTodoDto patchDto);
    Task<bool> UpdateTodoAsync(Guid id, TodoItem item);
    Task<bool> DeleteTodoAsync(Guid id);
}
