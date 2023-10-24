using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                    File file = new File(filePath);
                    files.Add(file);
                }
            }
            catch { }
        }
    }
}
