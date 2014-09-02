using Microsoft.Xna.Framework.Content;

namespace ArchGame.Components {
	/// <summary>
	/// Wrap content inside an IArchLoadable so that it can be loaded.
	/// Even if this class's lifetime ends (it is garbage collected), most content will remain inside XNA's ContentManager cache.
	/// This is intended to be passed to the LoadableSet (for the caching of content) or to the ComponentList (for later usage),
	/// but can be used for other purposes.
	/// The content will be loaded by the ContentManager. Make sure the content exists and that you are loading the correct type.
	/// </summary>
	/// <typeparam name="T">The type of the content</typeparam>
	public class ContentToIArchLoadable<T> : IArchLoadable {
		readonly string filename;

		/// <summary>
		/// The loaded item. This should be accesed only after the content has been loaded.
		/// </summary>
		public T Item { get; private set; }

		/// <summary>
		/// Initialize a new instance of type ContentToIArchLoadable.
		/// </summary>
		/// <param name="newFilename">The filename of the content you wish to load</param>
		public ContentToIArchLoadable(string newFilename) {
			filename = newFilename;
		}

		/// <summary>
		/// Load the item.
		/// </summary>
		/// <param name="contentManager">The ContentManager</param>
		public void LoadContent(ContentManager contentManager) {
			Item = contentManager.Load<T>(filename);
		}
	}
}