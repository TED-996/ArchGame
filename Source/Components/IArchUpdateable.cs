using Microsoft.Xna.Framework;

namespace ArchGame.Components {
	public interface IArchUpdateable {
		int UpdatePriority { get; }
		void Update(GameTime gameTime);
	}
}