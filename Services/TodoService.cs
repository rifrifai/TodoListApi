using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using todo.Dtos;
using todo.Models;
using todo.Repositories;

namespace todo.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repo;
    public TodoService(ITodoRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<TodoItem>> GetAllTodosAsync(TodoQueryParameters queryParameters)
    {
        return _repo.GetAllAsync(queryParameters);
    }

    public Task<TodoItem?> GetTodoByIdAsync(Guid id)
    {
        return _repo.GetByIdAsync(id);
    }

    public Task CreateTodoAsync(TodoItem item)
    {
        if (string.IsNullOrWhiteSpace(item.Title) && string.IsNullOrWhiteSpace(item.Desc))
            throw new ArgumentException("judul dan deskripsi tidak boleh kosong");

        if (!item.DueDate.HasValue) item.DueDate = DateTime.UtcNow.AddDays(1);
        return _repo.AddAsync(item);
    }

    public async Task<bool> PatchTodoAsync(Guid id, UpdateTodoDto patchDto)
    {
        var existingItem = await _repo.GetByIdAsync(id);
        if (existingItem == null) return false;

        if (patchDto.Title != null) existingItem.Title = patchDto.Title;

        if (patchDto.Desc != null) existingItem.Desc = patchDto.Desc;

        if (patchDto.IsCompleted.HasValue) existingItem.IsCompleted = patchDto.IsCompleted.Value;

        if (patchDto.DueDate.HasValue) existingItem.DueDate = patchDto.DueDate.Value;

        if (patchDto.Priority.HasValue) existingItem.Priority = patchDto.Priority.Value;

        var result = await _repo.UpdateAsync(existingItem);
        return result;
    }

    public async Task<bool> UpdateTodoAsync(Guid id, TodoItem item)
    {
        var existingItem = await _repo.GetByIdAsync(id);
        if (existingItem == null)
        {
            return false;
        }

        existingItem.Title = item.Title;
        existingItem.Desc = item.Desc;
        existingItem.IsCompleted = item.IsCompleted;

        var result = await _repo.UpdateAsync(existingItem);
        return result;
    }

    public Task<bool> DeleteTodoAsync(Guid id)
    {
        return _repo.DeleteAsync(id);
    }
}
