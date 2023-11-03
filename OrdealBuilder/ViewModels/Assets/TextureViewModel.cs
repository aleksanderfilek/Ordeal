using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder.ViewModels
{
    public class TextureViewModel : ViewModelBase
    {
        private File _file;
        private TextureAsset _asset;

        public string Size { get; set; }
        public int ColorSpaceIndex { get; set; }
        public int FilterIndex { get; set; }
        public int WrapIndex { get; set; }
        public bool GenerateMipmap { get; set; }
        public string AtlasWidth { get; set; }
        public string AtlasHeight { get; set; }

        public TextureViewModel(File file) 
        {
            _file = file;
            _asset = (TextureAsset)_file.Asset;
            _asset.Load();

            SetSize(_asset.Width, _asset.Height);
            SetColorSpace(_asset.ColorSpace);
            SetFilterMethod(_asset.Filter);
            SetWrapMethod(_asset.Wrap);
            SeGenerateMipmap(_asset.GenerateMipmap > 0);
            SetAtlasWidth(_asset.AtlasWidth);
            SetAtlasHeight(_asset.AtlasHeight);
        }

        #region Setters
        private void SetSize(uint width, uint height)
        {
            Size = width.ToString() + " x " + height.ToString();
            OnPropertyChanged(nameof(Size));
        }

        private void SetColorSpace(int index)
        {
            ColorSpaceIndex = index;
            OnPropertyChanged(nameof(ColorSpaceIndex));
        }

        private void SetFilterMethod(int index)
        {
            FilterIndex = index;
            OnPropertyChanged(nameof(FilterIndex));
        }

        private void SetWrapMethod(int index)
        {
            WrapIndex = index;
            OnPropertyChanged(nameof(WrapIndex));
        }

        private void SeGenerateMipmap(bool enable)
        {
            GenerateMipmap = enable;
            OnPropertyChanged(nameof(GenerateMipmap));
        }

        private void SetAtlasWidth(uint width)
        {
            AtlasWidth = width.ToString();
            OnPropertyChanged(nameof(AtlasWidth));
        }

        private void SetAtlasHeight(uint height)
        {
            AtlasHeight = height.ToString();
            OnPropertyChanged(nameof(AtlasHeight));
        }
        #endregion
        #region Getters
        public void ColorSpaceChanged(int index)
        {
            _asset.ColorSpace = (byte)index;
        }

        public void FilterMethodChanged(int index)
        {
            _asset.Filter = (byte)index;
        }
        public void WrapMethodChanged(int index)
        {
            _asset.Wrap = (byte)index;
        }
        public void GenerateMipmapChanged(bool enabled)
        {
            _asset.GenerateMipmap = (byte)((enabled)?1:0);
        }

        public void AtlasWidthChanged(uint width)
        {
            _asset.AtlasWidth = width;
        }

        public void AtlasHeitghtChanged(uint height)
        {
            _asset.AtlasHeight = height;
        }
        #endregion
    }
}
