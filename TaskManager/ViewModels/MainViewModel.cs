using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TaskManager.Commands;
using TaskManager.Core;
using TaskManager.Services.Interfaces;
using TaskManager.ViewModels.TaskItem;

namespace TaskManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields
        private readonly ITaskRepository _taskRepository;

        private TaskItemViewModel? _selectedTask;
        private bool _isPopupOpen;
        private BaseViewModel? _popupViewModel;
        #endregion

        #region Properties
        public ObservableCollection<TaskItemViewModel> Tasks { get; }

        public TaskItemViewModel? SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (SetProperty(ref _selectedTask, value))
                {
                    RemoveTaskCommand.RaiseCanExecuteChanged();
                    ToggleCompleteCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set => SetProperty(ref _isPopupOpen, value);
        }
        public BaseViewModel? PopupViewModel
        {
            get => _popupViewModel;
            set => SetProperty(ref _popupViewModel, value);
        }
        #endregion

        #region Commands
        public RelayCommand AddTaskCommand { get; }
        public RelayCommand EditTaskCommand { get; }
        public RelayCommand RemoveTaskCommand { get; }
        public RelayCommand ToggleCompleteCommand { get; }
        #endregion

        #region Constructors
        public MainViewModel(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;

            // Load models from repository
            IEnumerable<Models.TaskItem> models = _taskRepository.LoadTasks();

            // Convert models → viewModels
            Tasks = new ObservableCollection<TaskItemViewModel>(models.Select(model => new TaskItemViewModel(model)));

            foreach (TaskItemViewModel task in Tasks)
            {
                HookItemEvents(task);
            }

            _isPopupOpen = false;

            AddTaskCommand = new RelayCommand(AddTask);
            EditTaskCommand = new RelayCommand(EditTask, () => SelectedTask != null);
            RemoveTaskCommand = new RelayCommand(RemoveTask, () => SelectedTask != null);
            ToggleCompleteCommand = new RelayCommand(ToggleCompleted, () => SelectedTask != null);
        }
        #endregion

        #region Private Methods
        private void HookItemEvents(TaskItemViewModel item) 
        { 
            item.EditRequested += OnEditRequested; 
            item.DeleteRequested += OnDeleteRequested; 
        }

        private void OnEditRequested(TaskItemViewModel task)
        {
            SelectedTask = task;
            EditTask();
        }

        private void OnDeleteRequested(TaskItemViewModel task)
        {
            SelectedTask = task;
            RemoveTask();
        }

        private void OpenPopup(BaseViewModel viewModel)
        {
            PopupViewModel = viewModel;
            IsPopupOpen = true;
        }

        private void ClosePopup()
        {
            PopupViewModel = null;
            IsPopupOpen = false;
        }

        private void AddTask()
        {
            TaskItemCreateViewModel createViewModel = new TaskItemCreateViewModel();

            createViewModel.CloseRequested += () => ClosePopup();
            createViewModel.SaveRequested += model =>
            {
                _taskRepository.SaveTask(model);
                TaskItemViewModel viewModel = new TaskItemViewModel(model);
                HookItemEvents(viewModel);
                Tasks.Add(viewModel);
                ClosePopup();
            };

            OpenPopup(createViewModel);
        }

        private void EditTask()
        {
            if (SelectedTask == null) return;

            TaskItemEditViewModel editViewModel = new TaskItemEditViewModel(SelectedTask.Model);

            editViewModel.CloseRequested += () => ClosePopup();
            editViewModel.SaveRequested += () =>
            {
                editViewModel.ApplyChanges();
                SelectedTask.RefreshFromModel();
                _taskRepository.SaveTask(SelectedTask.Model);
                ClosePopup();
            };

            OpenPopup(editViewModel);
        }

        private void RemoveTask()
        {
            if (SelectedTask == null) return;

            _taskRepository.DeleteTask(SelectedTask.Model);
            Tasks.Remove(SelectedTask);
            SelectedTask = null;
        }

        private void ToggleCompleted()
        {
            if (SelectedTask == null) return;

            Models.TaskItem model = SelectedTask.Model;
            model.IsCompleted = !model.IsCompleted;

            _taskRepository.SaveTask(model);

            OnPropertyChanged(nameof(Tasks));
        }
        #endregion
    }
}
