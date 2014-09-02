namespace ArchGame.Modules {
	/// <summary>
	/// Generic module provider. Use only if you do not need a mock module.
	/// </summary>
	public class GenericModuleProvider<T> : ModuleProvider<T> where T : class {
		/// <summary>
		/// Initialize a new instance of type GenericModuleProvider.
		/// </summary>
		public GenericModuleProvider(){
			
		} 

		/// <summary>
		/// Initialize new instance of type GenericModuleProvider and set its module.
		/// </summary>
		/// <param name="item">The module to set in the provider</param>
		public GenericModuleProvider(T item) : base(item) {
			
		} 

		/// <summary>
		/// Get the mock module. Returns null.
		/// </summary>
		protected override T GetMockModule() {
			return null;
		}
	}
}