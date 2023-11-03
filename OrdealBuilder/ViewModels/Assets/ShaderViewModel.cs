using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdealBuilder.ViewModels
{
    public class ShaderViewModel : ViewModelBase
    {
        public string VertexShaderContent { get; set; }
        public string FragmentShaderContent { get; set; }


        private File _file;
        private ShaderAsset _asset;

        public ShaderViewModel(File file)
        {
            _file = file;
            _asset = (ShaderAsset)_file.Asset;
            _asset.Load();

            SetVertexShaderContent(_asset.VertexShaderContent);
            SetFragmentShaderContent(_asset.FragmentShaderContent);
        }

        private void SetVertexShaderContent(string content)
        {
            VertexShaderContent = content;
            OnPropertyChanged(nameof(VertexShaderContent));
        }

        private void SetFragmentShaderContent(string content)
        {
            FragmentShaderContent = content;
            OnPropertyChanged(nameof(FragmentShaderContent));
        }

        public void VertextShaderChanged(string content)
        {
            _asset.VertexShaderContent = content;
        }

        public void FragmentShaderChanged(string content)
        {
            _asset.FragmentShaderContent = content;
        }
    }
}
