using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace OrdealBuilder
{
    public class MeshAsset : Asset
    {
        private List<List<float>> _buffers = new List<List<float>>(3);
        public List<List<float>> Buffers
        {
            get => _buffers;
            private set { }
        }

        private List<int> _indicies = new List<int>();
        public List<int> Indicies
        {
            get => _indicies;
            private set { }
        }

        public MeshAsset(string path, AssetType type) : base(path, type)
        {
            if (!System.IO.File.Exists(Path))
            {
                string content;
                using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
                {
                    content = streamReader.ReadToEnd();
                }

                List<float[]> positions = new List<float[]>();
                List<float[]> normals = new List<float[]>();
                List<float[]> texCoords = new List<float[]>();
                Dictionary<string, int[]> indiciesMap = new Dictionary<string, int[]>();
                List<string> indicies = new List<string>();

                StringReader reader = new StringReader(content);
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    switch(values[0])
                    {
                        case "v":
                            float[] positionsArr = new float[3];
                            positionsArr[0] = float.Parse(values[1], CultureInfo.InvariantCulture);
                            positionsArr[1] = float.Parse(values[2], CultureInfo.InvariantCulture);
                            positionsArr[2] = float.Parse(values[3], CultureInfo.InvariantCulture);
                            positions.Add(positionsArr);
                            break;
                        case "vn":
                            float[] normalsArr = new float[3];
                            normalsArr[0] = float.Parse(values[1], CultureInfo.InvariantCulture);
                            normalsArr[1] = float.Parse(values[2], CultureInfo.InvariantCulture);
                            normalsArr[2] = float.Parse(values[3], CultureInfo.InvariantCulture);
                            normals.Add(normalsArr);
                            break;
                        case "vt":
                            float[] coordsArr = new float[2];
                            coordsArr[0] = float.Parse(values[1], CultureInfo.InvariantCulture);
                            coordsArr[1] = float.Parse(values[2], CultureInfo.InvariantCulture);
                            texCoords.Add(coordsArr);
                            break;
                        case "f":
                            for (int i = 1; i < values.Length; i++)
                            {
                                int[] vertex = new int[3];
                                string[] inds = values[i].Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                vertex[0] = int.Parse(inds[0], CultureInfo.InvariantCulture) - 1;
                                vertex[1] = int.Parse(inds[1], CultureInfo.InvariantCulture) - 1;
                                vertex[2] = int.Parse(inds[2], CultureInfo.InvariantCulture) - 1;
                                if(!indiciesMap.ContainsKey(values[i]))
                                    indiciesMap.Add(values[i], vertex);
                                indicies.Add(values[i]);
                            }
                            break;
                    }
                }

                List<string> generated = new List<string>();
                int counter = -1;

                _buffers.Add(new List<float>());
                _buffers.Add(new List<float>());
                _buffers.Add(new List<float>());

                foreach (string index in indicies)
                {
                    counter++;
                    int foundIndex = generated.IndexOf(index);
                    if (foundIndex >= 0)
                    {
                        _indicies.Add(foundIndex);
                        continue;
                    }

                    int[] inds = indiciesMap[index];

                    _buffers[0].Add(positions[inds[0]][0]);
                    _buffers[0].Add(positions[inds[0]][1]);
                    _buffers[0].Add(positions[inds[0]][2]);

                    _buffers[1].Add(texCoords[inds[1]][0]);
                    _buffers[1].Add(texCoords[inds[1]][1]);

                    _buffers[2].Add(normals[inds[2]][0]);
                    _buffers[2].Add(normals[inds[2]][1]);
                    _buffers[2].Add(normals[inds[2]][2]);

                    generated.Add(index);
                    _indicies.Add(generated.Count - 1);
                }

                Save();
            }
        }

        public override void Save()
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(System.IO.File.Open(Path, FileMode.OpenOrCreate)))
            {
                binaryWriter.Write((uint)_buffers.Count);

                binaryWriter.Write((byte)3);
                binaryWriter.Write((uint)_buffers[0].Count);
                foreach(float value in _buffers[0])
                    binaryWriter.Write(value);

                binaryWriter.Write((byte)3);
                binaryWriter.Write((uint)_buffers[1].Count);
                foreach (float value in _buffers[1])
                    binaryWriter.Write(value);

                binaryWriter.Write((byte)2);
                binaryWriter.Write((uint)_buffers[2].Count);
                foreach (float value in _buffers[2])
                    binaryWriter.Write(value);

                binaryWriter.Write((uint)_indicies.Count);
                foreach (int value in _indicies)
                    binaryWriter.Write(value);
            }
        }

        public override void Load()
        {
            using (BinaryReader binaryReader = new BinaryReader(System.IO.File.Open(Path, FileMode.Open)))
            {
                uint buffersCount = binaryReader.ReadUInt32();
                _buffers = new List<List<float>>((int)buffersCount);

                for(int i = 0; i < buffersCount; i++)
                {
                    byte bufferType = binaryReader.ReadByte();
                    uint bufferSize = binaryReader.ReadUInt32();
                    List<float> buffer = new List<float>((int)bufferSize);
                    for (int j = 0; j < bufferSize; j++)
                        buffer.Add(binaryReader.ReadSingle());

                    _buffers.Add(buffer);
                }

                uint indiciesSize = binaryReader.ReadUInt32();
                _indicies = new List<int>((int)indiciesSize);
                for (int i = 0; i < indiciesSize; i++)
                    _indicies.Add(binaryReader.ReadInt32());
            }
        }

        public override void Clear()
        {
            _buffers = new List<List<float>>(3);
            _indicies = new List<int>();
        }
    }
}
