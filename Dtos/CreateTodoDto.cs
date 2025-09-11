using System.ComponentModel.DataAnnotations;

namespace todo.Dtos;

public class CreateTodoDto
{
    [Required]
    public string? Title { get; set; }
    public string? Desc { get; set; }
    public DateTime? DueDate { get; set; }
}
