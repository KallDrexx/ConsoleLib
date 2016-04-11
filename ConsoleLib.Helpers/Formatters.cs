using System;
using ConsoleLib.Console;
using Newtonsoft.Json;

namespace ConsoleLib.Helpers
{
    public static class Formatters
    {
        /// <summary>
        /// Allows calling "toJson(object)" from the console to get a json representation
        /// of the object
        /// </summary>
        /// <param name="registrar">Object the json formatter function's registration should be associated with</param>
        public static void RegisterJsonObjectFormatter(object registrar)
        {
            ConsoleManager.Instance.RegisterObjectToConsole(registrar, "toJson", (Func<object, string>)ToJson);
        }

        private static string ToJson(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            return JsonConvert.SerializeObject(obj);
        }
    }
}
