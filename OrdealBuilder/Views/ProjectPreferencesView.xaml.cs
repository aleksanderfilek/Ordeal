using OrdealBuilder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrdealBuilder.Views
{
    /// <summary>
    /// Interaction logic for ProjectPreferencesView.xaml
    /// </summary>
    public partial class ProjectPreferencesView : UserControl
    {
        public ProjectPreferencesView()
        {
            InitializeComponent();
        }

        private void OnSelectBuilderPath(object sender, RoutedEventArgs e)
        {
            ProjectPreferences? projectPreferences = Project.GetProjectPreferences();
            if (projectPreferences != null)
            {
                var fileDialog = new System.Windows.Forms.OpenFileDialog();
                fileDialog.Filter = "Builder (*.exe)|*.exe";
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BuilderPath.Content = fileDialog.FileName;
                    projectPreferences.BuilderPath = fileDialog.FileName;
                    Project.Get().SaveProjectPreferences();
                }
            }
        }

        private void ResourceExtension_LostFocus(object sender, RoutedEventArgs e)
        {
            ProjectPreferences? projectPreferences = Project.GetProjectPreferences();
            if(projectPreferences != null)
            {
                string NewExtension = ((System.Windows.Controls.TextBox)sender).Text;
                Project.Get().UpdateProjectFilesExtension(NewExtension);
                Project.Get().SaveProjectPreferences();
            }
        }

        private void ApplicationName_LostFocus(object sender, RoutedEventArgs e)
        {
            ProjectPreferences? projectPreferences = Project.GetProjectPreferences();
            if (projectPreferences != null)
            {
                projectPreferences.ApplicationName = ((System.Windows.Controls.TextBox)sender).Text;
                Project.Get().SaveProjectPreferences();
            }
        }
        private void Compiler_LostFocus(object sender, RoutedEventArgs e)
        {
            ProjectPreferences? projectPreferences = Project.GetProjectPreferences();
            if (projectPreferences != null)
            {
                projectPreferences.Compiler = ((System.Windows.Controls.TextBox)sender).Text;
                Project.Get().SaveProjectPreferences();
            }
        }
        private void SourceFileExtension_LostFocus(object sender, RoutedEventArgs e)
        {
            ProjectPreferences? projectPreferences = Project.GetProjectPreferences();
            if (projectPreferences != null)
            {
                projectPreferences.SourceFileExtension = ((System.Windows.Controls.TextBox)sender).Text;
                Project.Get().SaveProjectPreferences();
            }
        }
    }
}
