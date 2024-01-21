using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdealBuilder;
using System.Threading;
using System.ComponentModel;

namespace OrdealBuilder.Build.Steps
{
    internal class Compile : BuildStep
    {
        private List<string> sourceFiles = new List<string>();
        private List<string> objectFiles = new List<string>();
        private List<string> OutputMessages = new List<string>();

        private enum CompileState
        {
            None,
            GatheringFiles,
            Compiling,
            Linking
        }

        private CompileState State = CompileState.None;

        public override bool Run(BuildData Data)
        {
            GatherSourceFiles(Data);
            CompileSource(Data);
            LinkObjectFiles(Data);
            return true;
        }

        private void GatherSourceFiles(BuildData Data)
        {
            State = CompileState.GatheringFiles;
            string path = Project.Get().ProjectSourcePath;
            string fileSeatchPattern = "*" + Data.Profile.Extension;
            RecursiveSourceFileSearch(path, fileSeatchPattern);
        }

        private void RecursiveSourceFileSearch(string dirPath, string fileSeatchPattern)
        {
            string[] filePaths = System.IO.Directory.GetFiles(dirPath, fileSeatchPattern);
            sourceFiles.AddRange(filePaths);

            string[] dirs = System.IO.Directory.GetDirectories(dirPath);
            foreach (string dir in dirs)
            {
                RecursiveSourceFileSearch(dir, fileSeatchPattern);
            }
        }


        private void CompileSource(BuildData Data)
        {
            State = CompileState.Compiling;

            if (System.IO.Directory.Exists(System.IO.Path.Combine(Project.Get().ProjectPath, "object")))
            {
                System.IO.Directory.Delete(System.IO.Path.Combine(Project.Get().ProjectPath, "object"), true);
            }

            StringBuilder startCommandBuilder = new StringBuilder();

            foreach (string define in Data.Profile.CompilerDefines)
            {
                startCommandBuilder.Append(" -D");
                startCommandBuilder.Append(define);
            }

            startCommandBuilder.Append(" -g3 -Wall -c");

            int counter = 0;
            foreach (string sourceFile in sourceFiles)
            {
                counter++;

                string relativePath = System.IO.Path.GetRelativePath(Project.Get().ProjectSourcePath, System.IO.Path.GetDirectoryName(sourceFile));
                if(relativePath.CompareTo(".") == 0)
                {
                    relativePath = "";
                }

                OutputLogManager.Log(OutputLogType.Normal, "[" + counter.ToString() + "/" + sourceFiles.Count + "] " + relativePath + "\\" + System.IO.Path.GetFileName(sourceFile));

                string objectDirPath = System.IO.Path.Combine("object", relativePath);
                objectDirPath = System.IO.Path.Combine(Project.Get().ProjectPath, objectDirPath);
                System.IO.Directory.CreateDirectory(objectDirPath);
                string objectFileName = System.IO.Path.Combine(objectDirPath, System.IO.Path.GetFileName(sourceFile));
                objectFileName = System.IO.Path.ChangeExtension(objectFileName, ".o");

                objectFiles.Add(objectFileName);

                StringBuilder endCommandBuilder = new StringBuilder();
                endCommandBuilder.Append(startCommandBuilder);
                endCommandBuilder.Append(" ");
                endCommandBuilder.Append(sourceFile);
                endCommandBuilder.Append(" -o ");
                endCommandBuilder.Append(objectFileName);

                RunProcess(endCommandBuilder.ToString());
            }
        }

        private void LinkObjectFiles(BuildData Data)
        {
            State = CompileState.Linking;

            string libsDir = System.IO.Path.Combine(Project.Get().ProjectPath, Data.Profile.LibsDir);

            StringBuilder commandBuilder = new StringBuilder();
            foreach (string define in Data.Profile.CompilerDefines)
            {
                commandBuilder.Append(" -D");
                commandBuilder.Append(define);
            }

            commandBuilder.Append(" -L");
            commandBuilder.Append(libsDir);

            commandBuilder.Append(" -o ");
            commandBuilder.Append(Data.OutputDir);
            commandBuilder.Append("\\");
            commandBuilder.Append(Data.Profile.Name + ".exe");


            foreach (string objectFile in objectFiles)
            {
                commandBuilder.Append(" ");
                commandBuilder.Append(objectFile);
            }

            foreach (string lib in Data.Profile.Libs)
            {
                commandBuilder.Append(" ");
                commandBuilder.Append(lib);
            }

            RunProcess(commandBuilder.ToString());
        }

        private void RunProcess(string Command)
        {
            OutputMessages.Clear();

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "gcc.exe";
            startInfo.Arguments = Command;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_OutputDataReceived;
            process.StartInfo = startInfo;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            OutputLogManager.LogMultiple(OutputLogType.Warning, OutputMessages);
        }

        private void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if(e.Data != null)
            {
                OutputMessages.Add(e.Data.ToString());
            }
        }
    }
}
