using System.Collections.Generic;

namespace SFMLGame.Modules {
	public interface IModuleRequester {
		IEnumerable<string> GetRequestedModules(); 
		void SetModules(ModuleCollection collection);
	}

	/* How things go: A module must have an empty constructor. The factory maintains a list of IModuleConstructors. The
	 * factory constructs all of them, keeps them in memory and fullfills all requesters from those IModules and others.
	 * The raw modules are kept inside providers. 
	 * 
	 * So:
	 * GameImpl.Ctor -> RegisterCtor, RegisterProvider, RegisterRequester
	 * Game.LoadContent -> Construct, Fullfill
	 * 
	 * 
	 */
}