using Microsoft.AspNetCore.Identity;

namespace Project_Manager.Models
{
    public class ApplicationUser:IdentityUser
    {
        public double Salary { get; set; }

        public ICollection<Todo> Todos { get; set; }
    }
}
