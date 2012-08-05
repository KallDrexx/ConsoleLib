About
=======
ConsoleLib is meant to be an easily plugable debug console for use in XNA games.  It allows you to pass in C# objects and interact with them at runtime via scripting languages.  While this was originally designed to be used with the FlatRedBall 2d engine, it can be used with any game engine. 

It includes both an backed console management system and a networked client to remotely interact with your game's console.

![example image](http://dl.dropbox.com/u/6753359/console5.PNG)

Quick Start
============
Add the ConsoleLib project to your project and add the following code:

> ConsoleManager.Instance.InitializeProcessor(new JavascriptConsoleProcessor());
> var host = new ConsoleServiceHost("localhost", 4000);

Expose any objects you wish to the console via

> ConsoleManager.Instance.RegisterObjectToConsole(this, "NameFromConsole", object);

Remove all registered objects from console

> ConsoleManager.Instance.UnregisterObjectsFromSource(this);

Run the networked console client (ConsoleClient project) and connect to your game.

Console Commands
================
The console comes with several commands built in to help with actions.

> objects() - This list all c# objects the console knows about
> properties("object") - This lists all public properties for the object
> methods("object") - This lists all public methods for the specified object

Console Scripting
================
By default the console accepts commands in Javascript using the [Javascript.Net](http://javascriptdotnet.codeplex.com/).  However, any language can be used simply by creating a custom IConsoleProcessor class for your desired language, wheither that's python, lua, or whatever you are interested in.  Once an IConsoleProcessor derived class has been create it set it up with the Console Manager by calling

> Consolemanager.Instance.initializeProcessor(new YourCustomConsoleProcessor());