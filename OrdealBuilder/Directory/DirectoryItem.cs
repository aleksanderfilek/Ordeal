using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public abstract class DirectoryItem
    {
        public string Path { get; set; }
        public string Name
        {
            get => System.IO.Path.GetFileName(Path);
            private set { }
        }

        public DirectoryItem(string path)
        {
            Path = path;
        }
    }
}
