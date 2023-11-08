using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
                uint pixelDataSize = binaryReader.ReadUInt32();
                _pixelData = binaryReader.ReadBytes((int)pixelDataSize);
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
                binaryWriter.Write((uint)_pixelData.Length);
                binaryWriter.Write(_pixelData, 0, _pixelData.Length);
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

        private readonly uint QOI_HEADER_SIZE = 4;
        private readonly byte[] qoiPadding = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 1 };
        private readonly byte QOI_OP_RUN = 0xC0;
        private readonly byte QOI_OP_INDEX = 0x00;
        private readonly byte QOI_OP_RGBA = 0xFF;
        private readonly byte QOI_OP_RGB = 0xFE;
        private readonly byte QOI_OP_DIFF = 0x40;
        private readonly byte QOI_OP_LUMA = 0x80;

        private struct qoi_rgba
        {
            public byte r, g, b, a;

            public qoi_rgba()
            {
                r = 0;
                g = 0;
                b = 0; 
                a = 0;
            }

            public uint V()
            {
                uint v = (uint)a;
                v |= (uint)r << 24;
                v |= (uint)g << 16;
                v |= (uint)b << 8;
                return v;
            }

            public int Hash()
            {
                return (r * 3) + (g * 5) + (b * 7) + (a * 11);
            }
        }

        private byte[] QOIEncode(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var length = bitmapData.Stride * bitmapData.Height;

            byte[] rawImage = new byte[length];

            Marshal.Copy(bitmapData.Scan0, rawImage, 0, length);
            bitmap.UnlockBits(bitmapData);

            for(int i = 0; i < length; i += _channel)
            {
                byte temp = rawImage[i + 0];
                rawImage[i + 0] = rawImage[i + 2];
                rawImage[i + 2] = temp;
            }

            return Qoi.Encode(rawImage, (int)_width, (int)_height, (int)_channel);

            //int i, maxSize, p, run;
            //int pxLen, pxEnd, pxPos;

            //maxSize = (int)(_width * _height * (_channel + 1) + QOI_HEADER_SIZE + qoiPadding.Length);
            //p = 0;
            //byte[] bytes = new byte[maxSize];

            //bytes[p++] = (byte)'q';
            //bytes[p++] = (byte)'o';
            //bytes[p++] = (byte)'i';
            //bytes[p++] = (byte)'f';

            //qoi_rgba[] index = new qoi_rgba[64];
            //qoi_rgba px, pxPrev;
            //run = 0;

            //pxPrev.r = 0;
            //pxPrev.g = 0;
            //pxPrev.b = 0;
            //pxPrev.a = 255;
            //px = pxPrev;

            //pxLen = (int)(_width * _height * _channel);
            //pxEnd = pxLen - _channel;

            //for(pxPos = 0; pxPos < pxLen; pxPos += _channel)
            //{
            //    px.r = rawImage[pxPos + 0];
            //    px.g = rawImage[pxPos + 1];
            //    px.b = rawImage[pxPos + 2];
            //    if (_channel == 4)
            //    {
            //        px.a = rawImage[pxPos + 3];
            //    }

            //    if(px.V() == pxPrev.V())
            //    {
            //        run++;
            //        if(run == 62 || pxPos == pxEnd)
            //        {
            //            bytes[p++] = (byte)(QOI_OP_RUN | (run - 1));
            //            run = 0;
            //        }
            //    }
            //    else
            //    {
            //        int indexPos;
            //        if(run > 0)
            //        {
            //            bytes[p++] = (byte)(QOI_OP_RUN | (run - 1));
            //            run = 0;
            //        }

            //        indexPos = px.Hash() % 64;

            //        if (index[indexPos].V() == px.V())
            //        {
            //            bytes[p++] = (byte)(QOI_OP_INDEX | indexPos);
            //        }
            //        else
            //        {
            //            index[indexPos] = px;

            //            if(px.a == pxPrev.a)
            //            {
            //                int vr = px.r - pxPrev.r;
            //                int vg = px.g - pxPrev.g;
            //                int vb = px.b - pxPrev.b;

            //                int vg_r = vr - vg;
            //                int vg_b = vb - vg;

            //                if(vr > -3 && vr < 2 && vg > -3 && vg < 2 && vb > -3 && vb < 2)
            //                {
            //                    bytes[p++] = (byte)(QOI_OP_DIFF | (vr + 2) << 4 | (vg + 2) << 2 | (vb + 2));
            //                }
            //                else if (vg_r > -9 && vg_r < 8 && vg > -33 && vg < 32 && vg_b > -9 && vg_b < 8)
            //                {
            //                    bytes[p++] = (byte)(QOI_OP_LUMA | (vg + 32));
            //                    bytes[p++] = (byte)((vg_r + 8) << 4 | (vg_b + 8));
            //                }
            //                else
            //                {
            //                    bytes[p++] = QOI_OP_RGB;
            //                    bytes[p++] = px.r;
            //                    bytes[p++] = px.g;
            //                    bytes[p++] = px.b;
            //                }
            //            }
            //            else
            //            {
            //                bytes[p++] = QOI_OP_RGBA;
            //                bytes[p++] = px.r;
            //                bytes[p++] = px.g;
            //                bytes[p++] = px.b;
            //                bytes[p++] = px.a;
            //            }
            //        }
            //    }
            //    pxPrev = px;
            //}

            //for(i = 0; i < 8; i++)
            //{
            //    bytes[p++] = qoiPadding[i];
            //}

            //byte[] compressedImage = new byte[p];
            //Array.Copy(bytes, compressedImage, p);
            //return compressedImage;
        }
    }
}
