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
        public string? Path { get; set; }
        public AssetType Type { get; }
        public bool Loaded { get; private set; }

        public Asset(string path, AssetType type)
        {
            Loaded = false;
            OriginalPath = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Path = System.IO.Path.ChangeExtension(path, Project.GetProjectPreferences().ResourceExtension);
            Type = type;
        }

        public virtual void Load()
        {
            Loaded = true;
        }

        public virtual void Save()
        {

        }

        public virtual void Clear()
        {
            Loaded = false;
        }
    }
}
