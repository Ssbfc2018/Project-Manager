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
    public class ProjectController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public ProjectController(ApplicationDbContext DB, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _db = DB;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult CreateProject()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProject(Project newProject)
        {
            _db.Projects.Add(newProject);
            _db.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        public IActionResult CreateTodo(int projectId)
        {
            ViewBag.projectId = projectId;
            return View();
        }
        [HttpPost]
        public IActionResult CreateTodo(string Title,string Content,DateTime Deadline,string priority, int projectId)
        {
            Todo newTodo = new Todo
            {
                Title = Title,
                Content = Content,
                Deadline = Deadline,
                Priority=(Priority)Enum.Parse(typeof(Priority), priority, true),
                ProjectId = projectId
            };
            //newTodo.ProjectId = projectId;
            _db.Todos.Add(newTodo);
            _db.SaveChanges();

            return RedirectToAction("Details", "Home", new { projectId = projectId });
        }

        public IActionResult MarkAsComplete(int todoId)
        {
            Todo current = _db.Todos.FirstOrDefault(t => t.Id == todoId);
            current.Completed = true;
            current.Completion = 100;
            _db.SaveChanges();

            return RedirectToAction("Details", "Home", new { projectId = current.ProjectId });
        }
        public IActionResult Hide(int todoId)
        {
            Todo current = _db.Todos.FirstOrDefault(t => t.Id == todoId);
            current.Hide = true;
            _db.SaveChanges();

            return RedirectToAction("Details", "Home", new { projectId = current.ProjectId });
        }

        public IActionResult Comment(int todoId)
        {
            ViewBag.content = _db.Todos.FirstOrDefault(t => t.Id == todoId).Content;
            ViewBag.todoId = todoId;

            return View();
        }
    }
}
