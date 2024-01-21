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
            if(Modified && Asset.Loaded)
            {
                Asset.Save();
            }
        }

        public void UpdateProjectFilesExtension(string OldExtension, string NewExtension)
        {
            if(Type != AssetType.Invalid)
            {
                string OldFilePath = System.IO.Path.ChangeExtension(Path, OldExtension);
                string NewFilePath = System.IO.Path.ChangeExtension(Path, NewExtension);
                if (Asset != null && Asset.Path != null)
                {
                    Asset.Path = NewFilePath;
                }

                if (System.IO.File.Exists(OldFilePath))
                {
                    System.IO.File.Move(OldFilePath, NewFilePath);
                    System.IO.File.Delete(OldFilePath);
                }
                else
                {
                    Save();
                }
            }
        }
    }
}
