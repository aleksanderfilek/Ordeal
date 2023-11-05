using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder.ViewModels
{
    public class MeshViewModel : ViewModelBase
    {
        private File _file;
        private MeshAsset _asset;

        public int VerticesNum { get; set; }
        public int IndiciesNum { get; set; }

        public MeshViewModel(File file)
        {
            _file = file;
            _asset = (MeshAsset)_file.Asset;
            _asset.Load();

            VerticesNum = _asset.Buffers[0].Count/3;
            OnPropertyChanged(nameof(VerticesNum));
            IndiciesNum = _asset.Indicies.Count;
            OnPropertyChanged(nameof(IndiciesNum));

        }
    }
}
