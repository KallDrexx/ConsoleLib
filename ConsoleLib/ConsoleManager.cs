using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleLib.Console
{
    public class ConsoleManager
    {
        public delegate void OutputDisplayUpdatedDelegate(IEnumerable<string> newText, string category);

        #region Singleton Support and Constructor

        private static readonly Lazy<ConsoleManager> _instance = new Lazy<ConsoleManager>(() => new ConsoleManager());
        public static ConsoleManager Instance { get { return _instance.Value; } }

        protected ConsoleManager() 
        {
            _registeredConsoleObjects = new List<RegisteredConsoleObject>();
        }

        #endregion

        #region Constants and Member Variables

        protected const string ECHOED_INPUT_PREPEND = "---> ";

        protected List<KeyValuePair<string, OutputDisplayUpdatedDelegate>> _outputDisplayUpdateHandlers;
        protected string _temporaryOutputcategory;
        protected IConsoleProcessor _consoleProcessor;
        protected List<RegisteredConsoleObject> _registeredConsoleObjects { get; set; }

        #endregion

        #region Public Methods

        public void InitializeProcessor(IConsoleProcessor processor)
        {
            _consoleProcessor = processor;
            InitConsole();
        }

        public void AddOutput(string text, string category = "")
        {
            if (_consoleProcessor == null)
                throw new InvalidOperationException("No console processor has been initialized");

            category = category ?? string.Empty;

            // If an output category is stored for processed output, use that
            if (_temporaryOutputcategory != null)
                category = _temporaryOutputcategory;

            var lines = (text ?? string.Empty).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Get all output handlers that are registered for this category
            var handlers = _outputDisplayUpdateHandlers
                                .Where(x => x.Key == string.Empty || x.Key.Equals(category, StringComparison.OrdinalIgnoreCase))
                                .Select(x => x.Value)
                                .Distinct();

            foreach (var handler in handlers)
                handler(lines, category);
        }

        public void ProcessInput(string input, string category = "")
        {
            if (_consoleProcessor == null)
                throw new InvalidOperationException("No console processor has been initialized");

            // Ignore empty input
            if (string.IsNullOrWhiteSpace(input))
                return;

            // Set the temporary category so all output triggered
            //   by console commands is done in the same category 
            //   as the process input call
            _temporaryOutputcategory = category;

            // Make sure to add the prepend to all lines
            foreach (string line in input.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                AddOutput(ECHOED_INPUT_PREPEND + line);

            AddOutput(_consoleProcessor.ProcessInput(input));
            _temporaryOutputcategory = null;

            // Refresh all the console objects so none are accidentally deleted
            RefreshObjectsInScript();
        }

        /// <summary>
        /// Registers an object to make it accessible via the console
        /// </summary>
        /// <param name="source">Object that is registering the object</param>
        /// <param name="name">Variable name that the object will be accessible in the console with</param>
        /// <param name="obj"></param>
        public void RegisterObjectToConsole<TObj>(object source, string name, TObj obj)
        {
            if (_consoleProcessor == null)
                throw new InvalidOperationException("No console processor has been initialized");

            _consoleProcessor.RegisterObject(name, obj);
            _registeredConsoleObjects.Add(new RegisteredConsoleObject { Name = name, Object = obj, Source = source });
        }

        /// <summary>
        /// Removes all references from script and the console manager for all objects from the specified source
        /// </summary>
        /// <param name="source"></param>
        public void UnregisterObjectsFromSource(object source)
        {
            if (_consoleProcessor == null)
                throw new InvalidOperationException("No console processor has been initialized");

            var foundObjs = new List<RegisteredConsoleObject>();
            foreach (var storedObj in _registeredConsoleObjects)
                if (storedObj.Source == source)
                    foundObjs.Add(storedObj);

            if (foundObjs.Count > 0)
            {
                foreach (var foundObj in foundObjs)
                {
                    // Remove references to the found object
                    _consoleProcessor.UnregisterObject(foundObj.Name, foundObj.Object);
                    _registeredConsoleObjects.Remove(foundObj);
                }
            }
        }

        public void UnregisterObject(string name, object obj)
        {
            if (_consoleProcessor == null)
                throw new InvalidOperationException("No console processor has been initialized");

            var foundObjs = new List<RegisteredConsoleObject>();
            foreach (var storedObj in _registeredConsoleObjects)
                if (storedObj.Object == obj && storedObj.Name == name)
                    foundObjs.Add(storedObj);

            if (foundObjs.Count > 0)
            {
                foreach (var foundObj in foundObjs)
                {
                    // Remove references to the found object
                    _consoleProcessor.UnregisterObject(foundObj.Name, foundObj.Object);
                    _registeredConsoleObjects.Remove(foundObj);
                }
            }
        }

        public void RegisterOutputUpdateHandler(OutputDisplayUpdatedDelegate handler, string category = "")
        {
            if (_consoleProcessor == null)
                throw new InvalidOperationException("No console processor has been initialized");

            category = (category ?? string.Empty).Trim();
            _outputDisplayUpdateHandlers.Add(new KeyValuePair<string, OutputDisplayUpdatedDelegate>(category, handler));
        }

        public void UnregisterOutputHandler(OutputDisplayUpdatedDelegate handler, string category = "")
        {
            if (_consoleProcessor == null)
                throw new InvalidOperationException("No console processor has been initialized");

            category = category ?? string.Empty;

            for (int x = _outputDisplayUpdateHandlers.Count - 1; x >= 0; x--)
            {
                // If no category specified, remove the handler from all categories
                if (_outputDisplayUpdateHandlers[x].Value == handler)
                    if (category.Trim() == string.Empty || category.Equals(_outputDisplayUpdateHandlers[x].Key))
                        _outputDisplayUpdateHandlers.RemoveAt(x);
            }
        }

        public List<string> GetCategoriesRegisteredForHandler(OutputDisplayUpdatedDelegate handler)
        {
            return _outputDisplayUpdateHandlers.Where(x => x.Value == handler)
                                            .Select(x => x.Key)
                                            .Distinct()
                                            .ToList();
        }

        public List<KeyValuePair<string, object>> GetKnownScriptObjects()
        {
            if (_consoleProcessor == null)
                throw new InvalidOperationException("No console processor has been initialized");

            return _registeredConsoleObjects
                .Select(x => new KeyValuePair<string, object>(x.Name, x.Object))
                .ToList();
        }

        #endregion

        #region Utility Methods

        protected void InitConsole()
        {
            _outputDisplayUpdateHandlers = new List<KeyValuePair<string, OutputDisplayUpdatedDelegate>>();

            //RegisterObjectToConsole(this, "console", new ConsoleUtils());
            RegisterObjectToConsole(this, "objects", (ConsoleUtils.GetConsoleObjectsDelegate)new ConsoleUtils().ListObjects);
            RegisterObjectToConsole(this, "details", (ConsoleUtils.GetObjectDetailsDelegate)new ConsoleUtils().ListObjectDetails);
            RegisterObjectToConsole(this, "properties", (ConsoleUtils.GetObjectDetailsDelegate)new ConsoleUtils().ListObjectProperties);
            RegisterObjectToConsole(this, "methods", (ConsoleUtils.GetObjectDetailsDelegate)new ConsoleUtils().ListObjectMethods);
        }

        /// <summary>
        /// Make sure that all objects registered in script haven't been accidentally overwritten
        /// </summary>
        protected void RefreshObjectsInScript()
        {
            // Refresh the objects with the console processor
            foreach (var storedObj in _registeredConsoleObjects)
                _consoleProcessor.RegisterObject(storedObj.Name, storedObj.Object);
        }

        #endregion
    }
}
