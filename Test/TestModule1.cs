using System.Collections.Generic;
using SFMLGame.Modules;

namespace SFMLGameTest {
	public interface ITestModule1 {
		int GetData();
	}

	public class TestModule1 : ITestModule1, IModuleRequester {
		ITestModule2 module2;

		public IEnumerable<string> GetRequestedModules() {
			return new[] {"TestModule2"};
		}

		public void SetModules(ModuleCollection collection) {
			module2 = collection.GetModule<TestModule2>();
		}

		public int GetData() {
			return module2.GetData();
		}
	}
}