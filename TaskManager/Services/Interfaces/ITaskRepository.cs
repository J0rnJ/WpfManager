using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Models;

namespace TaskManager.Services.Interfaces
{
    public interface ITaskRepository
    {
        IEnumerable<TaskItem> LoadTasks();

        void SaveTasks(List<TaskItem> tasks);

        void SaveTask(TaskItem task);

        void DeleteTask(TaskItem task);
    }
}
