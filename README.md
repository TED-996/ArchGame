ArchGame
=========

ArchGame is a C# architecture-oriented framework on top of on Microsoft's XNA. However, with little changes, ArchGame or parts of it can be laid on top of any C# game / graphics framework.

By using ArchGame, you can focus only on your game sacrificing the customizability of your code using an engine. Use the proven power of XNA without worrying about software architecture.

Features so far:
- Component-based system (The `ArchGame.Components` namespace); declaratively add components and stop worrying about them.
- Threaded content loading, fixing the long startup times by loading most of the content in a background thread.
- Complete input system, with keyboard, mouse and character-based input.
- State machine controlling the components and modules to load, draw and update.
- Service locator, offering global access to some modules (like logging). Use sparingly.
- Module factory / Dependency Injection container, taking the worries out of object construction, loading and disposing, avoiding global state and favouring loose coupling.
- Synchronous or asynchronous logging to file or to console.
- Lots of useful extension methods, ranging from Math to XML to Vector2 to SpriteBatch.

All the library code is [documented](http://ted-996.github.io/ArchGame/Docs/html/index.html). The project has its own [Github wiki](https://github.com/TED-996/ArchGame/wiki) with lots of information.

The example project (in the Test folder) clearly shows how easy working with ArchGame is.

Disclaimer: Some of the feature names might be poorly chosen. Please let me know. Also, please let me know if I'm doing something bad.

**Please read [the Wiki](https://github.com/TED-996/ArchGame/wiki) for detailed explanations on how to use ArchGame.**

##Links

* [Github.io page](http://ted-996.github.io/ArchGame/)
* [Documentation](http://ted-996.github.io/ArchGame/Docs/html/index.html)
* [Documentation .ZIP](http://ted-996.github.io/ArchGame/Docs/Docs.zip)
* [Github Wiki](https://github.com/TED-996/ArchGame/wiki)

##License

MIT
