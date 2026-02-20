using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using TaskManager.Models;
using TaskManager.Services.Interfaces;

namespace TaskManager.Services.Implementations
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _filePath;

        public JsonTaskRepository()
        {
            _filePath = "tasks.json";
        }

        public IEnumerable<TaskItem> LoadTasks()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TaskItem>();
            }

            string json = File.ReadAllText(_filePath);

            List<TaskItem>? tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);

            if (tasks == null)
            {
                return new List<TaskItem>();
            }

            return tasks;
        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;

            string json = JsonSerializer.Serialize(tasks, options);

            File.WriteAllText(_filePath, json);
        }

        public void SaveTask(TaskItem task)
        {
            List<TaskItem> tasks = LoadTasks().ToList();

            TaskItem? existing = tasks.FirstOrDefault(t => t.Id == task.Id);

            if (existing != null)
            {
                existing.Title = task.Title;
                existing.Description = task.Description;
                existing.IsCompleted = task.IsCompleted;
            }
            else
            {
                tasks.Add(task);
            }

            SaveTasks(tasks);
        }

        public void DeleteTask(TaskItem task)
        {
            List<TaskItem> tasks = LoadTasks().ToList();

            TaskItem? existing = tasks.FirstOrDefault(t => t.Id == task.Id);

            if (existing != null)
            {
                tasks.Remove(existing);
                SaveTasks(tasks);
            }
        }
    }
}
