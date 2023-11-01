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
    public partial class TextureView : UserControl
    {
        private TextureViewModel _model;

        public TextureView()
        {
            InitializeComponent();

            _model = (TextureViewModel)DataContext;
        }

        private void ColorSpaceChanged(object sender, SelectionChangedEventArgs e)
        {
            _model = (TextureViewModel)DataContext;
            _model.ColorSpaceChanged(((ComboBox)sender).SelectedIndex);
        }

        private void FilterMethodChanged(object sender, SelectionChangedEventArgs e)
        {
            _model.FilterMethodChanged(((ComboBox)sender).SelectedIndex);
        }

        private void WrapMethodChanged(object sender, SelectionChangedEventArgs e)
        {
            _model.WrapMethodChanged(((ComboBox)sender).SelectedIndex);
        }

        private void GenerateMipmapEnabled(object sender, RoutedEventArgs e)
        {
            _model.GenerateMipmapChanged(true);
        }

        private void GenerateMipmapDisabled(object sender, RoutedEventArgs e)
        {
            _model.GenerateMipmapChanged(false);
        }

        private void AtlasWidthChanged(object sender, TextChangedEventArgs e)
        {
            _model.AtlasWidthChanged(Int32.Parse(((TextBox)sender).Text));
        }

        private void AtlasHeitghtChanged(object sender, TextChangedEventArgs e)
        {
            _model.AtlasHeitghtChanged(Int32.Parse(((TextBox)sender).Text));
        }
    }
}
