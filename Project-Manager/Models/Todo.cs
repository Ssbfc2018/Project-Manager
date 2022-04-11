namespace Project_Manager.Models
{
    public enum Priority
    {
        Urgent=0,
        Important=1,
        Normal=2
    }

    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Deadline { get; set; }
        public bool Completed { get; set; }
        public int Completion { get; set; }
        public Priority Priority { get; set; }
        public bool Hide { get; set; }

        public int? ProjectId { get; set; }
        public Project? Project { get; set; }
        public string? DeveloperId { get; set; }
        public ApplicationUser? Developer { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public Todo()
        {
            Comments = new List<Comment>();
        }
    }
}
