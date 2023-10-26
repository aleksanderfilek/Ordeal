using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public class AssetManager
    {
        public static Asset GetAsset(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath);
            switch (extension)
            {
                case ".jpg":
                case ".png":
                    return new TextureAsset(filePath, AssetType.Texture);
                case ".obj":
                    return new MeshAsset(filePath, AssetType.Mesh);
                case ".ttf":
                    return new FontAsset(filePath, AssetType.Font);
            }

            return new InvalidAsset(filePath, AssetType.Invalid);
        }
    }
}
