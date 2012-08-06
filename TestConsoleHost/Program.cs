using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleLib.Console;
using ConsoleLib.Console.Processors;
using ConsoleLib.Console.Networking;
using System.Threading;

namespace TestConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new TestUtils();

            // Initialize the console
            var sourceObj = new object();
            ConsoleManager.Instance.InitializeProcessor(new JintConsoleProcessor());
            ConsoleManager.Instance.RegisterObjectToConsole(sourceObj, "test", test);
            var host = new ConsoleServiceHost("localhost", 4000);

            Console.WriteLine("Server started, waiting for commands");

            while (!test.Quit)
            {
                // Keep looping until quit has been called
                Thread.Sleep(100);
            }

            ConsoleManager.Instance.UnregisterObjectsFromSource(sourceObj);
        }
    }
}
