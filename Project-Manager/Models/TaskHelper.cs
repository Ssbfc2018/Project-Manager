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
        public static Todo AddTask(string content)
        {
            Todo task = new Todo
            {
                Content = content,
                Completion = 0,
                Completed = false

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
                developer.Todos.Remove(todo);
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

        public static void AssignTaskToDeveloper(string Id, int id)
        {
            Todo todo = db.Todos.FirstOrDefault(t => t.Id == id);
            todo.DeveloperId = Id;
            db.SaveChanges();
        }

    }
}
