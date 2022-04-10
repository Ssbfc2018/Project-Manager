using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public double Budget { get; set; }
        public int CompletionPercentage { get; set; }
        public Priority Priority { get; set; }

        public ICollection<Todo> Todos { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }

        public Project()
        {
            Todos=new List<Todo>();
            UserProjects=new HashSet<UserProject>();
        }
    }
}