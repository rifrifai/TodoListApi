using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos;
using todo.Models;

namespace todo.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync(TodoQueryParameters queryParameters);
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task AddAsync(TodoItem item);
    Task <bool>UpdateAsync(TodoItem item);
    Task<bool> DeleteAsync(Guid id);
}