namespace TodoDemoApi.DTOs
{
    public record CreateToDoDto(string Title, string? Description, DateTime? DueDate, int? AssignedToUserId);
    public record UpdateToDoDto(string? Title, string? Description, DateTime? DueDate, bool? IsCompleted, int? AssignedToUserId);
    public record ToDoDTO(
            int Id,
            string Title,
            string? Description,
            bool IsCompleted,
            DateTime? DueDate,
            int? AssignedToUserId,
            int CreatedByUserId,
            string? AssignedToUsername,
            string? CreatedByUsername
        );
}
