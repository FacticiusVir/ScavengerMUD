using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeper.ScavengerMud
{
    public interface ILogging
    {
        void Log(string message, TraceLevel level = TraceLevel.Info);
    }
}
