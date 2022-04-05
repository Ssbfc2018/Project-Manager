namespace Project_Manager.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public double Budget { get; set; }
        public int CompletionPercentage { get; set; }

        public ICollection<Todo> Todos { get; set; }
        public string ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }
        public string Name { get; internal set; }
    }
}
