﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public class InvalidAsset : Asset
    {
        public InvalidAsset(string path, AssetType type) : base(path, type)
        {
        }
    }
}
