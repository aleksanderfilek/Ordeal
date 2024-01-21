using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrdealBuilder.Build
{
    public abstract class BuildStep
    {
        public abstract bool Run(BuildData Data);
    }
}
