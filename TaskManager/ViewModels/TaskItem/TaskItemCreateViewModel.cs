using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Commands;
using TaskManager.Core;

namespace TaskManager.ViewModels.TaskItem
{
    public class TaskItemCreateViewModel : BaseViewModel
    {
        #region Fields
        private string _title = string.Empty;
        private string _description = string.Empty;
        #endregion

        #region Properties
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        #endregion

        #region Events
        public event Action? CloseRequested;
        public event Action<Models.TaskItem>? SaveRequested;
        #endregion

        #region Commands
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }
        #endregion

        #region Constructors
        public TaskItemCreateViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }
        #endregion

        #region Methods
        public Models.TaskItem ToModel()
        {
            Models.TaskItem model = new Models.TaskItem();
            model.Title = Title;
            model.Description = Description;
            model.IsCompleted = false;
            return model;
        }
        #endregion

        #region Private Methods
        private void Save()
        {
            Models.TaskItem model = ToModel();
            SaveRequested?.Invoke(model);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke();
        }
        #endregion
    }
}
