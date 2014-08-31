using Microsoft.Xna.Framework.Content;

namespace ArchGame.Components {
	public interface IArchLoadable {
		void LoadContent(ContentManager contentManager);
	}
}