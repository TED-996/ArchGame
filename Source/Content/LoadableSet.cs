using System.Collections.Generic;
using ArchGame.Components;
using Microsoft.Xna.Framework.Content;

namespace ArchGame.Content {
	/// <summary>
	/// The LoadableSet is a class that keeps 2 lists of IArchLoadables to be used in the content loading phase.
	/// The preloadables will be loaded before the window appears and is intended for content critical for the game at any given moment.
	/// The loadables will be loaded while the loading state is being shown on screen, in a background thread.
	/// It is reccommended to load all the content of the game in one of those lists, and to have the preloadables as few as possible.
	/// See the wiki for more information.
	/// </summary>
	public class LoadableSet {
		readonly List<IArchLoadable> loadables;
		readonly List<IArchLoadable> preLoadables;

		/// <summary>
		/// Initializes a new instance of type LoadableSet.
		/// </summary>
		/// <param name="newLoadables">The IArchLoadables to be loaded in the background thread</param>
		/// <param name="newPreloadables">The IArchLoadables to be loaded befroe the window appears</param>
		public LoadableSet(IEnumerable<IArchLoadable> newLoadables, IEnumerable<IArchLoadable> newPreloadables) {
			loadables = new List<IArchLoadable>(newLoadables);
			preLoadables = new List<IArchLoadable>(newPreloadables);
		}

		/// <summary>
		/// Load the loadables (in the background thread)
		/// </summary>
		/// <param name="contentManager">The ContentManager to load content from</param>
		public void Load(ContentManager contentManager) {
			foreach (IArchLoadable loadable in loadables) {
				loadable.LoadContent(contentManager);
			}
		}

		/// <summary>
		/// Load the preloadables (before the window appears)
		/// </summary>
		/// <param name="contentManager">The ContentManager to load content with</param>
		public void Preload(ContentManager contentManager) {
			foreach (IArchLoadable loadable in preLoadables) {
				loadable.LoadContent(contentManager);
			}
			
		}

		/// <summary>
		/// Discards the content so it can be garbage collected to free memory.
		/// A copy is always kept inside XNA's ContentManager cache, and will be used later in the classes where the content is needed.
		/// </summary>
		public void Discard() {
			loadables.Clear();
			preLoadables.Clear();
		}
	}
}