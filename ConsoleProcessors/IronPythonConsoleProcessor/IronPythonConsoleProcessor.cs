﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleLib.Console;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

namespace ConsoleLib.Console.Processors
{
    public class IronPythonConsoleProcessor : IConsoleProcessor
    {
        protected ScriptEngine _engine;
        protected ScriptScope _scope;

        public IronPythonConsoleProcessor()
        {
            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();
        }

        public string ProcessInput(string input)
        {
            try
            {
                var result = _engine.Execute(input, _scope);
                if (result == null)
                    return string.Empty;

                return result.ToString();
            }

            // Due to the complexity and number of exceptions that IronPython
            //   throws, I'm just catching all exceptions
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void RegisterObject<TObj>(string name, TObj obj)
        {
            _scope.SetVariable(name, obj);
        }

        public void UnregisterObject(string name, object obj)
        {
            _scope.SetVariable(name, null);
        }
    }
}
