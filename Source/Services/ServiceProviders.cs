using ArchGame.Modules;

namespace ArchGame.Services {
	public class LoggerProvider : ServiceProvider<ILogger> {
		protected override ILogger GetMockService() {
			return new NullLogger();
		}
	}
}