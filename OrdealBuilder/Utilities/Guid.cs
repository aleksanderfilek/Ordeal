using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OrdealBuilder
{
    public class Guid
    {
        public static uint Create(string name)
        {
            uint crc = 0xFFFFFFFF;
            int n = name.Length;

            for (int i = 0; i < n; i++)
            {
                char ch = name[i];
                for (int j = 0; j < 8; j++)
                {
                    uint b = (ch ^ crc) & 1;
                    crc >>= 1;
                    if (b > 0) crc = crc ^ 0xEDB88320;
                    ch >>= 1;
                }
            }

            return ~crc;
        }
    }
}
