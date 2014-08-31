using System;

namespace ArchGame.Modules {
	public interface IModuleProvider {
		Object GetModule();
		void SetModule(Object newModule);
		Type GetModuleType();
	}

	public abstract class ModuleProvider<T> : IModuleProvider where T : class {
		T module;

		protected ModuleProvider() {
			module = null;
		}

		protected ModuleProvider(T item) {
			module = item;
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