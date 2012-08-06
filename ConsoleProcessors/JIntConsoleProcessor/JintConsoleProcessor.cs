using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jint;

namespace ConsoleLib.Console.Processors
{
    public class JintConsoleProcessor : IConsoleProcessor
    {
        protected JintEngine _engine;

        public JintConsoleProcessor()
        {
            _engine = new JintEngine();
        }

        public string ProcessInput(string input)
        {
            try
            {
                var result = _engine.Run(input);
                if (result != null)
                    return result.ToString();

                return string.Empty;
            }
            catch (JintException ex)
            {
                return ex.Message;
            }
        }

        public void RegisterObject<TObj>(string name, TObj obj)
        {
            // If the object is a delegate, add it with SetFunction instead of SetParameter
            if (obj.GetType().IsSubclassOf(typeof(Delegate)))
                _engine.SetFunction(name, obj as Delegate);
            else
                _engine.SetParameter(name, obj);
        }

        public void UnregisterObject(string name, object obj)
        {
            _engine.SetParameter(name, null);
        }
    }
}
