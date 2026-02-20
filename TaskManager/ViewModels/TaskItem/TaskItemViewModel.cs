using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Commands;
using TaskManager.Core;

namespace TaskManager.ViewModels.TaskItem
{
    public class TaskItemViewModel : BaseViewModel
    {
        #region Fields
        private readonly Models.TaskItem _model;
        #endregion

        #region Properties
        public string Title
        {
            get => _model.Title;
            set
            {
                if (_model.Title != value)
                {
                    _model.Title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string Description
        {
            get => _model.Description;
            set
            {
                if (_model.Description != value)
                {
                    _model.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public bool IsCompleted
        {
            get => _model.IsCompleted;
            set
            {
                if (_model.IsCompleted != value)
                {
                    _model.IsCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }
        public Models.TaskItem Model => _model;
        #endregion

        #region Commands
        public RelayCommand ToggleCompletedCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        #endregion

        #region Events
        public event Action<TaskItemViewModel>? EditRequested;
        public event Action<TaskItemViewModel>? DeleteRequested;
        #endregion

        #region Constructors
        public TaskItemViewModel(Models.TaskItem model)
        {
            _model = model;

            ToggleCompletedCommand = new RelayCommand(ToggleCompleted);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }
        #endregion

        #region Methods
        public void RefreshFromModel()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(IsCompleted));
        }
        #endregion

        #region Private Methods
        private void ToggleCompleted()
        {
            _model.IsCompleted = !_model.IsCompleted;
            OnPropertyChanged(nameof(IsCompleted));
        }
        private void Edit()
        {
            EditRequested?.Invoke(this);
        }
        private void Delete()
        {
            DeleteRequested?.Invoke(this);
        }
        #endregion
    }
}
