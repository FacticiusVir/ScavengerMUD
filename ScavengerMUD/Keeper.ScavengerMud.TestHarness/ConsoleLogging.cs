using System;
using System.Diagnostics;

namespace Keeper.ScavengerMud.TestHarness
{
    internal class ConsoleLogging
        : ILogging
    {
        public void Log(string message, TraceLevel level)
        {
            Console.WriteLine("{0}: {1}", level, message);
        }
    }
}