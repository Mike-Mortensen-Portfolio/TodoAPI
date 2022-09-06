namespace TodoAPI.Context
{
    public class Todo
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsComplete { get; set; }
        public Priority Priority { get; set; }
    }

    public enum Priority
    {
        Low,
        Normal,
        High
    }
}
