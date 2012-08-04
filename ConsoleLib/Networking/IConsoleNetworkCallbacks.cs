using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ConsoleLib.Console.Networking
{
    public interface IConsoleNetworkCallbacks
    {
        [OperationContract(IsOneWay = true)]
        void NewOutput(IEnumerable<string> text, string category);
    }
}
