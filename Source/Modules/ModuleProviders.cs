namespace SFMLGame.Modules {
	public class GenericModuleProvider<T> : ModuleProvider<T> where T : class {
		public GenericModuleProvider(){
			
		} 

		public GenericModuleProvider(T item) : base(item) {
			
		} 

		protected override T GetMockModule() {
			return null;
		}
	}
}