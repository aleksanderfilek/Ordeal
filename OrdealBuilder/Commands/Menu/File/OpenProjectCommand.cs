using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrdealBuilder.Commands
{
    public class OpenProjectCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            StringBuilder filterBuilder = new StringBuilder();
            filterBuilder.Append("project files (*");
            filterBuilder.Append(Project.ProjectExtension);
            filterBuilder.Append("|*");
            filterBuilder.Append(Project.ProjectExtension);
            fileDialog.Filter = filterBuilder.ToString();
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                    Project.Get().OpenProject(fileDialog.FileName);
            }
        }
    }
}
