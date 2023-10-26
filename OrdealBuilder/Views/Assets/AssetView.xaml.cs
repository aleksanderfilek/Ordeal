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
using OrdealBuilder.ViewModels;

namespace OrdealBuilder.Views
{
    /// <summary>
    /// Interaction logic for AssetView.xaml
    /// </summary>
    public partial class AssetView : UserControl
    {
        public AssetView()
        {
            InitializeComponent();
        }

        public void OpenFile(File file)
        {
            AssetViewModel model = (AssetViewModel)DataContext;
            model.OpenFile(file);
        }
    }
}
