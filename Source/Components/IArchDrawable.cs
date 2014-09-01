using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Components {
	/// <summary>
	/// IArchDrawable is an interface for components that should draw objects on the screen.
	/// </summary>
	public interface IArchDrawable {
		/// <summary>
		/// The ZIndex is a number that represents the order in which components should be drawn on the screen.
		/// An IArchDrawable with a lower ZIndex will be drawn before, and therefore behind IArchDrawables with a higher ZIndex.
		/// </summary>
		int ZIndex { get; }

		/// <summary>
		/// Draw the IArchDrawable.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		void Draw(SpriteBatch spriteBatch);
	}
}