using Microsoft.Xna.Framework;

namespace ArchGame.Input {
	/// <summary>
	/// IObstructionManager is an interface for a class that manages a list of obstructions.
	/// It is currently implemented only by the InputManager.
	/// </summary>
	public interface IObstructionManager {
		/// <summary>
		/// Adds a rectangle to the obstruction list.
		/// </summary>
		/// <param name="rectangle">The rectangle to be obstructed</param>
		/// <param name="zIndex">The ZIndex of the rectangle</param>
		void ObstructArea(Rectangle rectangle, int zIndex);
	}
}