using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Net.WebRequestMethods;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using System.Windows.Markup;
using System.Buffers.Binary;
using System.Security.Cryptography.Pkcs;

namespace OrdealBuilder
{
    public class TextureAsset : Asset
    {
        #region Properties
        private uint _width = 0;
        public uint Width 
        { 
            get => _width; 
            set 
            { 
                if (_width != value)
                {
                    _width = value;
                    Modified = true;
                }
            } 
        }

        private uint _height = 0;
        public uint Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    Modified = true;
                }
            }
        }

        private byte _channel = 0;
        public byte Channel
        {
            get => _channel;
            set
            {
                if (_channel != value)
                {
                    _channel = value;
                    Modified = true;
                }
            }
        }

        private byte _colorSpace = 0;
        public byte ColorSpace
        {
            get => _colorSpace;
            set
            {
                if (_colorSpace != value)
                {
                    _colorSpace = value;
                    Modified = true;
                }
            }
        }

        private byte _filter = 0;
        public byte Filter
        {
            get => _filter;
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    Modified = true;
                }
            }
        }

        private byte _wrap = 0;
        public byte Wrap
        {
            get => _wrap;
            set
            {
                if (_wrap != value)
                {
                    _wrap = value;
                    Modified = true;
                }
            }
        }

        private byte _generateMipmap = 0;
        public byte GenerateMipmap
        {
            get => _generateMipmap;
            set
            {
                if (_generateMipmap != value)
                {
                    _generateMipmap = value;
                    Modified = true;
                }
            }
        }

        private uint _atlasWidth = 0;
        public uint AtlasWidth
        {
            get => _atlasWidth;
            set
            {
                if (_atlasWidth != value)
                {
                    _atlasWidth = value;
                    Modified = true;
                }
            }
        }

        private uint _atlasHeight = 0;
        public uint AtlasHeight
        {
            get => _atlasHeight;
            set
            {
                if (_atlasHeight != value)
                {
                    _atlasHeight = value;
                    Modified = true;
                }
            }
        }

        private byte[] _pixelData;
        #endregion

        public TextureAsset(string path, AssetType type) : base(path, type)
        {
            if(!System.IO.File.Exists(Path))
            {
                var bitmap = new Bitmap(path, true);
                _width = (uint)bitmap.Width;
                _height = (uint)bitmap.Height;
                _channel = 3;
                uint result = (uint)bitmap.Flags & 2;
                if (result != 0)
                {
                    _channel = 4;
                }
                _colorSpace = 0;
                _filter = 0;
                _wrap = 0;
                _generateMipmap = 0;
                _atlasWidth = _width;
                _atlasHeight = _height;
                _pixelData = QOIEncode(bitmap);

                bitmap.Dispose();
                Save();
            }
        }

        public override void Load()
        {
            using (BinaryReader binaryReader = new BinaryReader(System.IO.File.Open(Path, FileMode.Open)))
            {
                _width = binaryReader.ReadUInt32();
                _height = binaryReader.ReadUInt32();
                _channel = binaryReader.ReadByte();
                _colorSpace = binaryReader.ReadByte();
                _filter = binaryReader.ReadByte();
                _wrap = binaryReader.ReadByte();
                _generateMipmap = binaryReader.ReadByte();
                _atlasWidth = binaryReader.ReadUInt32();
                _atlasHeight = binaryReader.ReadUInt32();
                _pixelData = binaryReader.ReadBytes((int)(_width * _height * _channel));
            }
        }

        public override void Save()
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(System.IO.File.Open(Path, FileMode.OpenOrCreate)))
            {
                binaryWriter.Write(_width);
                binaryWriter.Write(_height);
                binaryWriter.Write(_channel);
                binaryWriter.Write(_colorSpace);
                binaryWriter.Write(_filter);
                binaryWriter.Write(_wrap);
                binaryWriter.Write(_generateMipmap);
                binaryWriter.Write(_atlasWidth);
                binaryWriter.Write(_atlasHeight);
                binaryWriter.Write(_pixelData);
            }
            Modified = false;
        }

        public override void Clear()
        {
            _width = 0;
            _height = 0;
            _channel = 0;
            _colorSpace = 0;
            _filter = 0;
            _wrap = 0;
            _generateMipmap = 0;
            _atlasWidth = 0;
            _atlasHeight = 0;
            _pixelData = new byte[0];
        }

        private struct qoi_rgba_t
        {
            public byte r = 0, g = 0, b = 0, a = 0;

            public qoi_rgba_t()
            {
               
            }

            public uint Num()
            {
                return ((uint)r) << 24 | ((uint)g) << 16 | ((uint)b) << 8 | ((uint)a);
            }
        }

        private int QOI_COLOR_HASH(qoi_rgba_t color)
        {
            return (color.r * 3 + color.g * 5 + color.b * 7 + color.a * 11);
        }

        private byte[] QOIEncode(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var length = bitmapData.Stride * bitmapData.Height;

            byte[] rawImage = new byte[length];

            Marshal.Copy(bitmapData.Scan0, rawImage, 0, length);
            bitmap.UnlockBits(bitmapData);

            const uint QOI_OP_INDEX = 0x00; /* 00xxxxxx */
            const uint QOI_OP_DIFF =  0x40; /* 01xxxxxx */
            const uint QOI_OP_LUMA =  0x80; /* 10xxxxxx */
            const uint QOI_OP_RUN  =  0xc0; /* 11xxxxxx */
            const uint QOI_OP_RGB  =  0xfe; /* 11111110 */
            const uint QOI_OP_RGBA  = 0xff; /* 11111111 */

            const uint QOI_MAGIC = ((uint)'q') << 24 | ((uint)'o') << 16 | ((uint)'i') << 8 | ((uint)'f');
            const uint QOI_HEADER_SIZE = 4;

            byte[] qoi_padding = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 1 };

            int i, max_size, p, run;
            int px_len, px_end, px_pos, channels;
            byte[] bytes;
            qoi_rgba_t[] index = new qoi_rgba_t[64];
            qoi_rgba_t px, px_prev;

            max_size = (int)(Width * Height * ((byte)Channel + 1) + QOI_HEADER_SIZE + sizeof(byte) * 8);

            p = 0;
            bytes = new byte[max_size];

            BinaryPrimitives.WriteUInt32LittleEndian(bytes, QOI_MAGIC);

            run = 0;
            px_prev.r = 0;
            px_prev.g = 0;
            px_prev.b = 0;
            px_prev.a = 255;
            px = px_prev;

            px_len = (int)(Width * Height * (byte)Channel);
            px_end = px_len - (byte)Channel;
            channels = (byte)Channel;

            for (px_pos = 0; px_pos < px_len; px_pos += channels)
            {
                px.r = rawImage[px_pos + 0];
                px.g = rawImage[px_pos + 1];
                px.b = rawImage[px_pos + 2];

                if (channels == 4)
                {
                    px.a = rawImage[px_pos + 3];
                }

                if (px.Num() == px_prev.Num())
                {
                    run++;
                    if (run == 62 || px_pos == px_end)
                    {
                        bytes[p++] = (byte)(QOI_OP_RUN | (run - 1));
                        run = 0;
                    }
                }
                else
                {
                    int index_pos;

                    if (run > 0)
                    {
                        bytes[p++] = (byte)(QOI_OP_RUN | (run - 1));
                        run = 0;
                    }

                    index_pos = QOI_COLOR_HASH(px) % 64;

                    if (index[index_pos].Num() == px.Num())
                    {
                        bytes[p++] = (byte)(QOI_OP_INDEX | index_pos);
                    }
                    else
                    {
                        index[index_pos] = px;

                        if (px.a == px_prev.a)
                        {
                            sbyte vr = (sbyte)(px.r - px_prev.r);
                            sbyte vg = (sbyte)(px.g - px_prev.g);
                            sbyte vb = (sbyte)(px.b - px_prev.b);

                            sbyte vg_r = (sbyte)(vr - vg);
                            sbyte vg_b = (sbyte)(vb - vg);

                            if (
                                vr > -3 && vr < 2 &&
                                vg > -3 && vg < 2 &&
                                vb > -3 && vb < 2
                                )
                            {
                                bytes[p++] = (byte)(QOI_OP_DIFF | (vr + 2) << 4 | (vg + 2) << 2 | (vb + 2));
                            }
                            else if (
                                vg_r > -9 && vg_r < 8 &&
                                vg > -33 && vg < 32 &&
                                vg_b > -9 && vg_b < 8
                                )
                            {
                                bytes[p++] = (byte)(QOI_OP_LUMA | (vg + 32));
                                bytes[p++] = (byte)((vg_r + 8) << 4 | (vg_b + 8));
                            }
                            else
                            {
                                bytes[p++] = (byte)QOI_OP_RGB;
                                bytes[p++] = px.r;
                                bytes[p++] = px.g;
                                bytes[p++] = px.b;
                            }
                        }
                        else
                        {
                            bytes[p++] = (byte)QOI_OP_RGBA;
                            bytes[p++] = px.r;
                            bytes[p++] = px.g;
                            bytes[p++] = px.b;
                            bytes[p++] = px.a;
                        }
                    }
                }
                px_prev = px;
            }

            for (i = 0; i < 8; i++)
            {
                bytes[p++] = qoi_padding[i];
            }

            byte[] compressedImage = new byte[p];
            Array.Copy(bytes, compressedImage, p);
            return compressedImage;
        }
    }
}
