using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleLib.Console
{
    public class ConsoleOutputLogger
    {
        protected const string DEFAULT_LOG_FILE_TEMPLATE = @"logs/{0}.log";

        public void StartLogging(string category = "")
        {
            ConsoleManager.Instance.RegisterOutputUpdateHandler(OutputHandler, category);

            string message = string.Format("Logging started at {0} for category {1}",
                                            DateTime.Now.ToShortTimeString(), 
                                            category.Trim() == "" ? "<all categories>" : category);

            ConsoleManager.Instance.AddOutput(message, category);
        }

        public void StopLogging(string category = "")
        {
            string message = string.Format("Logging stopped at {0} for category {1}",
                                            DateTime.Now.ToShortTimeString(),
                                            category.Trim() == "" ? "<all categories>" : category);

            ConsoleManager.Instance.AddOutput(message, category);
            ConsoleManager.Instance.UnregisterOutputHandler(OutputHandler, category);
        }

        public string GetLoggedCategories()
        {
            var categories = ConsoleManager.Instance.GetCategoriesRegisteredForHandler(OutputHandler);
            if (categories == null || categories.Count == 0)
                return "No console categories currently being logged";

            string result = "Logged categories: ";
            for (int x = 0; x < categories.Count; x++)
            {
                if (x > 0)
                    result += ", ";

                if (categories[x].Trim() == string.Empty)
                    result += "<all categories>";
                else
                    result += categories[x].Trim();
            }

            return result;
        }

        protected void OutputHandler(IEnumerable<string> text, string category)
        {
            string filename = string.Format(DEFAULT_LOG_FILE_TEMPLATE, DateTime.Now.ToString("yyyyMMdd"));

            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));

            using (var stream = File.AppendText(filename))
            {
                foreach (string line in text)
                {
                    stream.WriteLine("{0}: {1}", category.Trim(), line);                
                }
            }
        }
    }
}
