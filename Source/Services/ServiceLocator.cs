using System;
using System.Collections.Generic;
using System.Linq;

namespace SFMLGame.Services {
	public static class ServiceLocator {
		public static IServiceProvider[] DefaultProviders {
			get {
				return new IServiceProvider[] {
					new LoggerProvider(),
					new FactoryProvider()
				};
			}
		}

		
		static readonly Dictionary<Type, IServiceProvider> Providers;

		/// <summary>
		/// Instantiates a new ServiceLocator
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
		/// Adds providers.
		/// </summary>
		public static void AddProviders(IEnumerable<IServiceProvider> providers) {
			foreach (IServiceProvider provider in providers) {
				AddProvider(provider);
			}
		}

		/// <summary>
		/// Gets a service.
		/// </summary>
		/// <typeparam name="T">The ABSTRACT/INTERFACE type to get.</typeparam>
		/// <returns>The service of the requested type, if the provider exists.</returns>
		public static T GetService<T>() where T : class {
			IServiceProvider provider = Providers[typeof (T)];
			if (provider == null) {
				throw new ApplicationException("ServiceProvider not found.");
			}
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