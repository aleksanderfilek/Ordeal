using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public class ShaderAsset : Asset
    {
        private string _vertexShaderContent = "";
        public string VertexShaderContent
        {
            get => _vertexShaderContent;
            set
            {
                if (_vertexShaderContent.CompareTo(value) != 0)
                {
                    _vertexShaderContent = value;
                    GatherUniforms();
                    Modified = true;
                }
            }
        }

        private string _fragmentShaderContent = "";
        public string FragmentShaderContent
        {
            get => _fragmentShaderContent;
            set
            {
                if(_fragmentShaderContent.CompareTo(value) != 0)
                {
                    _fragmentShaderContent = value;
                    GatherUniforms();
                    Modified = true;
                }
            }
        }

        private List<string> _uniforms = new List<string>();
        public List<string> Uniforms
        {
            get => _uniforms;
            private set { }
        }

        public ShaderAsset(string path, AssetType type) : base(path, type)
        {
            if (!System.IO.File.Exists(Path))
            {
                string content;
                using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
                {
                    content = streamReader.ReadToEnd();
                    content = content.Replace("\r\n", "\n");

                    int vertexShaderIndex = content.IndexOf("#vertex", 0);
                    int fragmentShaderIndex = content.IndexOf("#fragment", 0);

                    _vertexShaderContent = content.Substring(vertexShaderIndex + 8, fragmentShaderIndex - vertexShaderIndex - 8);
                    _fragmentShaderContent = content.Substring(fragmentShaderIndex + 10, content.Length - fragmentShaderIndex - 10);

                    GatherUniforms();
                }
                Save();
            }
        }

        public override void Save()
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(System.IO.File.Open(Path, FileMode.OpenOrCreate)))
            {
                binaryWriter.Write((uint)_vertexShaderContent.Length);
                binaryWriter.Write(_vertexShaderContent.ToArray(),0, _vertexShaderContent.Length);
                binaryWriter.Write((uint)_fragmentShaderContent.Length);
                binaryWriter.Write(_fragmentShaderContent.ToArray(), 0, _fragmentShaderContent.Length);
                binaryWriter.Write((uint)_uniforms.Count);
                foreach (string uniform in _uniforms)
                {
                    binaryWriter.Write(Guid.Create(uniform));
                    binaryWriter.Write((uint)uniform.Length);
                    binaryWriter.Write(uniform.ToArray(), 0, uniform.Length);
                }
            }

            using(StreamWriter  writer = new StreamWriter(OriginalPath))
            {
                writer.WriteLine("#vertex");
                writer.WriteLine(_vertexShaderContent);
                writer.WriteLine("#frargment");
                writer.WriteLine(_fragmentShaderContent);
            }

            Modified = false;
        }

        public override void Load()
        {
            using (BinaryReader binaryReader = new BinaryReader(System.IO.File.Open(Path, FileMode.Open)))
            {
                uint buffer = binaryReader.ReadUInt32();
                _vertexShaderContent = new string(binaryReader.ReadChars((int)buffer));
                buffer = binaryReader.ReadUInt32();
                _fragmentShaderContent = new string(binaryReader.ReadChars((int)buffer));
                uint uniformsNumber = binaryReader.ReadUInt32();
                for(int i = 0; i < uniformsNumber; i++)
                {
                    buffer = binaryReader.ReadUInt32();
                    buffer = binaryReader.ReadUInt32();
                    _uniforms.Add(new string(binaryReader.ReadChars((int)buffer)));
                }
            }
        }

        public override void Clear()
        {
            _vertexShaderContent = "";
            _fragmentShaderContent = "";
            _uniforms = new List<string>();
        }

        private void GatherUniforms()
        {
            List<string> uniformsList = new List<string>();
            string uniformWord = "uniform";
            int uniformIndex = 0;
            while (true)
            {
                uniformIndex = _vertexShaderContent.IndexOf(uniformWord, uniformIndex);
                if (uniformIndex < 0)
                {
                    break;
                }

                int nameStartIndex = _vertexShaderContent.IndexOf(" ", uniformIndex + uniformWord.Length + 1);
                int nameEndIndex = _vertexShaderContent.IndexOf(";", nameStartIndex);
                string name = _vertexShaderContent.Substring(nameStartIndex + 1, nameEndIndex - nameStartIndex);
                uniformsList.Add(name);
            }

            uniformIndex = 0;
            while (true)
            {
                uniformIndex = _fragmentShaderContent.IndexOf(uniformWord, uniformIndex);
                if (uniformIndex < 0)
                {
                    break;
                }

                int nameStartIndex = _fragmentShaderContent.IndexOf(" ", uniformIndex + uniformWord.Length + 1);
                int nameEndIndex = _fragmentShaderContent.IndexOf(";", nameStartIndex);
                string name = _fragmentShaderContent.Substring(nameStartIndex + 1, nameEndIndex - nameStartIndex);
                uniformsList.Add(name);
            }
            _uniforms = uniformsList;
        }
    }
}
