using ArchGame.Input;

namespace ArchGame.Components {
	/// <summary>
	/// IArchObstruction is an interface for components that obstruct certain parts of the screen, so that components drawn below it
	/// will not receive mouse clicks made on this component.
	/// Requires IZIndexComponent to get the ZIndex of the obstruction.
	/// </summary>
	public interface IArchObstruction : IZIndexComponent {
		/// <summary>
		/// Get the rectangles that should be obstructed.
		/// </summary>
		void ObstructArea(InputManager inputManager);
	}
}