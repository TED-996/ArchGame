using System.Collections.Generic;

namespace ArchGame.Modules {
	public interface IModuleRequester {
		IEnumerable<string> GetRequestedModules(); 
		void SetModules(ModuleCollection collection, ModuleFactory factory);
	}
}