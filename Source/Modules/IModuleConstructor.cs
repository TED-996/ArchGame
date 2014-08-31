using System;

namespace ArchGame.Modules {
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

	public class LambdaConstructor<TImpl, TInterface> : IModuleConstructor
		where TImpl : class, TInterface
		where TInterface : class {

		readonly Func<TImpl> constructorFunction;
		TImpl item;

		public LambdaConstructor(Func<TImpl> newConstructorFunction) {
			constructorFunction = newConstructorFunction;
		}

		public Object Construct() {
			item = constructorFunction();
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