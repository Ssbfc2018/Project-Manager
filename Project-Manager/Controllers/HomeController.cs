using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Models;
using System.Diagnostics;

namespace Project_Manager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public HomeController(ApplicationDbContext DB, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _db = DB;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            List<IdentityUserRole<string>> role = _db.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
            List<string> roleName=new List<string>();
            foreach(IdentityUserRole<string> r in role)
            {
                roleName.Add((await _db.Roles.FindAsync(r.RoleId)).Name);
            }
            ViewBag.roles = roleName;

            List<Project> projects=new List<Project>();
            if (roleName.Contains("Admin"))
            {
                projects = _db.Projects.OrderByDescending(p=>p.CompletionPercentage).ToList();
            }
            else
            {
                List<UserProject> userProjects = _db.UserProjects.Where(up => up.UserId == user.Id).ToList();
                foreach(UserProject up in userProjects)
                {
                    projects.Add(_db.Projects.FirstOrDefault(p => p.Id == up.ProjectId));
                }
            }

            return View(projects);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Dashboard()
        {
            return View(_db.Projects.Include(p=>p.Todos).ThenInclude(t=>t.DeveloperId)
                .OrderByDescending(p=>p.Priority));
        }

        public async Task<IActionResult> Details(int projectId)
        {
            var userTodoProjects = _db.Todos.Where(td => td.ProjectId == projectId);

            string userName = User.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            List<IdentityUserRole<string>> role = _db.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
            List<string> roleName = new List<string>();
            foreach (IdentityUserRole<string> r in role)
            {
                roleName.Add((await _db.Roles.FindAsync(r.RoleId)).Name);
            }
            ViewBag.roles = roleName;

            if (roleName.Contains("Developer"))
            {
                var todoList = _db.Todos.Where(td => td.ProjectId == projectId);
                userTodoProjects = todoList.Where(utp => utp.DeveloperId == user.Id);
            }
            
            Project? projectExits = _db.Projects.Include(p => p.Todos).FirstOrDefault(p => p.Id == projectId);
            if (projectExits != null)
            {
                if(projectExits.Todos.Where(t => t.Completed == true).Where(t => t.Hide == false) != null)
                {
                    ViewBag.completedTodo = projectExits.Todos.Where(t => t.Completed == true).Where(t => t.Hide == false).ToList();
                }
            }
            ViewBag.completedTodo = null;

            ViewBag.projectId = projectId;

            return View(userTodoProjects);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}