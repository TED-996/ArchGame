using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Components {
	public interface IArchDrawable {
		int ZIndex { get; }
		void Draw(SpriteBatch spriteBatch);
	}
}