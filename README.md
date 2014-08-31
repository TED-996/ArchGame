ArchGame
=========

ArchGame is a C# architecture-oriented framework based on Microsoft's XNA. However, with little changes, ArchGame or parts of it can be laid on top of any C# game / graphics framework.

Features so far:
- Threaded content loading
- Logging
- Service locator/provider
- Module factory / Dependency Injection container
- Component-based updating and drawing
- Lots of extension methods

Disclaimer: Some of the feature names might be poorly chosen. Please let me know. Also, please let me know if I'm doing something bad.

---

##ModuleFactory explanation

The ModuleFactory class is a dependency injection container and object manager. Basically, it, at an appropriate time, constructs all the objects of the internal representation of the game ("Modules") -for example World, Physics, PlayerManager, Network, etc., classes each dealing with one of the modules, sections of the game code.

The modules or other classes can implement `IModuleRequester`. This is a way of exposing the dependencies to other modules or classes. This interface requires two methods to be implemented, `IEnumerable<string> GetRequestedModules()`, which returns, for example, an array of strings with the type names that are requested, for example `new[] {"WorldData", "PlayerManager", "NetworkManager"}`, and `void SetModules(ModuleCollection collection, ModuleFactory factory);`. In the last method, the requesters will retrieve the modules from the `ModuleCollection`, like so: `physicsManager = collection.GetModule<IPhysicsManager>();`

Modules are tracked for requests fullfilling, content loading and disposing. Each action happens at a definite time. Modules registered at a later moment will not have request fullfilled or content loaded (disposing happens only when the game exits)

Modules are registered in the factory in the ArchGame implementation constructor. In order to register modules in the factory, you have the following options:

* `RegisterModule`: register a module that will be constructed, will have requests fullfilled, etc and will be requestable.
* `RegisterConstructor`: registers a module that will be constructed and will have all the requests fullfilled, etc, but will not be requestable.
* `RegisterProvider`: manually make a module requestable, but not construct it, fullfill its requests, etc. You will not be able to set the module object at a later date.
* `RegisterRequester`: register an already-constructed object. Its requests will be fullfilled, but it will not be tracked for content loading or disposing.
* `RegisterObject`: register an already-constructed object. It will be tracked for requests, content and disposing.

###Factory Feature table:

|Feature                | Module | Constructor | Provider | Requester | Object |
|-----------------------|--------|-------------|----------|-----------|--------|
| Constructed           | [x]    | [x]         | []       | []        | []     |
| Requests fullfilled   | [x]    | [x]         | []       | [x]       | [x]    |
| Content loaded        | [x]    | [x]         | []       | []        | [x]    |
| Disposed              | [x]    | [x]         | []       | []        | [x]    |
| Requestable           | [x]    | []          | [x]      | []        | []     |

In order to fullfill a request after the factory has passed this step, you can use the `FullfillRequestNow(IModuleRequester requester)` method.


## Application Lifetime

####Legend
* (v) = virtual. You can add your code here in implementation.
* (a) = abstract. This is code required in the implementation.
* (v, e) = virtual, empty. This is a virtual method with no code in the abstract ArchGame class. You don't have to override this, but it could be useful.

####Lifetime

Main thread:
* ArchGame constructor
* Game implementation constructor. **Register modules here**
* ArchGame.Initialize (v)
* ArchGame.LoadContent
    * ArchGame.SetServiceProviders (v)
    * ArchGame.GetServices (v, e)
    * Content preloading. **Some content is loaded before the window appears**
    * ArchGame.GetLoadingState (a)
    * YourLoadingState.LoadContent *(Hopefully, loaded in the preloading phase)*
    * **Loading thread starts.**
    * **Window appears**, drawing the loading state

Loading thread:
* ModuleFactory.ConstructModules
    * Modules are constructed
    * Modules are tracked
    * Some modules are made requestable
* ModuleFactory.FullfillRequests
    * YourModuleRequester.GetRequestedModules
    * YourModuleRequester.SetModules
* **The rest of the content is loaded**
* **The tracked modules' content is loaded**
* ArchGame.AfterLoadContent (v)
* ArchGame.GetAfterLoadState (a) **This is the menu/intro state**
* YourMenuState.LoadContent *(Hopefully, loaded before)*
* **Menu state appears**
* Thread ends.

Main thread (cont.)

* Update/Draw until Game implementation.Exit called
* Content disposed
* ModuleFactory.Dispose *(the tracked modules are disposed)*

---

##LoadableSet explanation
The LoadableSet is used to hold two sets of IArchLoadables. The first will be loaded in the preloading phase (before the window appears). This is the content critical to the loading state. The second will be loaded while the loading state is shown on the screen, in another thread. The content will be garbage collected in the IArchLoadables, but will remain in the XNA ContentManager's cache and will be retrieved from memory when the states will request them. Another solution is to wrap content loading in a module (for example, the world renderer will be inside the World module and will have content loaded by the ModuleFactory)

---

##License

MIT
