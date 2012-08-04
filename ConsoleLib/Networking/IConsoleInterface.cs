using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ConsoleLib.Console.Networking
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IConsoleNetworkCallbacks))]
    public interface IConsoleInterface
    {
        [OperationContract]
        void Subscribe();

        [OperationContract]
        void ProcessInput(string input);

        [OperationContract]
        void ChangeCategory(string category);
    }
}
