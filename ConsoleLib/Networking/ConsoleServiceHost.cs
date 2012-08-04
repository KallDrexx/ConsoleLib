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

        public ConsoleServiceHost(string serverName, int port)
        {
            var uri = new Uri(string.Format("net.tcp://{0}:{1}", serverName, port));
            _host = new ServiceHost(typeof(ConsoleNetworkInterface), uri);
            _host.AddServiceEndpoint(typeof(IConsoleInterface), new NetTcpBinding(), uri);

            _host.Open();
        }

        public void Dispose()
        {
            _host.Close();
        }
    }
}
