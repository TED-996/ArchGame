using System;

namespace SFMLGame.Modules {
	public interface IModuleConstructor {
		IModule Construct();

		IModule GetItem();
		Type GetInterfaceType();
	}

	public class ModuleConstructor<TImpl, TInterface> : IModuleConstructor
		where TImpl : TInterface, new()
		where TInterface : class, IModule {

		TImpl item;

		public IModule Construct() {
			item = new TImpl();

			return item;
		}

		public IModule GetItem() {
			return item;
		}

		public Type GetInterfaceType() {
			return typeof (TInterface);
		}
	}
}