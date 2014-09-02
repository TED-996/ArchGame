using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchGame.Modules {
	/// <summary>
	/// The ModuleCollection is a class that contains a list of modules and allows their retrieval by type.
	/// </summary>
	public class ModuleCollection {
		readonly Dictionary<TypeStringPair, IModuleProvider> providers;

		readonly Dictionary<Type, TypeStringPair> typeToPair;
		readonly Dictionary<string, TypeStringPair> stringToPair; 

		/// <summary>
		/// Instantiates a new ModuleCollection
		/// </summary>
		internal ModuleCollection() {
			providers = new Dictionary<TypeStringPair, IModuleProvider>();

			typeToPair = new Dictionary<Type, TypeStringPair>();
			stringToPair = new Dictionary<string, TypeStringPair>();
		}

		/// <summary>
		/// Adds a provider.
		/// </summary>
		internal void AddProvider(IModuleProvider provider) {
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
		internal void AddProviders(IEnumerable<IModuleProvider> newProviders) {
			foreach (IModuleProvider provider in newProviders) {
				AddProvider(provider);
			}
		}

		/// <summary>
		/// Gets a module.
		/// The type must be the same type as the one used when registering.
		/// </summary>
		/// <typeparam name="T">The ABSTRACT/INTERFACE (if it exists) type to get.</typeparam>
		/// <returns>The module of the requested type, if the provider exists.</returns>
		public T GetModule<T>() where T : class {
			Type type = typeof (T);
			if (!typeToPair.ContainsKey(type)) {
				throw new InvalidOperationException("ModuleProvider not found.");
			}
			IModuleProvider provider = providers[typeToPair[type]];
			return (T) provider.GetModule();
		}

		/// <summary>
		/// Sets the module for a provider of the specified type.
		/// </summary>
		/// <typeparam name="T">The type to set. May be exact type or derived.</typeparam>
		/// <param name="module">The module to set.</param>
		internal void SetModule<T>(T module) where T : class {
			SetModule(module, typeof (T));
		}

		/// <summary>
		/// Sets the module for a provider of the specified type.
		/// Use this if module is Object, with type being the interface/implementation type.
		/// </summary>
		/// <param name="module">The module to set.</param>
		/// <param name="type">The type of the module.</param>
		internal void SetModule(Object module, Type type) {
			if (!typeToPair.ContainsKey(type)) {
				throw new InvalidOperationException("ModuleProvider not found.");
			}
			IModuleProvider provider = providers.FirstOrDefault(prov => prov.Key.Type.IsAssignableFrom(type)).Value;
			provider.SetModule(module);
		}

		/// <summary>
		/// Gets a provider from type.
		/// TODO: Maybe delete?
		/// </summary>
		/// <typeparam name="T">The type that the provider provides.</typeparam>
		internal IModuleProvider GetProvider<T>() where T : class {
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
		internal IModuleProvider GetProvider(string typeName) {
			if (!stringToPair.ContainsKey(typeName)) {
				throw new InvalidOperationException("ModuleProvider not found.");
			}

			return providers[stringToPair[typeName]];
		}

		/// <summary>
		/// Returns true if there exists a provider with the specified type.
		/// </summary>
		/// <param name="providerType">The type of the provider to search</param>
		/// <returns>True if the provider exists, false otherwise.</returns>
		internal bool DoesProviderExist(Type providerType) {
			return typeToPair.ContainsKey(providerType);
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