﻿using System;
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
            catch (Exception ex)
            {
                return $"Exception: {ex.GetType().Name} - {ex.Message}";
            }
        }

        public void RegisterObject<TObj>(string name, TObj obj)
        {
            _engine.SetValue(name, obj);
        }

        public void UnregisterObject(string name, object obj)
        {
            _engine.SetValue(name, (string)null);
        }
    }
}
