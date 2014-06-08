using System.Collections.Generic;
using Spooker.Content;

namespace SFMLGame {
	public class LoadableSet {
		readonly List<ILoadable> loadables;
		readonly List<ILoadable> preLoadables;

		public LoadableSet(IEnumerable<ILoadable> newLoadables, IEnumerable<ILoadable> newPreloadables) {
			loadables = new List<ILoadable>(newLoadables);
			preLoadables = new List<ILoadable>(newPreloadables);
		}

		public void Load(ContentManager contentManager) {
			foreach (ILoadable loadable in loadables) {
				loadable.LoadContent(contentManager);
			}
		}

		public void Preload(ContentManager contentManager) {
			foreach (ILoadable loadable in preLoadables) {
				loadable.LoadContent(contentManager);
			}
		}
	}
}