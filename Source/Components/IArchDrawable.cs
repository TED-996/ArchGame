using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Components {
	/// <summary>
	/// IArchDrawable is an interface for components that should draw objects on the screen.
	/// Requires IZIndexComponent to get the order in which the components should be drawn.
	/// </summary>
	public interface IArchDrawable : IZIndexComponent {
		/// <summary>
		/// Draw the IArchDrawable.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		void Draw(SpriteBatch spriteBatch);
	}
}