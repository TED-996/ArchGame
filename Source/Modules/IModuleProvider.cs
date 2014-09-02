using System;

namespace ArchGame.Modules {
	/// <summary>
	/// An IModuleProvider is a class that keeps a module in memory, along with its type. It is used in the ModuleCollection.
	/// </summary>
	public interface IModuleProvider {
		/// <summary>
		/// Gets the module in the provider.
		/// </summary>
		Object GetModule();

		/// <summary>
		/// Sets the module in the provider.
		/// </summary>
		void SetModule(Object newModule);

		/// <summary>
		/// Gets the type of the module
		/// </summary>
		Type GetModuleType();
	}

	/// <summary>
	/// Abstract generic class, implementing IModuleProvider.
	/// The mock module is used instead when the module is null / not set. It is used to facilitate unit testing.
	/// </summary>
	/// <typeparam name="T">The type of the module or of its interface</typeparam>
	public abstract class ModuleProvider<T> : IModuleProvider where T : class {
		T module;

		/// <summary>
		/// Initializes the base class of a ModuleProvider implementation.
		/// </summary>
		protected ModuleProvider() {
			module = null;
		}

		/// <summary>
		/// Sets the module in the provider.
		/// </summary>
		protected ModuleProvider(T item) {
			module = item;
		} 

		/// <summary>
		/// Gets the module in the provider.
		/// </summary>
		public object GetModule() {
			return module ?? GetMockModule();
		}

		/// <summary>
		/// Get the mock module of the provider. The mock module is used when the module is not set or is null, facilitating unit testing.
		/// </summary>
		protected abstract T GetMockModule();

		/// <summary>
		/// Sets the module in the provider.
		/// </summary>
		public void SetModule(object newModule) {
			module = (T) newModule;
		}

		/// <summary>
		/// Gets the module type.
		/// </summary>
		public Type GetModuleType() {
			return typeof(T);
		}
	}
}