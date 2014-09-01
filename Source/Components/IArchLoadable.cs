using Microsoft.Xna.Framework.Content;

namespace ArchGame.Components {
	/// <summary>
	/// IArchLoadable is an interface for components that should load content (from files, especially from the Content project)
	/// at the LoadContent stage.
	/// </summary>
	public interface IArchLoadable {
		/// <summary>
		/// Load the content of the IArchLoadable.
		/// </summary>
		/// <param name="contentManager">The ContentManager to load from</param>
		void LoadContent(ContentManager contentManager);
	}
}