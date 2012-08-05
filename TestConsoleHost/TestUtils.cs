using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleLib.Console;

namespace TestConsoleHost
{
    public class TestUtils
    {
        public bool Quit { get; set; }

        public void Print(string message)
        {
            ConsoleManager.Instance.AddOutput(message);
        }
    }
}
