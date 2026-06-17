using System;
using System.Collections.Generic;

namespace POEPART2
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLogger _logger;

        public TaskManager(ActivityLogger logger)
        {
            _storage = new TaskStorageHelper();
            _logger = logger;
        }

        // Add a new task
        public string AddTask(string title, string description, string reminder)
        {
            var task = new Task
            {
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };

            _storage.AddTask(task);
            _logger.Log($"Task added: '{title}' (Reminder: {reminder})");

            string message = $"Task added: '{title}' - {description}";
            if (!string.IsNullOrEmpty(reminder))
                message += $" (Reminder set for {reminder})";

            return message;
        }

        // Get all tasks
        public List<Task> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        // Mark task as complete
        public string MarkAsComplete(int id)
        {
            var task = _storage.GetTask(id);
            if (task != null)
            {
                _storage.MarkAsComplete(id);
                _logger.Log($"Task marked complete: '{task.Title}'");
                return $"Task '{task.Title}' marked as complete!";
            }
            return "Task not found.";
        }

        // Delete task
        public string DeleteTask(int id)
        {
            var task = _storage.GetTask(id);
            if (task != null)
            {
                _storage.DeleteTask(id);
                _logger.Log($"Task deleted: '{task.Title}'");
                return $"Task '{task.Title}' deleted.";
            }
            return "Task not found.";
        }

        // Get incomplete tasks
        public List<Task> GetIncompleteTasks()
        {
            return _storage.LoadTasks().Where(t => !t.IsComplete).ToList();
        }
    }
}