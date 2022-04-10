using Microsoft.AspNetCore.Identity;

namespace Project_Manager.Models
{
    public class ApplicationUser:IdentityUser
    {
        public double Salary { get; set; }

        public ICollection<Todo> DeveloperTodos { get; set; }
        public ICollection<UserProject> UserProjects { get; set; }

        public ApplicationUser()
        {
            Salary = 0.00;
            DeveloperTodos = new HashSet<Todo>();
            UserProjects = new HashSet<UserProject>();
        }
    }
}
