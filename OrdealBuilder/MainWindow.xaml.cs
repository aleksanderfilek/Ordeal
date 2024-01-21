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

namespace OrdealBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void RefreshContentBrowser()
        {
            contentBrowser.Refresh();
        }

        private bool IsContentBrowserMinimized = false;

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            IsContentBrowserMinimized = !IsContentBrowserMinimized;
            var def = grid.ColumnDefinitions[0];
            int newWidth = (IsContentBrowserMinimized) ? 50 : 300;
            string text = (IsContentBrowserMinimized) ? ">" : "<";
            MinimizeTextBlock.Text = text;
            def.Width = new GridLength(newWidth, GridUnitType.Pixel);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshContentBrowser();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Project.Get().IsSomethingToSave())
            {
                MessageBoxResult dr = System.Windows.MessageBox.Show("There are some modified file. Do you want save them?", "Save changes", MessageBoxButton.YesNo);
                if (dr == MessageBoxResult.Yes)
                {
                    Project.Get().SaveAll();
                }
            }
        }
    }
}
