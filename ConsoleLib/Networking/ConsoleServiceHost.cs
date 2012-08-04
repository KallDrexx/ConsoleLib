using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ConsoleLib.Console.Networking
{
    public class ConsoleServiceHost : IDisposable
    {
        protected ServiceHost _host;

        public ConsoleServiceHost()
        {
            _host = new ServiceHost(typeof(ConsoleNetworkInterface), new Uri[] { new Uri("net.tcp://localhost:4000") });
            _host.AddServiceEndpoint(typeof(IConsoleInterface), new NetTcpBinding(), "net.tcp://localhost:4000");

            _host.Open();
        }

        public void Dispose()
        {
            _host.Close();
        }
    }
}
