namespace ArchGame.Services {
	/// <summary>
	/// The provider for the ILogger service.
	/// If null (as a mock), it will return a NullLogger. Use this if you want to disable logging.
	/// </summary>
	public class LoggerProvider : ServiceProvider<ILogger> {
		/// <summary>
		/// If the logger is not set, a NullLogger will be returned.
		/// </summary>
		protected override ILogger GetMockService() {
			return new NullLogger();
		}
	}
}