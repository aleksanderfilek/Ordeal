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
            StringBuilder filterBuilder = new StringBuilder();
            filterBuilder.Append("project files (*");
            filterBuilder.Append(Project.ProjectExtension);
            filterBuilder.Append("|*");
            filterBuilder.Append(Project.ProjectExtension);
            saveFileDialog.Filter = filterBuilder.ToString();
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Project.Get().NewProject(Path.GetFullPath(saveFileDialog.FileName));
            }
         }
    }
}
