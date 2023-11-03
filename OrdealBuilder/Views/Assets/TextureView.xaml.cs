using OrdealBuilder.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
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

            UpdateDataContext();
        }

        private void UpdateDataContext()
        {
            if (_model == null)
                _model = (TextureViewModel)DataContext;
        }

        private void ColorSpaceChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataContext();

            _model.ColorSpaceChanged(((ComboBox)sender).SelectedIndex);
        }

        private void FilterMethodChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataContext();
            _model.FilterMethodChanged(((ComboBox)sender).SelectedIndex);
        }

        private void WrapMethodChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataContext();
            _model.WrapMethodChanged(((ComboBox)sender).SelectedIndex);
        }

        private void GenerateMipmapEnabled(object sender, RoutedEventArgs e)
        {
            UpdateDataContext();
            _model.GenerateMipmapChanged(true);
        }

        private void GenerateMipmapDisabled(object sender, RoutedEventArgs e)
        {
            UpdateDataContext();

            _model.GenerateMipmapChanged(false);
        }

        private void AtlasWidthChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataContext();
            uint size = 0;
            string text = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(text))
                size = UInt32.Parse(text);
            _model.AtlasWidthChanged(size);
        }

        private void AtlasHeitghtChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataContext();
            uint size = 0;
            string text = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(text))
                size = UInt32.Parse(text);
            _model.AtlasHeitghtChanged(size);
        }
    }
}
