using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder.Build.Steps
{
    internal class CopyLibs : BuildStep
    {
        public override bool Run(BuildData Data)
        {
            string rootDir = System.IO.Path.GetDirectoryName(Project.Get().RootDirectory.Path);
            string libsDir = Path.Combine(rootDir, Data.Profile.LibsDir);
            if (System.IO.Directory.Exists(libsDir))
            {
                var filePaths = System.IO.Directory.GetFiles(libsDir);
                foreach (string file in filePaths)
                {
                    string destFileName = Path.Combine(Data.OutputDir, Path.GetFileName(file));
                    System.IO.File.Copy(file, destFileName, true);
                }
            }
            return true;
        }
    }
}
