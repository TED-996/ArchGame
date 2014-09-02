using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchGame.Services {
	/// <summary>
	/// The ServiceLocator is a class that contains a list of services and makes them globally accesible by type.
	/// Use this feature sparingly, global access is often missused.
	/// </summary>
	public static class ServiceLocator {
		/// <summary>
		/// Get the default providers.
		/// </summary>
		public static IServiceProvider[] DefaultProviders {
			get {
				return new IServiceProvider[] {
					new LoggerProvider()
				};
			}
		}

		static readonly Dictionary<Type, IServiceProvider> Providers;

		/// <summary>
		/// Initializes the static ServiceProvider class.
		/// </summary>
		static ServiceLocator() {
			Providers = new Dictionary<Type, IServiceProvider>();
		}

		/// <summary>
		/// Adds a provider.
		/// </summary>
		public static void AddProvider(IServiceProvider provider) {
			Type providerType = provider.GetServiceType();
			if (Providers.ContainsKey(providerType)) {
				return;
			}
			Providers.Add(providerType, provider);
		}

		/// <summary>
		/// Adds providers from an IEnumerable.
		/// </summary>
		public static void AddProviders(IEnumerable<IServiceProvider> providers) {
			foreach (IServiceProvider provider in providers) {
				AddProvider(provider);
			}
		}

		/// <summary>
		/// Gets a service.
		/// The type must be the same as the one used when registering.
		/// </summary>
		/// <typeparam name="T">The ABSTRACT/INTERFACE (if it exists) type to get.</typeparam>
		/// <returns>The service of the requested type, if the provider exists.</returns>
		public static T GetService<T>() where T : class {
			Type type = typeof(T);
			if (!Providers.ContainsKey(type)) {
				throw new ApplicationException("ServiceProvider not found.");
			}
			IServiceProvider provider = Providers[type];
			return (T) provider.GetService();
		}

		/// <summary>
		/// Sets the service for a provider of the specified type.
		/// </summary>
		/// <typeparam name="T">The type to set. May be exact type or derived.</typeparam>
		/// <param name="service">The service to set.</param>
		public static void SetService<T>(T service) where T : class {
			Type type = typeof(T);
			IServiceProvider provider = Providers.FirstOrDefault(prov => prov.Key.IsAssignableFrom(type)).Value;
			if (provider == null) {
				throw new ApplicationException("ServiceProvider not found.");
			}
			provider.SetService(service);
		}
	}
}