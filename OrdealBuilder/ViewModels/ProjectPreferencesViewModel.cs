using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrdealBuilder.ViewModels
{
    public class ProjectPreferencesViewModel : ViewModelBase
    {
        public string ResourceExtension { get; set; }
        public string ApplicationName { get; set; }
        public string BuilderPath { get; set; }
        public string Compiler { get; set; }
        public string SourceFileExtension { get; set; }

        public ProjectPreferencesViewModel() 
        { 
            ProjectPreferences? projectPreferences = Project.GetProjectPreferences();
            if (projectPreferences != null)
            {
                ResourceExtension = projectPreferences.ResourceExtension;
                ApplicationName = projectPreferences.ApplicationName;
                BuilderPath = projectPreferences.BuilderPath;
                Compiler = projectPreferences.Compiler;
                SourceFileExtension = projectPreferences.SourceFileExtension;
            }
            else
            {
                ResourceExtension = "";
                ApplicationName = "";
                BuilderPath = "";
                Compiler = "";
                SourceFileExtension = "";
            }
            OnPropertyChanged(nameof(ResourceExtension));
            OnPropertyChanged(nameof(ApplicationName));
            OnPropertyChanged(nameof(BuilderPath));
            OnPropertyChanged(nameof(Compiler));
            OnPropertyChanged(nameof(SourceFileExtension));
        }
    }
}
