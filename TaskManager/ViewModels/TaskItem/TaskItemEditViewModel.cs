using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TaskManager.Commands;
using TaskManager.Core;

namespace TaskManager.ViewModels.TaskItem
{
    public class TaskItemEditViewModel : BaseViewModel
    {
        #region Fields
        private readonly Models.TaskItem _original;

        private string _title;
        private string _description;
        private bool _isCompleted;
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

        public bool IsCompleted
        {
            get => _isCompleted;
            set => SetProperty(ref _isCompleted, value);
        }
        #endregion

        #region Events
        public event Action? CloseRequested;
        public event Action? SaveRequested;
        #endregion

        #region Commands
        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }
        #endregion

        #region Constructors
        public TaskItemEditViewModel(Models.TaskItem model)
        {
            _original = model;

            _title = model.Title;
            _description = model.Description;
            _isCompleted = model.IsCompleted;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }
        #endregion

        #region Methods
        public void ApplyChanges()
        {
            _original.Title = Title;
            _original.Description = Description;
            _original.IsCompleted = IsCompleted;
        }
        #endregion

        #region Private Methods
        private void Save()
        {
            SaveRequested?.Invoke();
        }

        private void Cancel()
        {
            CloseRequested?.Invoke();
        }
        #endregion
    }
}
