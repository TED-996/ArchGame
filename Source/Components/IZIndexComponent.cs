namespace ArchGame.Components {
	/// <summary>
	/// IZIndexComponent is an interface for components that follow an order in which they are drawn on the screen.
	/// </summary>
	public interface IZIndexComponent {
		/// <summary>
		/// The ZIndex is a number that represents the order in which components should be drawn on the screen.
		/// A component with a lower ZIndex will be drawn before, and therefore behind components with a higher ZIndex.
		/// </summary>
		int ZIndex { get; }
	}
}