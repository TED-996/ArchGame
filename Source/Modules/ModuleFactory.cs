using System;
using System.Collections.Generic;
using System.Linq;
using Spooker.Content;

namespace SFMLGame.Modules {
	/// <summary>
	/// Responsible for building the modules.
	/// </summary>
	public class ModuleFactory : IDisposable, ILoadable {
		readonly ModuleCollection moduleCollection;
		readonly List<IModuleRequester> requesters;

		readonly List<IModuleConstructor> constructors;
		readonly List<Object> modules;

		readonly List<ILoadable> loadables; 
		readonly List<IDisposable> disposables; 
		
		internal ModuleFactory() {
			moduleCollection = new ModuleCollection();
			requesters = new List<IModuleRequester>();

			constructors = new List<IModuleConstructor>();

			modules = new List<Object>();
			moduleCollection.AddProviders(GetDefaultProviders());

			disposables = new List<IDisposable>();
			loadables = new List<ILoadable>();
		}

		IEnumerable<IModuleProvider> GetDefaultProviders() {
			return new List<IModuleProvider>();
		}

		/// <summary>
		/// Register a module provider.
		/// </summary>
		/// <param name="provider">The provider to be registered.</param>
		public void RegisterProvider(IModuleProvider provider) {
			moduleCollection.AddProvider(provider);
		}

		/// <summary>
		/// Registers a provider with a module already set.
		/// The module must not be of type Object, this will break lookups. Use a more derived type or interface.
		/// </summary>
		/// <typeparam name="T">The module type.</typeparam>
		/// <param name="module">The module to be set.</param>
		public void RegisterProvider<T>(T module) where T : class {
			if (typeof (T) == typeof (Object)) {
				throw new ArgumentException("Module must not be of type Object, this will break lookups." +
					"Use a more specific type or interface.");
			}
			moduleCollection.AddProvider(new GenericModuleProvider<T>(module));
		}

		/// <summary>
		/// Register a module requester. The request will be fullfilled after the content has been loaded.
		/// It you want to fullfill the request immediately, use FullfillRequestNow(IModuleRequester).
		/// </summary>
		/// <param name="requester">An object that implements IModuleRequester.</param>
		public void RegisterRequester(IModuleRequester requester) {
			requesters.Add(requester);
		}

		/// <summary>
		/// Gets a constructed module.
		/// </summary>
		/// <typeparam name="T">The type of the module. If an interface type was registered, this must be the interface type too.
		/// (e.g. IModule1)</typeparam>
		/// <returns>The module that was constructed.</returns>
		public T GetModule<T>() where T : class {
			if (typeof(T) == typeof(Object)) {
				throw new ArgumentException("Module must not be of type Object, this will break lookups." +
					"Use a more specific type or interface.");
			}
			return moduleCollection.GetModule<T>();
		}

		/// <summary>
		/// Builds a collection of modules.
		/// </summary>
		/// <param name="providers">An array of strings, containing the name (without namespace) of the types to request.</param>
		/// <returns>A ModuleCollection that contains the requested modules.</returns>
		ModuleCollection BuildCollection(params string[] providers) {
			ModuleCollection collection = new ModuleCollection();
			collection.AddProviders(providers.Select(typeName => moduleCollection.GetProvider(typeName)));
			return collection;
		}

		/// <summary>
		/// Register a constructor.
		/// </summary>
		/// <param name="constructor">The constructor to be registered.</param>
		public void RegisterConstructor(IModuleConstructor constructor) {
			constructors.Add(constructor);
		}

		/// <summary>
		/// Register a constructor.
		/// </summary>
		/// <typeparam name="TImpl">The implementation type. (e.g Module1)</typeparam>
		/// <typeparam name="TInterface">The interface type for the module (NOT Object) (e.g. IModule1)</typeparam>
		public void RegisterConstructor<TImpl, TInterface>() where TImpl : TInterface, new() where TInterface : class {
			RegisterConstructor(new ModuleConstructor<TImpl, TInterface>());
		}

		/// <summary>
		/// Regster a module.
		/// </summary>
		/// <typeparam name="TImpl">The implementation type. (e.g Module1)</typeparam>
		/// <typeparam name="TInterface">The interface type for the module (NOT Object) (e.g. IModule1)</typeparam>
		public void RegisterModule<TImpl, TInterface>() where TImpl : TInterface, new() where TInterface : class {
			RegisterConstructor<TImpl, TInterface>();
			moduleCollection.AddProvider(new GenericModuleProvider<TInterface>());
		}

		/// <summary>
		/// Register a module.
		/// </summary>
		/// <typeparam name="TImpl">The module type (e.g. Module1)</typeparam>
		public void RegisterModule<TImpl>() where TImpl : class, new() {
			RegisterModule<TImpl, TImpl>();
		}

		/// <summary>
		/// Fullfill the IModuleRequester requests. Called by Game, only once, after the content has been loaded and the
		/// modules have been constructed.
		/// </summary>
		internal void FullfillRequests() {
			foreach (IModuleRequester requester in requesters) {
				requester.SetModules(BuildCollection(requester.GetRequestedModules().ToArray()));
			}
		}

		/// <summary>
		/// Fullfills a request immediately.
		/// </summary>
		/// <param name="requester">The requester to receive the dependencies.</param>
		public void FullfillRequestNow(IModuleRequester requester) {
			requester.SetModules(BuildCollection(requester.GetRequestedModules().ToArray()));
		}

		/// <summary>
		/// Constructs the modules and adds them as requesters if necessary. Called by Game, only once, after the content has
		/// been loaded.
		/// </summary>
		internal void ConstructModules() {
			foreach (IModuleConstructor constructor in constructors) {
				Object module = constructor.Construct();

				modules.Add(module);
				if (module is IModuleRequester) {
					requesters.Add(module as IModuleRequester);
				}
				if (module is IDisposable) {
					disposables.Add(module as IDisposable);
				}
				if (module is ILoadable) {
					loadables.Add(module as ILoadable);
				}

				try {
					moduleCollection.SetModule(module, constructor.GetInterfaceType());
				}
				catch(InvalidOperationException) {
					//Provider not set. Ignore.
				}
			}
		}

		/// <summary>
		/// Calls dispose on the modules that implement IDisposable.
		/// </summary>
		public void Dispose() {
			foreach (IDisposable disposable in disposables) {
				disposable.Dispose();
			}
		}

		/// <summary>
		/// Calls LoadContent on the modules that implement ILoadable.
		/// </summary>
		/// <param name="content"></param>
		public void LoadContent(ContentManager content) {
			foreach (ILoadable loadable in loadables) {
				loadable.LoadContent(content);
			}
		}
	}
}