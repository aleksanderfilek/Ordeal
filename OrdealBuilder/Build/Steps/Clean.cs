using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder.Build.Steps
{
    internal class Clean : BuildStep
    {
        public override bool Run(BuildData Data)
        {
            string[] dirs = System.IO.Directory.GetDirectories(Data.OutputDir);
            foreach (string dir in dirs)
            {
                System.IO.Directory.Delete(dir, true);
            }

            string[] files = System.IO.Directory.GetFiles(Data.OutputDir);
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
            return true;
        }
    }
}
