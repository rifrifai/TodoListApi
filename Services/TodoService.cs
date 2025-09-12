using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using todo.Dtos;
using todo.Models;
using todo.Repositories;

namespace todo.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repo;
    private readonly IMapper _mapper;
    public TodoService(ITodoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoDto>> GetAllTodosAsync(TodoQueryParameters queryParameters)
    {
        var todoItems = await _repo.GetAllAsync(queryParameters);
        var result = _mapper.Map<IEnumerable<TodoDto>>(todoItems);
        return result;
    }

    public async Task<TodoDto?> GetTodoByIdAsync(Guid id)
    {
        var todoItem = await _repo.GetByIdAsync(id);
        if (todoItem == null) return null;
        var result = _mapper.Map<TodoDto>(todoItem);
        return result;
    }

    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
    {
        var todoItem = _mapper.Map<TodoItem>(createTodoDto);
        await _repo.AddAsync(todoItem);
        var result = _mapper.Map<TodoDto>(todoItem);
        return result;
        // if (string.IsNullOrWhiteSpace(item.Title) && string.IsNullOrWhiteSpace(item.Desc))
        //     throw new ArgumentException("judul dan deskripsi tidak boleh kosong");

        // if (!item.DueDate.HasValue) item.DueDate = DateTime.UtcNow.AddDays(1);
        // return _repo.AddAsync(item);
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

    public async Task<TodoDto?> UpdateTodoAsync(Guid id, UpdateTodoDto updateTodoDto)
    {
        var todoItem = await _repo.GetByIdAsync(id);
        if (todoItem == null) return null;

        _mapper.Map(updateTodoDto, todoItem);
        await _repo.UpdateAsync(todoItem);

        var result = _mapper.Map<TodoDto>(todoItem);
        return result;

        // var existingItem = await _repo.GetByIdAsync(id);
        // if (existingItem == null)
        // {
        //     return false;
        // }

        // existingItem.Title = item.Title;
        // existingItem.Desc = item.Desc;
        // existingItem.IsCompleted = item.IsCompleted;

        // var result = await _repo.UpdateAsync(existingItem);
        // return result;
    }

    public async Task<bool> DeleteTodoAsync(Guid id)
    {
        var result = await _repo.DeleteAsync(id);
        return result;
    }
}
