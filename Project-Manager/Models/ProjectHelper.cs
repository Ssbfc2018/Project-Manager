using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_Manager.Data;
using Project_Manager.Models;

namespace TaskTank.Models
{
    public class ProjectHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();

        public static Project AddProject(string name, double budget)
        {
            Project project = new Project
            {
                Name = name,
                Budget = budget
            };
            return project;
        }

        public static string DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);

            if (project.Todos.Count != 0)
            {
                List<Todo> tasks = db.Todos.Where(t => t.Project == project).ToList();
                foreach (var t in tasks)
                {
                    db.Todos.Remove(t);
                }
            }

            if (project.UserProjects.Count != 0)
            {
                db.UserProjects.RemoveRange(project.UserProjects);
            }

            db.Projects.Remove(project);
            db.SaveChanges();
            return "The project you have choosen was deleted and its tasks as well";
        }

        public static void UpdateProject(int id, string newName)
        {
            Project project = db.Projects.FirstOrDefault(p => p.Id == id);
            project.Name = newName;
            db.SaveChanges();
        }

        public static Project GetAProject(int id)
        {
            Project project = db.Projects.FirstOrDefault(p => p.Id == id);
            return project;
        }
    }
}
