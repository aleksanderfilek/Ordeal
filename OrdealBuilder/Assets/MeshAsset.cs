﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public class MeshAsset : Asset
    {
        public MeshAsset(string path, AssetType type) : base(path, type)
        {
            if (!System.IO.File.Exists(Path))
            {

            }
        }

        public override void Save()
        {

        }

        public override void Load()
        {

        }

        public override void Clear()
        {
            
        }
    }
}
