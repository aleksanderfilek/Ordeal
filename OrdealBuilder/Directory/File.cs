using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public class File : DirectoryItem
    {
        public bool Modified { get => Asset.Modified; }

        public Asset Asset { get; private set; }

        public AssetType Type { get => Asset.Type; }

        public File(string path) : base(path)
        {
            Asset = AssetManager.GetAsset(path);
            Asset.Clear();
        }

        public void Save()
        {
            if(Modified)
            {
                Asset.Save();
            }
        }
    }
}
