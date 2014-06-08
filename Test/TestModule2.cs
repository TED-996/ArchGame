using System.Collections.Generic;
using SFMLGame.Modules;

namespace SFMLGameTest {
	public interface ITestModule2 : IModule {
		int GetData();
	}

	public class TestModule2 : ITestModule2 {
		int data;

		public TestModule2() {
			data = 100;
		}

		public int GetData() {
			return data;
		}
	}
}