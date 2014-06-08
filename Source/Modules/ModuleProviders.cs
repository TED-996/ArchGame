namespace SFMLGame.Modules {
	public class GenericModuleProvider<T> : ModuleProvider<T> where T : class, IModule {
		protected override T GetMockModule() {
			return null;
		}
	}
}