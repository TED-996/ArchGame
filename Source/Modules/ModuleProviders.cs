namespace SFMLGame.Modules {
	public class GenericModuleProvider<T> : ModuleProvider<T> where T : class {
		protected override T GetMockModule() {
			return null;
		}
	}
}