namespace ToDoApi.Models
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string? TaskName { get; set; }
        public bool IsComplete { get; set; }
    }
}

