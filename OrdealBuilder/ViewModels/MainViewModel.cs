using OrdealBuilder.Commands;
using OrdealBuilder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrdealBuilder.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand NewProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand SaveAllCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ExportCommand { get; }

        private Project project;

        private MainWindow mainWindow;

        public MainViewModel()
        {
            mainWindow = (MainWindow)App.Current.MainWindow;

            project = new Project();

            // file menu commands
            NewProjectCommand = new NewProjectCommand();
            OpenProjectCommand = new OpenProjectCommand();
            SaveAllCommand = new SaveAllCommand();
            ExitCommand = new ExitCommand();

            // tools menu commands
            ExportCommand = new ExportCommand();

            mainWindow.contentBrowser.FileSelected += ContentBrowser_FileSelected;

            project.OnDirectoryChanged += Project_OnDirectoryChanged;
        }

        private void ContentBrowser_FileSelected(object? sender, FileSelectedArgs e)
        {
            mainWindow.assetView.OpenFile(e.File);
        }

        private void Project_OnDirectoryChanged(object? sender, DirectoryChangedArgs e)
        {
            mainWindow.RefreshContentBrowser();
        }
    }
}
