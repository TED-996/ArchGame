using Microsoft.Xna.Framework;

namespace ArchGame.Components {
	/// <summary>
	/// IArchUpdateable is an interface for components that should be updated.
	/// </summary>
	public interface IArchUpdateable {
		/// <summary>
		/// The UpdatePriority is a number that represents the order in which components should be updated.
		/// An IArchUpdateable with a higher UpdatePriority will be updated before an IArchUpdateable with a lower UpdatePriority.
		/// Take note that this is reverse to IArchDrawable.ZIndex.
		/// </summary>
		int UpdatePriority { get; }

		/// <summary>
		/// Update the IArchUpdateable.
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		void Update(GameTime gameTime);
	}
}