using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder.ViewModels
{
    public class NavigatorViewModel : ViewModelBase
    {
        private static NavigatorViewModel instance;
        public static NavigatorViewModel Get() { return instance; }

        public ViewModelBase? CurrentViewModel { get; set; }

        public NavigatorViewModel()
        {
            instance = this;
        }

        public void OpenFile(File file)
        {
            CurrentViewModel = new AssetViewModel();
            OnPropertyChanged(nameof(CurrentViewModel));
            ((AssetViewModel)CurrentViewModel).OpenFile(file);
        }

        public void OpenProjectPreferences()
        {
            CurrentViewModel = new ProjectPreferencesViewModel();
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public void OpenOutputLog()
        {
            CurrentViewModel = new OutputLogViewModel();
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
