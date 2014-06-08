using System;

namespace SFMLGame.Modules {
	public interface IModuleProvider {
		Object GetModule();
		void SetModule(Object newModule);
		Type GetModuleType();
	}

	public abstract class ModuleProvider<T> : IModuleProvider where T : class, IModule {
		T module;

		protected ModuleProvider() {
			module = null;
		}

		public object GetModule() {
			return module ?? GetMockModule();
		}

		protected abstract T GetMockModule();

		public void SetModule(object newModule) {
			module = (T) newModule;
		}

		public Type GetModuleType() {
			return typeof(T);
		}
	}
}