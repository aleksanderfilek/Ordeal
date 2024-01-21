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
using OrdealBuilder;
using OrdealBuilder.ViewModels;

namespace OrdealBuilder.Views
{
    /// <summary>
    /// Interaction logic for OutputLogView.xaml
    /// </summary>
    public partial class OutputLogView : UserControl
    {
        public OutputLogView()
        {
            InitializeComponent();

        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            var CurrentViewModel = new OutputLogViewModel();
            CurrentViewModel.Clear();
        }

    }
}
