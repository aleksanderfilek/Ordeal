using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder
{
    public class Qoi
    {
        public const byte Index = 0x00;
        public const byte Diff = 0x40;
        public const byte Luma = 0x80;
        public const byte Run = 0xC0;
        public const byte Rgb = 0xFE;
        public const byte Rgba = 0xFF;
        public const byte Mask2 = 0xC0;

        public static int MaxPixels = 400_000_000;

        public const int HashTableSize = 64;

        public const byte HeaderSize = 14;
        public const string MagicString = "qoif";

        public static readonly int Magic = CalculateMagic(MagicString.AsSpan());
        public static readonly byte[] Padding = { 0, 0, 0, 0, 0, 0, 0, 1 };

        public static int CalculateHashTableIndex(int r, int g, int b, int a) =>
            ((r & 0xFF) * 3 + (g & 0xFF) * 5 + (b & 0xFF) * 7 + (a & 0xFF) * 11) % HashTableSize * 4;

        public static bool IsValidMagic(byte[] magic) => CalculateMagic(magic) == Magic;

        private static int CalculateMagic(ReadOnlySpan<char> chars) => chars[0] << 24 | chars[1] << 16 | chars[2] << 8 | chars[3];
        private static int CalculateMagic(ReadOnlySpan<byte> data) => data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3];

        public static byte[] Encode(byte[] data, int width, int height, int channels)
        {
            if (width == 0)
            {
                return new byte[1];
            }

            if (height == 0 || height >= MaxPixels / width)
            {
                return new byte[1];
            }

            byte[] bytes = new byte[HeaderSize + Padding.Length + (width * height * channels)];

            int p = 0;

            bytes[p++] = (byte)(Magic >> 24);
            bytes[p++] = (byte)(Magic >> 16);
            bytes[p++] = (byte)(Magic >> 8);
            bytes[p++] = (byte)Magic;

            byte[] index = new byte[HashTableSize * 4];

            byte prevR = 0;
            byte prevG = 0;
            byte prevB = 0;
            byte prevA = 255;

            byte r = 0;
            byte g = 0;
            byte b = 0;
            byte a = 255;

            int run = 0;
            bool hasAlpha = channels == 4;

            int pixelsLength = width * height * channels;
            int pixelsEnd = pixelsLength - channels;
            int counter = 0;

            for (int pxPos = 0; pxPos < pixelsLength; pxPos += channels)
            {
                r = data[pxPos];
                g = data[pxPos + 1];
                b = data[pxPos + 2];
                if (hasAlpha)
                {
                    a = data[pxPos + 3];
                }

                if (RgbaEquals(prevR, prevG, prevB, prevA, r, g, b, a))
                {
                    run++;
                    if (run == 62 || pxPos == pixelsEnd)
                    {
                        bytes[p++] = (byte)(Run | (run - 1));
                        run = 0;
                    }
                }
                else
                {
                    if (run > 0)
                    {
                        bytes[p++] = (byte)(Run | (run - 1));
                        run = 0;
                    }

                    int indexPos = CalculateHashTableIndex(r, g, b, a);

                    if (RgbaEquals(r, g, b, a, index[indexPos], index[indexPos + 1], index[indexPos + 2], index[indexPos + 3]))
                    {
                        bytes[p++] = (byte)(Index | (indexPos / 4));
                    }
                    else
                    {
                        index[indexPos] = r;
                        index[indexPos + 1] = g;
                        index[indexPos + 2] = b;
                        index[indexPos + 3] = a;

                        if (a == prevA)
                        {
                            int vr = r - prevR;
                            int vg = g - prevG;
                            int vb = b - prevB;

                            int vgr = vr - vg;
                            int vgb = vb - vg;

                            if (vr is > -3 and < 2 &&
                                vg is > -3 and < 2 &&
                                vb is > -3 and < 2)
                            {
                                counter++;
                                bytes[p++] = (byte)(Diff | (vr + 2) << 4 | (vg + 2) << 2 | (vb + 2));
                            }
                            else if (vgr is > -9 and < 8 &&
                                     vg is > -33 and < 32 &&
                                     vgb is > -9 and < 8
                                    )
                            {
                                bytes[p++] = (byte)(Luma | (vg + 32));
                                bytes[p++] = (byte)((vgr + 8) << 4 | (vgb + 8));
                            }
                            else
                            {
                                bytes[p++] = Rgb;
                                bytes[p++] = r;
                                bytes[p++] = g;
                                bytes[p++] = b;
                            }
                        }
                        else
                        {
                            bytes[p++] = Rgba;
                            bytes[p++] = r;
                            bytes[p++] = g;
                            bytes[p++] = b;
                            bytes[p++] = a;
                        }
                    }
                }

                prevR = r;
                prevG = g;
                prevB = b;
                prevA = a;
            }

            for (int padIdx = 0; padIdx < Padding.Length; padIdx++)
            {
                bytes[p + padIdx] = Padding[padIdx];
            }

            p += Padding.Length;

            return bytes[..p];
        }

        private static bool RgbaEquals(byte r1, byte g1, byte b1, byte a1, byte r2, byte g2, byte b2, byte a2) =>
        r1 == r2 &&
        g1 == g2 &&
        b1 == b2 &&
        a1 == a2;
    }
}
