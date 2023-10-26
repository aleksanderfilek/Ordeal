using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrdealBuilder.Commands
{
    public class ExitCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            if (Project.Get().IsSomethingToSave())
            {
                MessageBoxResult dr = System.Windows.MessageBox.Show("There are some modified file. Do you want save them?", "Save changes", MessageBoxButton.YesNo);
                if (dr == MessageBoxResult.Yes)
                {
                    Project.Get().SaveAll();
                }
            }
            System.Windows.Application.Current.Shutdown();
        }
    }
}
