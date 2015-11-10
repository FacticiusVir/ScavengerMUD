using System;

namespace Keeper.ScavengerMud.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(5002);

            var game = new Game(server, new ConsoleLogging());

            server.Start();

            Console.WriteLine("Running...");

            Console.ReadLine();

            server.Stop();
        }
    }
}
