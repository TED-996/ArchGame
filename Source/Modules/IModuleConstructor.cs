using System;

namespace ArchGame.Modules {
	/// <summary>
	/// An IModuleConstructor is a class that constructs and returns an instance of a specific type.
	/// Can expose a less derived type (usually an interface) to outside code for loose coupling.
	/// </summary>
	public interface IModuleConstructor {
		/// <summary>
		/// Constructs and returns the module.
		/// </summary>
		Object Construct();

		/// <summary>
		/// Gets the constructed module.
		/// </summary>
		Object GetItem();

		/// <summary>
		/// Gets the interface type.
		/// </summary>
		Type GetInterfaceType();
	}

	/// <summary>
	/// Generic module constructor.
	/// The constructed module must have a parameterless constructor; use LambdaConstructor if this is not the case.
	/// </summary>
	/// <typeparam name="TImpl">The type of the constructed module</typeparam>
	/// <typeparam name="TInterface">The type of the interface of the constructed module</typeparam>
	public class ModuleConstructor<TImpl, TInterface> : IModuleConstructor
		where TImpl : TInterface, new()
		where TInterface : class {

		TImpl item;

		/// <summary>
		/// Constructs and returns the module. 
		/// </summary>
		public Object Construct() {
			item = new TImpl();

			return item;
		}

		/// <summary>
		/// Gets the module.
		/// </summary>
		public Object GetItem() {
			return item;
		}

		/// <summary>
		/// Gets the type of TInterface.
		/// </summary>
		public Type GetInterfaceType() {
			return typeof (TInterface);
		}
	}

	/// <summary>
	/// Generic module constructor for module types without a parameterless constructor.
	/// </summary>
	/// <typeparam name="TImpl">The type of the constructed module</typeparam>
	/// <typeparam name="TInterface">The type of the interface of the constructed module</typeparam>
	public class LambdaConstructor<TImpl, TInterface> : IModuleConstructor
		where TImpl : class, TInterface
		where TInterface : class {

		readonly Func<TImpl> constructorFunction;
		TImpl item;

		/// <summary>
		/// Initialize a new instance of type LambdaConstructor.
		/// </summary>
		/// <param name="newConstructorFunction">The function to use when constructing the module.</param>
		public LambdaConstructor(Func<TImpl> newConstructorFunction) {
			constructorFunction = newConstructorFunction;
		}

		/// <summary>
		/// Constructs and returns the module. 
		/// </summary>
		public Object Construct() {
			item = constructorFunction();
			return item;
		}

		/// <summary>
		/// Gets the module.
		/// </summary>
		public Object GetItem() {
			return item;
		}

		/// <summary>
		/// Gets the type of TInterface.
		/// </summary>
		public Type GetInterfaceType() {
			return typeof (TInterface);
		}
	}
}