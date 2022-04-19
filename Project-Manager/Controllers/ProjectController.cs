using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Authorize(Roles ="Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
            _db.Todos.Add(newTodo);
            _db.SaveChanges();

            return RedirectToAction("Details", "Home", new { projectId = projectId });
        }

        [Authorize(Roles = "Admin, Manager")]

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> EditProject(int projectId)
        {
            ViewBag.editItem = _db.Projects.FirstOrDefault(t => t.Id == projectId);

            return View();
        }
        [HttpPost]
        public IActionResult EditProject(int id, string Name, DateTime Deadline, string priority, int CompletionPercentage)
        {
            Project editItem = _db.Projects.FirstOrDefault(t => t.Id == id);
            editItem.Name = Name;
            editItem.Deadline = Deadline;
            editItem.Priority = (Priority)Enum.Parse(typeof(Priority), priority, true);
            editItem.CompletionPercentage = CompletionPercentage;
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> EditTodo(int todoId)
        {
            ViewBag.editItem = _db.Todos.FirstOrDefault(t => t.Id == todoId);
            ViewBag.developers = new SelectList(_db.Users, "Id", "Email");

            return View();
        }
        [HttpPost]
        public IActionResult EditTodo(int id, string Title, string Content, DateTime Deadline,string priority,int Completion, bool Completed)
        {
            Todo editItem = _db.Todos.FirstOrDefault(t => t.Id == id);
            editItem.Title = Title;
            editItem.Content = Content;
            editItem.Deadline = Deadline;
            editItem.Priority = (Priority)Enum.Parse(typeof(Priority), priority, true);
            editItem.Completion = Completion;
            editItem.Completed = Completed;
            _db.SaveChanges();

            return RedirectToAction("Details", "Home", new { projectId = editItem.ProjectId });
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteTodo(int todoId)
        {
            Todo deleteItem = _db.Todos.FirstOrDefault(t => t.Id == todoId);

            Comment deleteComment = _db.Comments.FirstOrDefault(c => c.TodoId == todoId);
            while (deleteComment != null) 
            {
                _db.Comments.Remove(deleteComment);
                _db.SaveChanges();
                deleteComment = _db.Comments.FirstOrDefault(c => c.TodoId == todoId);
            }

            int projectId = deleteItem.ProjectId.Value;
            _db.Todos.Remove(deleteItem);
            _db.SaveChanges();

            return RedirectToAction("Details", "Home", new { projectId = projectId });
        }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult DeleteProject(int projectId)
        {
            Project deleteItem = _db.Projects.FirstOrDefault(p => p.Id == projectId);

            Todo deleteTodo = _db.Todos.FirstOrDefault(t => t.ProjectId == projectId);

            while (deleteTodo != null)
            {
                Comment deleteComment = _db.Comments.FirstOrDefault(c => c.TodoId == deleteTodo.Id);
                while (deleteComment != null)
                {
                    _db.Comments.Remove(deleteComment);
                    _db.SaveChanges();
                    deleteComment = _db.Comments.FirstOrDefault(c => c.TodoId == deleteTodo.Id);
                }

                _db.Todos.Remove(deleteTodo);
                _db.SaveChanges();

                deleteTodo = _db.Todos.FirstOrDefault(t => t.ProjectId == projectId);
            }

            _db.Projects.Remove(deleteItem);
            _db.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        public IActionResult ChangeCompletion(int todoId, int newCompletion)
        {
            Todo editItem = _db.Todos.FirstOrDefault(t => t.Id == todoId);
            editItem.Completion = newCompletion;
            _db.SaveChanges();

            return RedirectToAction("Details", "Home", new { projectId = editItem.ProjectId });
        }
        public IActionResult MarkAsComplete(int todoId)
        {
            Todo current = _db.Todos.FirstOrDefault(t => t.Id == todoId);
            current.Completed = true;
            current.Completion = 100;
            _db.SaveChanges();

            //Notification newNote=new Notification
            //{
            //    Content=$"{todoId} in {current.ProjectId} is completed. ",
            //    Condition=false
            //};


            return RedirectToAction("Details", "Home", new { projectId = current.ProjectId });
        }
        [Authorize(Roles = "Admin, Manager")]
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
