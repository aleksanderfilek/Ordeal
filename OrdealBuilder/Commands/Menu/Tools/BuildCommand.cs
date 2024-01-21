using OrdealBuilder.Build;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace OrdealBuilder.Commands
{
    public class BuildCommand : CommandBase
    {

        private BackgroundWorker worker;

        public override void Execute(object? parameter)
        {
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = false;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                BuildProcess buildProcess = new BuildProcess(folderBrowser.SelectedPath);
                worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerAsync(buildProcess);
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            BuildProcess buildProcess = (BuildProcess)e.Argument;
            buildProcess.Start();
        }
    }
}
