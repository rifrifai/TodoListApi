using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using todo.Data;
using todo.Models;

namespace todo.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoContext _context;
    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        var result = await _context.TodoItems.ToListAsync();
        return result;
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id)
    {
        var result = await _context.TodoItems.FindAsync(id);
        return result;
    }

    public async Task AddAsync(TodoItem item)
    {
        if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
        await _context.TodoItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(TodoItem item)
    {
        var existingItem = await _context.TodoItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == item.Id);
        if (existingItem == null) return false;

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null)
        {
            return false;
        }
        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}