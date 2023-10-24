using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace OrdealBuilder.Commands
{
    public class NewProjectCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "project files (*.ordpro)|*.ordpro";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream;
                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    stream.Close();
                    Project.Get().NewProject(Path.GetFullPath(saveFileDialog.FileName));
                }
            }
         }
    }
}
