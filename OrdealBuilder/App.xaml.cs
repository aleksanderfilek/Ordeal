using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using OrdealBuilder.ViewModels;

namespace OrdealBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string? startupProject = null;
            if (e.Args.Length == 1)
            {
                FileInfo file = new FileInfo(e.Args[0]);
                if (file.Exists) //make sure it's actually a file
                {
                    startupProject = file.FullName;
                }
            }
            
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(startupProject)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
