using System.Collections.Generic;

namespace ArchGame.Modules {
	/// <summary>
	/// The IModuleRequester is an interface for classes that request modules registered in the ModuleFactory.
	/// </summary>
	public interface IModuleRequester {
		/// <summary>
		/// Returns an IEnumerable$lt;string&gt; with the type names of the requested modules.
		/// </summary>
		/// <returns></returns>
		IEnumerable<string> GetRequestedModules(); 

		/// <summary>
		/// Prompts the IModuleRequester to get the modules from the ModuleCollection.
		/// It may then use ModuleFactory.FulfillRequestNow(IModuleRequester requester) to fulfill the requests of other members.
		/// </summary>
		/// <param name="collection">The ModuleCollection with the requested modules. No other modules are present other than
		/// the reqested ones.</param>
		/// <param name="factory">The ModuleFactory for fulfilling the requests of other members</param>
		void SetModules(ModuleCollection collection, ModuleFactory factory);
	}
}