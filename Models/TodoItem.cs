using todo.Models.Enums;

namespace todo.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Desc { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? DueDate { get; set; }
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

    public int UserId { get; set; }     // foreign key
    public User User { get; set; } = null!;      // navigation property
}