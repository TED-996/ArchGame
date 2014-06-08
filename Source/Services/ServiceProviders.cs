using SFMLGame.Modules;

namespace SFMLGame.Services {
	public class LoggerProvider : ServiceProvider<ILogger> {
		protected override ILogger GetMockService() {
			return new NullLogger();
		}
	}

	public class FactoryProvider : ServiceProvider<ModuleFactory> {
		protected override ModuleFactory GetMockService() {
			return null;
		}
	}
}