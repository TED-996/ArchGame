using System.Collections.Generic;
using ArchGame.Components;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace ArchGame.Content {
	/// <summary>
	/// Wraps a type and a filename into an IArchLoadable.
	/// Use it to add content to the LoadableSet.
	/// You may also use it as a component in the ComponentList.
	/// </summary>
	/// <typeparam name="T">The type of the asset</typeparam>
	public class AssetAsIArchLoadable<T> : IArchLoadable {
		readonly string filename;

		/// <summary>
		/// Get the asset that was loaded.
		/// </summary>
		public T Asset { get; private set; }

		/// <summary>
		/// Initialize a new object of type AssetAsIArchLoadable
		/// </summary>
		/// <param name="newFilename">The filename of the asset</param>
		public AssetAsIArchLoadable(string newFilename) {
			filename = newFilename;
		}

		/// <summary>
		/// Load the asset.
		/// </summary>
		/// <param name="contentManager">The ContentManager to load the asset from</param>
		public void LoadContent(ContentManager contentManager) {
			Asset = contentManager.Load<T>(filename);
		}
	}

	/// <summary>
	/// Wraps a type and multiple filenames into an IArchLoadable.
	/// Use it to add content to the LoadableSet or to load assets in bulk.
	/// </summary>
	/// <typeparam name="T">The type of the assets</typeparam>
	public class MultipleAssetsAsIArchLoadable<T> : IArchLoadable {
		readonly string[] filenames;

		/// <summary>
		/// Get the assets that were loaded.
		/// </summary>
		public List<T> Assets { get; private set; }

		/// <summary>
		/// Initialize a new object of type MultipleAssetsAsIArchLoadable
		/// </summary>
		/// <param name="newFilenames">The filenames of the assets</param>
		public MultipleAssetsAsIArchLoadable(params string[] newFilenames) {
			filenames = newFilenames;
			Assets = null;
		}

		/// <summary>
		/// Load the assets.
		/// </summary>
		/// <param name="contentManager">The ContentManager to load the assets from</param>
		public void LoadContent(ContentManager contentManager) {
			Assets = filenames.Select(contentManager.Load<T>).ToList();
		}
	}
}