using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleLib.Console;
using Jint;
using Jint.Runtime;

namespace ConsoleLib.Processors.JintConsoleProcessor
{
    public class JintConsoleProcessor : IConsoleProcessor
    {
        private readonly Engine _engine = new Engine();

        public string ProcessInput(string input)
        {
            try
            {
                var result = _engine.Execute(input).GetCompletionValue().ToObject();
                if (result != null)
                    return result.ToString();

                return string.Empty;
            }
            catch (JavaScriptException ex)
            {
                return ex.Message;
            }
        }

        public void RegisterObject<TObj>(string name, TObj obj)
        {
            // If the object is a delegate, add it with SetFunction instead of SetParameter
            if (obj.GetType().IsSubclassOf(typeof(Delegate)))
                _engine.SetValue(name, obj as Delegate);
            else
                _engine.SetValue(name, obj);
        }

        public void UnregisterObject(string name, object obj)
        {
            _engine.SetValue(name, (string)null);
        }
    }
}
