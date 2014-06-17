using System;

namespace SFMLGame.Modules {
	public interface IModuleConstructor {
		Object Construct();

		Object GetItem();
		Type GetInterfaceType();
	}

	public class ModuleConstructor<TImpl, TInterface> : IModuleConstructor
		where TImpl : TInterface, new()
		where TInterface : class {

		TImpl item;

		public Object Construct() {
			item = new TImpl();

			return item;
		}

		public Object GetItem() {
			return item;
		}

		public Type GetInterfaceType() {
			return typeof (TInterface);
		}
	}
}