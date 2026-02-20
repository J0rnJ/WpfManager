using System.Configuration;
using System.Data;
using System.Windows;
using TaskManager.Services.Implementations;
using TaskManager.Services.Interfaces;
using TaskManager.ViewModels;
using TaskManager.Views;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ITaskRepository repository = new JsonTaskRepository();
            MainViewModel mainViewModel = new MainViewModel(repository);

            MainWindow mainWindow =  new MainWindow();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }
    }

}
