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
        public bool Modified { get; set; }
        public string? Name { get; }
        public string? OriginalPath { get; }
        public string? Path { get; }
        public AssetType Type { get; }

        public Asset(string path, AssetType type)
        {
            OriginalPath = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Path = System.IO.Path.ChangeExtension(path, ".oda");
            Type = type;
        }

        public virtual void Load()
        {

        }

        public virtual void Save()
        {

        }

        public virtual void Clear()
        {

        }
    }
}
