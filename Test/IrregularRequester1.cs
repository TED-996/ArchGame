using System.Collections.Generic;
using SFMLGame.Modules;
using SFMLGame.Services;

namespace SFMLGameTest {
	public class IrregularRequester1 : IModuleRequester {
		TestModule1 module1;
		public IEnumerable<string> GetRequestedModules() {
			return new[] {"TestModule1"};
		}

		public void SetModules(ModuleCollection collection) {
			module1 = collection.GetModule<TestModule1>();
		}

		public void Initialize() {
			ServiceLocator.GetService<ModuleFactory>().FullfillRequestNow(this);
		}

		public int GetData() {
			return module1.GetData();
		}
	}
}