using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ConsoleLib.Console
{
    public class ConsoleUtils
    {
        public delegate string GetConsoleObjectsDelegate();
        public delegate string GetObjectDetailsDelegate(string name);

        /// <summary>
        /// Lists all available c# objects known to the console
        /// </summary>
        public string ListObjects()
        {
            var sb = new StringBuilder();
            sb.Append("Available Console Objects");
            sb.Append(Environment.NewLine);
            sb.Append("------------------------------------");
            sb.Append(Environment.NewLine);

            var consoleObjs = ConsoleManager.Instance.GetKnownScriptObjects();

            foreach (var obj in consoleObjs.OrderBy(x => x.Key))
            {
                sb.Append(obj.Key);
                sb.Append(" (");
                sb.Append(obj.Value.GetType());
                sb.Append(")");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public string ListObjectDetails(string name)
        {
            var obj = ConsoleManager.Instance
                                    .GetKnownScriptObjects()
                                    .Where(x => x.Key == name)
                                    .Select(x => x.Value)
                                    .FirstOrDefault();

            var output = new StringBuilder();
            output.Append("Details for: ");
            output.Append(name);
            output.Append(Environment.NewLine);
            output.Append("------------------------");
            output.Append(Environment.NewLine);

            if (obj == null)
            {
                output.Append("No object registered with that name");
            }
            else
            {
                output.Append("Type: ");
                output.Append(obj.GetType());
                output.Append(Environment.NewLine);
                output.Append(Environment.NewLine);
                
                // Get all the properties
                output.Append("Properties: ");
                output.Append(obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).Length);
                output.Append(Environment.NewLine);
                output.Append(Environment.NewLine);

                // Get all public methods
                output.Append("Methods: ");
                output.Append(obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).Length);
                output.Append(Environment.NewLine);
                output.Append(Environment.NewLine);
            }

            return output.ToString();
        }

        public string ListObjectMethods(string name)
        {
            var obj = ConsoleManager.Instance
                                    .GetKnownScriptObjects()
                                    .Where(x => x.Key == name)
                                    .Select(x => x.Value)
                                    .FirstOrDefault();

            var output = new StringBuilder();
            output.Append("Methods for: ");
            output.Append(name);
            output.Append(Environment.NewLine);
            output.Append("------------------------");
            output.Append(Environment.NewLine);

            if (obj == null)
            {
                output.Append("No object registered with that name");
            }
            else
            {
                var methods = obj.GetType()
                                 .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                 .ToArray();

                if (methods.Length == 0)
                {
                    output.Append("None");
                    output.Append(Environment.NewLine);
                }

                foreach (var method in methods)
                {
                    output.Append(method.Name);
                    output.Append("(");

                    var parameters = method.GetParameters();
                    for (int x = 0; x < parameters.Length; x++)
                    {
                        if (x > 0)
                            output.Append(", ");

                        output.Append(parameters[x].Name);
                        output.Append("[");
                        output.Append(parameters[x].ParameterType);
                        output.Append("]");
                    }

                    output.Append(")  Returns: ");
                    output.Append(method.ReturnType);
                    output.Append(Environment.NewLine);
                }
            }

            return output.ToString();
        }

        public string ListObjectProperties(string name)
        {
            var obj = ConsoleManager.Instance
                                    .GetKnownScriptObjects()
                                    .Where(x => x.Key == name)
                                    .Select(x => x.Value)
                                    .FirstOrDefault();

            var output = new StringBuilder();
            output.Append("Properties for: ");
            output.Append(name);
            output.Append(Environment.NewLine);
            output.Append("------------------------");
            output.Append(Environment.NewLine);

            if (obj == null)
            {
                output.Append("No object registered with that name");
            }
            else
            {
                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                properties = properties.OrderBy(x => x.Name).ToArray();

                if (properties.Length == 0)
                {
                    output.Append("None");
                    output.Append(Environment.NewLine);
                }

                foreach (var property in properties)
                {
                    output.Append(property.Name);
                    output.Append(" [");
                    output.Append(property.PropertyType);
                    output.Append("]");
                    output.Append(Environment.NewLine);
                }
            }

            return output.ToString();
        }
    }
}
