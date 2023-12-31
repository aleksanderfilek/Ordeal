﻿using System;
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
    /// Interaction logic for ContentBrowser.xaml
    /// </summary>
    /// 

    public class FileSelectedArgs : EventArgs
    {
        private readonly File _file;

        public FileSelectedArgs(File file)
        {
            _file = file;
        }

        public File File
        {
            get { return _file; }
        }
    }

    public partial class ContentBrowser : UserControl
    {
        public event EventHandler<FileSelectedArgs> FileSelected;

        public ContentBrowser()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            FolderView.Items.Clear();

            Directory? directory = Project.Get().RootDirectory;
            if (directory == null)
            {
                return;
            }

            var item = new TreeViewItem()
            {
                Header = directory.Name,
                Tag = directory.Path
            };

            CreateDirectoryTree(directory, item);

            FolderView.Items.Add(item);
        }

        private void CreateDirectoryTree(Directory directory, TreeViewItem owner)
        {
            directory.Directories.ForEach(dir =>
            {
                var item = new TreeViewItem()
                {
                    Header = dir.Name,
                    Tag = dir.Path
                };

                CreateDirectoryTree(dir, item);

                owner.Items.Add(item);
            });

            directory.Files.ForEach(file =>
            {
                var item = new TreeViewItem()
                {
                    Header = file.Name,
                    Tag = file.Path
                };
                item.Selected += Item_Selected;
                owner.Items.Add(item);
            });
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            Directory? directory = Project.Get().RootDirectory;
            File file = directory.FindFile(item.Tag.ToString());
            if (file != null)
            {
                FileSelected?.Invoke(this, new FileSelectedArgs(file));
            }
        }
    }
}
