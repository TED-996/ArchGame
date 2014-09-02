using System;

namespace ArchGame.Services {
	/// <summary>
	/// An IModuleProvider is a class that keeps a service in memory, along with its type. It is used in the ServiceLocator
	/// </summary>
	public interface IServiceProvider {
		/// <summary>
		/// Get the service in the provider.
		/// </summary>
		Object GetService();

		/// <summary>
		/// Set the service in the provider.
		/// </summary>
		void SetService(Object newService);

		/// <summary>
		/// Get the type of the service.
		/// </summary>
		Type GetServiceType();
	}

	/// <summary>
	/// Abstract generic class, implementing IServiceProvider.
	/// The mock service is used instead when the service is null / not set. It is used to facilitate unit testing.
	/// </summary>
	/// <typeparam name="T">The type of the module or of its interface</typeparam>
	public abstract class ServiceProvider<T> : IServiceProvider where T : class {
		T service;

		/// <summary>
		/// Initialize the base class of a ServiceProvider implementation.
		/// </summary>
		protected ServiceProvider() {
			service = null;
		} 

		/// <summary>
		/// Get the service in the provider.
		/// </summary>
		public object GetService() {
			return service ?? GetMockService();
		}

		/// <summary>
		/// Get the mock service of the provider. The mock service is used when the service is not set or null, facilitating unit testing.
		/// </summary>
		protected abstract T GetMockService();

		/// <summary>
		/// Set the service in the provider.
		/// </summary>
		public void SetService(object newService) {
			service = (T) newService;
		}

		/// <summary>
		/// Get the type of the service.
		/// </summary>
		public Type GetServiceType() {
			return typeof (T);
		}
	}
}