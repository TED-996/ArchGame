using SFMLGame;
using SFMLGame.Services;
using Spooker.Content;

namespace SFMLGameTest {
	public class TestGame : Game {
		readonly XMLTester tester;
		IrregularRequester1 irregularRequester1;
		public TestGame() : base("Elementalist", 60, 1024, 768, "Files",
			new LoadableSet(new ILoadable[0], new ILoadable[0]), new ThreadedLogger()) {
			tester = new XMLTester();

			moduleFactory.RegisterModule<TestModule1>();
			moduleFactory.RegisterModule<TestModule2>();

			irregularRequester1 = new IrregularRequester1();
		}

		protected override void SetServiceProviders() {
			base.SetServiceProviders();
			ServiceLocator.SetService(logger);
		}

		public override void LoadContent(ContentManager content) {
			base.LoadContent(content);

			tester.LoadFromFile();
			
			logger.Log(tester.ToString(), "Game");
		}

		protected override void AfterLoadContent() {
		}
	}
}