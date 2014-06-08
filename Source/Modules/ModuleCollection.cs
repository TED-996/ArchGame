using System;
using System.Collections.Generic;
using System.Linq;

namespace SFMLGame.Modules {
	public class ModuleCollection {
		readonly Dictionary<TypeStringPair, IModuleProvider> providers;

		readonly Dictionary<Type, TypeStringPair> typeToPair;
		readonly Dictionary<string, TypeStringPair> stringToPair; 

		/// <summary>
		/// Instantiates a new ModuleLocator
		/// </summary>
		public ModuleCollection() {
			providers = new Dictionary<TypeStringPair, IModuleProvider>();

			typeToPair = new Dictionary<Type, TypeStringPair>();
			stringToPair = new Dictionary<string, TypeStringPair>();
		}

		/// <summary>
		/// Adds a provider.
		/// </summary>
		public void AddProvider(IModuleProvider provider) {
			Type providerType = provider.GetModuleType();
			if (typeToPair.ContainsKey(providerType)) {
				return;
			}
			TypeStringPair pair = new TypeStringPair(providerType);
			providers.Add(pair, provider);

			typeToPair.Add(pair.Type, pair);
			stringToPair.Add(pair.String, pair);
		}

		/// <summary>
		/// Adds providers.
		/// </summary>
		public void AddProviders(IEnumerable<IModuleProvider> newProviders) {
			foreach (IModuleProvider provider in newProviders) {
				AddProvider(provider);
			}
		}

		/// <summary>
		/// Gets a module.
		/// </summary>
		/// <typeparam name="T">The ABSTRACT/INTERFACE type to get.</typeparam>
		/// <returns>The module of the requested type, if the provider exists.</returns>
		public T GetModule<T>() where T : class, IModule {
			Type type = typeof (T);
			if (!typeToPair.ContainsKey(type)) {
				throw new InvalidOperationException("ModuleProvider not found.");
			}
			IModuleProvider provider = providers[typeToPair[typeof (T)]];
			return (T) provider.GetModule();
		}

		/// <summary>
		/// Sets the module for a provider of the specified type.
		/// </summary>
		/// <typeparam name="T">The type to set. May be exact type or derived.</typeparam>
		/// <param name="module">The module to set.</param>
		public void SetModule<T>(T module) where T : class, IModule {
			SetModule(module, typeof (T));
		}

		/// <summary>
		/// Sets the module for a provider of the specified type.
		/// Use this if module is IModule, with type being the interface/implementation type.
		/// </summary>
		/// <param name="module">The module to set.</param>
		/// <param name="type">The type of the module.</param>
		public void SetModule(IModule module, Type type) {
			if (!typeToPair.ContainsKey(type)) {
				throw new InvalidOperationException("ModuleProvider not found.");
			}
			IModuleProvider provider = providers.FirstOrDefault(prov => prov.Key.Type.IsAssignableFrom(type)).Value;
			provider.SetModule(module);
		}

		/// <summary>
		/// Gets a provider from type.
		/// </summary>
		/// <typeparam name="T">The type that the provider provides.</typeparam>
		/// <returns></returns>
		internal IModuleProvider GetProvider<T>() where T : class, IModule {
			Type type = typeof(T);
			if (!typeToPair.ContainsKey(type)) {
				throw new InvalidOperationException("ModuleProvider not found.");
			}
			return providers[typeToPair[typeof(T)]];
		}

		/// <summary>
		/// Gets a provider from type name.
		/// </summary>
		/// <param name="typeName">The name of the type that the provider provides.</param>
		/// <returns></returns>
		internal IModuleProvider GetProvider(string typeName) {
			if (!stringToPair.ContainsKey(typeName)) {
				throw new InvalidOperationException("ModuleProvider not found.");
			}

			return providers[stringToPair[typeName]];
		}
	}

	class TypeStringPair {
		public Type Type { get; private set; }
		public string String { get; private set; }
		
		public TypeStringPair(Type newType) {
			Type = newType;
			String = Type.Name;
		}
	}
}