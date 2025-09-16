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

    public async Task<IEnumerable<TodoDto>> GetAllTodosAsync(int? userId, TodoQueryParameters queryParameters)
    {
        var todoItems = await _repo.GetAllAsync(userId, queryParameters);
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

    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto, int userId)
    {
        var todoItem = _mapper.Map<TodoItem>(createTodoDto);
        todoItem.UserId = userId; // Set the user ID here

        await _repo.AddAsync(todoItem);
        
        var result = _mapper.Map<TodoDto>(todoItem);
        return result;
    }

    public async Task<TodoDto?> PatchTodoAsync(Guid id, UpdateTodoDto updateTodoDto)
    {
        var todoItem = await _repo.GetByIdAsync(id);
        if (todoItem == null) return null;

        _mapper.Map(updateTodoDto, todoItem);
        await _repo.UpdateAsync(todoItem);

        var result = _mapper.Map<TodoDto>(todoItem);
        return result;
    }

    public async Task<bool> DeleteTodoAsync(Guid id)
    {
        var result = await _repo.DeleteAsync(id);
        return result;
    }
}
