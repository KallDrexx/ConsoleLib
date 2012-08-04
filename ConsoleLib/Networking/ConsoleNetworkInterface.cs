using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ConsoleLib.Console.Networking
{
    public class ConsoleNetworkInterface : IConsoleInterface, IDisposable
    {
        protected IConsoleNetworkCallbacks _callback;

        public ConsoleNetworkInterface()
        {
            _callback = OperationContext.Current.GetCallbackChannel<IConsoleNetworkCallbacks>();
            ConsoleManager.Instance.RegisterOutputUpdateHandler(OutputHandler);
            ConsoleManager.Instance.AddOutput("Networked Console Connected");
        }

        public void Dispose()
        {
            ConsoleManager.Instance.UnregisterOutputHandler(OutputHandler);
            ConsoleManager.Instance.AddOutput("Networked Console Disconnected");
        }

        public void Subscribe()
        {
            // Only needed to start receiving output
        }

        public void Ping()
        {
            // This is just used to make sure a connection is still active
        }

        public void ProcessInput(string input)
        {
            ConsoleManager.Instance.ProcessInput(input);
        }

        public void ChangeCategory(string category)
        {
            ConsoleManager.Instance.UnregisterOutputHandler(OutputHandler);
            ConsoleManager.Instance.RegisterOutputUpdateHandler(OutputHandler, category);
        }

        protected void OutputHandler(IEnumerable<string> text, string category)
        {
            if (_callback != null)
                _callback.NewOutput(text, category);
        }
    }
}
