using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OrdealBuilder
{
    public class Asset
    {
        public string? Name { get; }
        public string? Path { get; }
        public AssetType Type { get; }

        public Asset(string path, AssetType type)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Path = System.IO.Path.ChangeExtension(path, ".oda");
            Type = type;

            StreamWriter stream = new StreamWriter(Path);
            stream.Close();
        }
    }
}
