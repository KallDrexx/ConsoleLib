About
=======
ConsoleLib is meant to be an easily plugable debug console, primarily meant for use in XNA games but can be used in any type of application.  It allows you to pass in C# objects and interact with them at runtime via scripting languages.

It includes both an backed console management system and a networked client to remotely interact with your game's console.

![example image](https://i.imgur.com/D94G0EL.png)

Quick Start
============
Add the ConsoleLib project to your project and add the following code:

    ConsoleManager.Instance.InitializeProcessor(new JavascriptConsoleProcessor());
    var host = new ConsoleServiceHost("localhost", 4000);

Expose any objects you wish to the console via

    ConsoleManager.Instance.RegisterObjectToConsole(this, "NameFromConsole", object);

Remove all registered objects from console

    ConsoleManager.Instance.UnregisterObjectsFromSource(this);
	
To have our code output data to the console add the following code

    ConsoleManager.Instance.AddOutput("message", "category");

Run the networked console client (ConsoleClient project) and connect to your game.

Console Commands
================
The console comes with several commands built in to help with actions.

    objects() - This list all c# objects the console knows about
    properties("object") - This lists all public properties for the object
    methods("object") - This lists all public methods for the specified object

Console Scripting
================
The repository comes with console processors that use:

* [Javascript.Net](http://javascriptdotnet.codeplex.com/) - Uses Google's V8 engine for fast Javascript processing
* [JInt](http://jint.codeplex.com/) - Provides better .Net integration, allowing instanciating .net objects via javascript
* [IronPython](http://www.ironpython.net/) - Provides Python support

However, any language can be supported simply by creating a custom IConsoleProcessor class for your desired language.  Once an IConsoleProcessor derived class has been create it set it up with the Console Manager by calling

	Consolemanager.Instance.initializeProcessor(new YourCustomConsoleProcessor());
	
Networked Console Client
========================
The ConsoleClient project is a Winforms project that uses WCF NetTcpBindings to connect to a system setup with the ConsoleLib ConsoleServiceHost.  It allows you to:
* Run console scripts remotely
* View console output remotely
* Pause console output
* Save and load scripts for later reference
* Save console output to files for later review
