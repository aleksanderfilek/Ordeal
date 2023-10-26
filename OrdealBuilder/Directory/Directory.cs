using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Documents;

namespace OrdealBuilder
{
    public class Directory : DirectoryItem
    {
        private List<Directory> directories = new List<Directory>();
        public List<Directory> Directories { get { return directories; } private set { } }

        private List<File> files = new List<File>();
        public List<File> Files { get { return files; } private set { } }

        public Directory(string path) : base(path)
        {
            Refresh();
        }

        public void Refresh()
        {
            directories = new List<Directory>();
            files = new List<File>();

            try
            {
                var dirPaths = System.IO.Directory.GetDirectories(Path);

                foreach (var dirPath in dirPaths)
                {
                    Directory dir = new Directory(dirPath);
                    directories.Add(dir);
                }
            }
            catch { }

            try
            {
                var filePaths = System.IO.Directory.GetFiles(Path);

                foreach (var filePath in filePaths)
                {
                    string extension = System.IO.Path.GetExtension(filePath);
                    if(extension.Equals(".oda") || extension.Equals(".odapro"))
                    {
                        continue;
                    }

                    File file = new File(filePath);
                    files.Add(file);
                }
            }
            catch { }
        }

        public void Save()
        {
            Directories.ForEach(d => d.Save());
            Files.ForEach(f => f.Save());
        }

        public bool IsSomethingToSave()
        {
            foreach(File file in files)
            {
                if(file.Modified)
                {
                    return true;
                }
            }
            foreach(Directory dir in directories)
            {
                if(dir.IsSomethingToSave())
                {
                    return true;
                }
            }
            return false;
        }

        public File? FindFile(string filePath)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            foreach (File file in files)
            {
                if (file.Name.Equals(fileName))
                {
                    return file;
                }
            }

            foreach (Directory dir in directories)
            {
                if(filePath.Contains(dir.Path))
                {
                    return dir.FindFile(filePath);
                }
            }

            return null;
        }
    }
}
