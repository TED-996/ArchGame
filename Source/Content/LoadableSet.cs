using System.Collections.Generic;
using ArchGame.Components;
using Microsoft.Xna.Framework.Content;

namespace ArchGame.Content {
	/// <summary>
	/// The LoadableSet is a class that keeps a list of IArchLoadables to be used in the content loading phase.
	/// The assets will be loaded while the loading state is being shown on screen, in a background thread.
	/// It is reccommended to load all the content of the game in this list.
	/// See the wiki for more information.
	/// </summary>
	public class LoadableSet : List<IArchLoadable> {
		/// <summary>
		/// Initializes a new instance of type LoadableSet.
		/// </summary>
		/// <param name="loadables">The IArchLoadables to be loaded in the background thread</param>
		public LoadableSet(IEnumerable<IArchLoadable> loadables) : base(loadables) {
		}

		/// <summary>
		/// Add a ContentManager asset from a type and a filename.
		/// </summary>
		/// <typeparam name="T">The type of the asset</typeparam>
		/// <param name="filename">The filename of the asset</param>
		public void Add<T>(string filename) {
			Add(new AssetAsILoadable<T>(filename));
		}

		/// <summary>
		/// Load the loadables (in the background thread).
		/// </summary>
		/// <param name="contentManager">The ContentManager to load content from</param>
		public void Load(ContentManager contentManager) {
			foreach (IArchLoadable loadable in this) {
				loadable.LoadContent(contentManager);
			}
		}

		/// <summary>
		/// Discards the content so it can be garbage collected to free memory.
		/// A copy is always kept inside XNA's ContentManager cache, and will be used later in the classes where the content is needed.
		/// </summary>
		public void Discard() {
			Clear();
		}
	}
}