using System.ComponentModel.DataAnnotations;
using todo.Models.Enums;

namespace todo.Dtos;

public class CreateTodoDto
{
    [Required]
    public string? Title { get; set; }
    public string? Desc { get; set; }
    public DateTime? DueDate { get; set; }
    public PriorityLevel Priority { get; set; }
}
