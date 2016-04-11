using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleLib.Console;
using ConsoleLib.Console.Networking;
using ConsoleLib.Processors.JintConsoleProcessor;

namespace TestConsoleHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            var test = new TestUtils();

            // Initialize the console
            var sourceObj = new object();
            ConsoleManager.Instance.InitializeProcessor(new JintConsoleProcessor());
            ConsoleManager.Instance.RegisterObjectToConsole(sourceObj, "test", test);
            ConsoleLib.Helpers.Formatters.RegisterJsonObjectFormatter(sourceObj);

            dynamic expandoTest = new ExpandoObject();;
            expandoTest.abc = "def";
            ConsoleManager.Instance.RegisterObjectToConsole(sourceObj, "expandoTest", expandoTest);

            var host = new ConsoleServiceHost("localhost", 4000);

            Console.WriteLine("Server started, waiting for commands");

            while (!test.Quit)
            {
                // Keep looping until quit has been called
                Thread.Sleep(100);
            }

            ConsoleManager.Instance.UnregisterObjectsFromSource(sourceObj);
        }

        public class TestUtils
        {
            public bool Quit { get; set; }

            public void Print(string message)
            {
                ConsoleManager.Instance.AddOutput(message);
            }
        }
    }
}
