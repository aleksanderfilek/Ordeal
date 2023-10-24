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
            Application.Current.Shutdown();
        }
    }
}
