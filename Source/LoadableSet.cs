using System.Collections.Generic;
using ArchGame.Components;
using Microsoft.Xna.Framework.Content;

namespace ArchGame {
	public class LoadableSet {
		readonly List<IArchLoadable> loadables;
		readonly List<IArchLoadable> preLoadables;

		public LoadableSet(IEnumerable<IArchLoadable> newLoadables, IEnumerable<IArchLoadable> newPreloadables) {
			loadables = new List<IArchLoadable>(newLoadables);
			preLoadables = new List<IArchLoadable>(newPreloadables);
		}

		public void Load(ContentManager contentManager) {
			foreach (IArchLoadable loadable in loadables) {
				loadable.LoadContent(contentManager);
			}
		}

		public void Preload(ContentManager contentManager) {
			foreach (IArchLoadable loadable in preLoadables) {
				loadable.LoadContent(contentManager);
			}
		}
	}
}