using Project_Manager.Data;
using Project_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskTank.Models
{
    public class TaskHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();

        public static List<Todo> ShowAllTask()
        {
            return db.Todos.ToList();
        }
        public static Todo AddTask(Project project, string title)
        {
            Todo task = new Todo
            {
                Title = title,
                Completion = 0,
                Completed = false,
                ProjectId = project.Id
            };

            return task;
        }

        public static void DeleteTask(int id)
        {
            Todo todo = db.Todos.FirstOrDefault(t => t.Id == id);
            
            Project project = db.Projects.Find(todo.ProjectId);

            if (todo.DeveloperId != null)
            {
                ApplicationUser developer = (ApplicationUser)db.Users.FirstOrDefault(u => u.Id == todo.DeveloperId);
                UserProject task = db.UserProjects.FirstOrDefault(up => up.ProjectId == project.Id);
                project.UserProjects.Remove(task);
                developer.UserProjects.Remove(task);
                db.UserProjects.Remove(task);
            }

            db.Todos.Remove(todo);
            project.Todos.Remove(todo);
            db.SaveChanges();
        }


        public static void UpdateTask(int id, string description)
        {
            Todo todo = db.Todos.FirstOrDefault(t => t.Id == id);
            todo.Content = description;
            db.SaveChanges();
        }

        public static void AssignTaskToDeveloper(string developerId, int todoId)
        {
            Todo todo = db.Todos.FirstOrDefault(t => t.Id == todoId);
            todo.DeveloperId = developerId;

            UserProject userProject = new UserProject()
            {
                UserId = developerId,
                ProjectId = todo.ProjectId.Value
            };
            ApplicationUser developer = (ApplicationUser)db.Users.FirstOrDefault(u => u.Id == developerId);
            developer.UserProjects.Add(userProject);
            Project project = db.Projects.FirstOrDefault(p => p.Id == todo.ProjectId.Value);
            project.UserProjects.Add(userProject);
            db.UserProjects.Add(userProject);

            db.SaveChanges();
        }

    }
}
