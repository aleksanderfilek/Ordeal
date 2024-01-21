using OrdealBuilder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder.Commands
{
    public class OpenOutputLogCommand : CommandBase
    {

        public override void Execute(object? parameter)
        {
            NavigatorViewModel.Get().OpenOutputLog();
        }
    }
}
