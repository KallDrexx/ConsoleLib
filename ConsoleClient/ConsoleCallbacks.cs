using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleLib.Console.Networking;
using System.Windows.Forms;

namespace ConsoleClient
{
    public class ConsoleCallbacks : IConsoleNetworkCallbacks
    {
        public delegate void OutputReceivedHandler(IEnumerable<string> text, string category);

        public event OutputReceivedHandler OutputReceivedHandlers;

        public void NewOutput(IEnumerable<string> text, string category)
        {
            if (OutputReceivedHandlers != null)
                OutputReceivedHandlers(text, category);
        }
    }
}
