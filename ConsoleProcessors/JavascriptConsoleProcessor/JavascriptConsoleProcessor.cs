using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noesis.Javascript;
using ConsoleLib.Console;

namespace ConsoleLib.Console.Processors
{
    public class JavascriptConsoleProcessor : IConsoleProcessor
    {
        protected JavascriptContext _jsContext;
        protected List<RegisteredConsoleObject> _registeredConsoleObjects { get; set; }
        protected object _lockObject = new object();

        public JavascriptConsoleProcessor()
        {
            _jsContext = new JavascriptContext();
            _registeredConsoleObjects = new List<RegisteredConsoleObject>();
        }

        public string ProcessInput(string input)
        {
            object result;
            try 
            { 
                // V8 engine is unstable if accessed simultaneously 
                lock (_lockObject)
                {
                    result = _jsContext.Run(input);
                }
            }
            catch (JavascriptException ex)
            {
                return ex.Message;
            }

            if (result != null)
                return result.ToString();

            return string.Empty;
        }

        public void RegisterObject(string name, object obj)
        {
            _jsContext.SetParameter(name, obj);
        }

        public void UnregisterObject(string name, object obj)
        {
            _jsContext.SetParameter(name, null);
        }
    }
}
