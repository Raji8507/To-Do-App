namespace TodoDemoApi.DTOs
{
    public record CreateToDoDto(string Title, string? Description, DateTime? DueDate, int? AssignedToUserId);
    public record UpdateToDoDto(string? Title, string? Description, DateTime? DueDate, bool? IsCompleted, int? AssignedToUserId);
    public record ToDoDto(int Id, string Title, string? Description, bool IsCompleted, DateTime? DueDate, int? AssignedToUserId, int CreatedByUserId);
}
