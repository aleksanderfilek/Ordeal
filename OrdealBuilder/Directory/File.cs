using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public class File : DirectoryItem
    {
        public bool Modified { get; set; }

        private Asset asset;

        public AssetType Type { get => asset.Type; }

        public File(string path) : base(path)
        {
            Modified = false;

            asset = AssetManager.GetAsset(path);
        }

        public void Save()
        {
            if(Modified)
            {
                Modified = false;
            }
        }
    }
}
