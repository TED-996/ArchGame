using System;

namespace ArchGame.Services {
	public interface IServiceProvider {
		Object GetService();
		void SetService(Object newService);
		Type GetServiceType();
	}

	public abstract class ServiceProvider<T> : IServiceProvider where T : class {
		T service;

		protected ServiceProvider() {
			service = null;
		} 

		public object GetService() {
			return service ?? GetMockService();
		}

		protected abstract T GetMockService();

		public void SetService(object newService) {
			service = (T) newService;
		}

		public Type GetServiceType() {
			return typeof (T);
		}
	}
}