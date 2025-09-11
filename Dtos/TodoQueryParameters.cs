using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Models.Enums;

namespace todo.Dtos;

public class TodoQueryParameters
{
    public bool? IsCompleted { get; set; }
    public PriorityLevel? Priority { get; set; }

    // sorting
    public string? SortBy { get; set; }     // dueDate or priority
    public string? SortOrder { get; set; } = "asc";         // asc or desc
}