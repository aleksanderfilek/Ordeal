using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public class DirectoryChangedArgs : EventArgs
    {
        private readonly Directory _directory;

        public DirectoryChangedArgs(Directory directory)
        {
            _directory = directory;
        }

        public Directory Directory
        {
            get { return _directory; }
        }
    }

    public class Project
    {
        private static Project? _instance;
        public static Project? Get() { return _instance; }

        private Directory? rootDirectory;
        public Directory? RootDirectory { get { return rootDirectory; } }
        public event EventHandler<DirectoryChangedArgs>? OnDirectoryChanged;

        public Project()
        {
            _instance = this;
        }

        public void NewProject(string projectPath)
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                rootDirectory = null;
                return;
            }
            OpenProject(Path.GetDirectoryName(projectPath));
        }

        public void OpenProject(string? projectPath)
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                rootDirectory = null;
                return;
            }

            rootDirectory = new Directory(projectPath);
            OnDirectoryChanged?.Invoke(this, new DirectoryChangedArgs(rootDirectory));
        }
    }
}
