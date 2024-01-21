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
    /// Interaction logic for ViewNavigator.xaml
    /// </summary>
    public partial class ViewNavigator : UserControl
    {
        public ViewNavigator()
        {
            InitializeComponent();
        }

        public void OpenFile(File file)
        {
            NavigatorViewModel model = (NavigatorViewModel)DataContext;
            model.OpenFile(file);
        }
    }
}
