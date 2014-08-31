using System.Collections.Generic;

namespace ArchGame.Modules {
	public interface IModuleRequester {
		IEnumerable<string> GetRequestedModules(); 
		void SetModules(ModuleCollection collection, ModuleFactory factory);
	}

	/* How things go: The factory maintains a list of IModuleConstructors. The factory constructs all of them, keeps them in
	 * memory and fullfills all requesters from those and others. The raw modules are kept inside providers. 
	 * 
	 * So:
	 * GameImpl.Ctor -> RegisterCtor, RegisterProvider, RegisterRequester
	 * Game.LoadContent -> Construct, Fullfill
	 * 
	 * RegisterConstructor: a module that will be constructed and will have all the requests fullfilled, but will not be requestable.
	 * RegisterProvider: manually make a module requestable, but not construct it or fullfill its requests
	 * RegisterModule: register a module that will be constructed, will have requests fullfilled, etc and will be requestable.
	 * 
	 * Feature:				RegisterConstructor		RegisterProvier		RegisterModule	RegisterRequester	RegisterObject
	 * Construct			yes						no					yes				no					no
	 * Requests fullfilled	yes						no					yes				yes					yes
	 * Content loaded		yes						no					yes				no					yes
	 * Disposed				yes						no					yes				no					yes
	 * Requestable			no						yes					yes				no					no
	 * 
	 */
}