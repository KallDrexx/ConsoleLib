using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleLib.Console
{
    public interface IConsoleProcessor
    {
        /// <summary>
        /// Attempts to processes the desired console input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ConsoleInputException">Thrown when an exception occurs processing input</exception>
        string ProcessInput(string input);
        void RegisterObject(string name, object obj);
        void UnregisterObject(string name, object obj);
    }
}
