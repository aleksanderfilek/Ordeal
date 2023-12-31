﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder.ViewModels
{
    public class AssetViewModel : ViewModelBase
    {
        public ViewModelBase? CurrentAssetViewModel { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }

        private File? _file = null;

        public AssetViewModel() 
        {
        }

        public void OpenFile(File file)
        {
            switch (file.Type)
            {
                case AssetType.Invalid:
                    CurrentAssetViewModel = null;
                    break;
                case AssetType.Texture:
                    CurrentAssetViewModel = new TextureViewModel(file);
                    break;
                case AssetType.Mesh:
                    CurrentAssetViewModel = new MeshViewModel(file);
                    break;
                case AssetType.Font:
                    break;
                case AssetType.Shader:
                    CurrentAssetViewModel = new ShaderViewModel(file);
                    break;
            }

            if (_file != null)
            {
                _file.Asset.Clear();
            }

            _file = file;
            Name = (CurrentAssetViewModel != null) ? file.Name : "";
            Type = (CurrentAssetViewModel != null) ? file.Type.ToString() : "";
            Path = (CurrentAssetViewModel != null) ? file.Path : "";

            OnPropertyChanged(nameof(CurrentAssetViewModel));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(Path));
        }
    }
}
