using System.Collections.Generic;
using System.Linq;

namespace POEPART2
{
    public class TaskStorageHelper
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // Load all tasks from database
        public List<Task> LoadTasks()
        {
            return db.Tasks.ToList();
        }

        // Add a new task to database
        public void AddTask(Task task)
        {
            db.Tasks.Add(task);
            db.SaveChanges();
        }

        // Mark a task as complete
        public void MarkAsComplete(int id)
        {
            var task = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task != null)
            {
                task.IsComplete = true;
                db.Tasks.Update(task);
                db.SaveChanges();
            }
        }

        // Delete a task from database
        public void DeleteTask(int id)
        {
            var task = db.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task != null)
            {
                db.Tasks.Remove(task);
                db.SaveChanges();
            }
        }

        // Get a single task by ID
        public Task GetTask(int id)
        {
            return db.Tasks.Where(t => t.Id == id).FirstOrDefault();
        }
    }
}