using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Models.Enums;

namespace todo.Dtos;

public class TodoDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Desc { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public PriorityLevel Priority { get; set; }
}
