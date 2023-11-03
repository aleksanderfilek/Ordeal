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
    /// Interaction logic for ShaderView.xaml
    /// </summary>
    public partial class ShaderView : UserControl
    {
        private ShaderViewModel _model;

        public ShaderView()
        {
            InitializeComponent();

            UpdateDataContext();
        }

        private void UpdateDataContext()
        {
            if (_model == null)
                _model = (ShaderViewModel)DataContext;
        }

        private void VertextShaderChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataContext();
            _model.VertextShaderChanged(((TextBox)sender).Text);
        }

        private void FragmentShaderChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataContext();
            _model.FragmentShaderChanged(((TextBox)sender).Text);
        }
    }
}
