using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace OrdealBuilder.Build.Steps
{
    internal class CopyResources : BuildStep
    {
        public override bool Run(BuildData Data)
        {
            string rootDir = System.IO.Path.GetDirectoryName(Project.Get().RootDirectory.Path);
            RecursiveResourceCopy(Data, rootDir, Project.Get().RootDirectory);
            return true;
        }

        private static void RecursiveResourceCopy(BuildData Data, string RootDir, Directory Dir)
        {
            string dirRelativePath = System.IO.Path.GetRelativePath(RootDir, Dir.Path);
            string dirPath = System.IO.Path.Combine(Data.OutputDir, dirRelativePath);
            System.IO.Directory.CreateDirectory(dirPath);

            List<File> files = Dir.Files;
            foreach (File file in files) 
            {
                if (file.Asset.Type != AssetType.Invalid)
                {
                    string resourcePath = file.Asset.Path;
                    string relativePath = System.IO.Path.GetRelativePath(RootDir, resourcePath);
                    string destFileName = System.IO.Path.Combine(Data.OutputDir, relativePath);
                    System.IO.File.Copy(resourcePath, destFileName, true);
                }
            }

            List<Directory> directories = Dir.Directories;
            foreach (Directory dir in directories)
            {
                RecursiveResourceCopy(Data, RootDir, dir);
            }
        }
    }
}
