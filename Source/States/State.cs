using ArchGame.Components;
using ArchGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.States {
	/// <summary>
	/// A State is a class which is used to define the behaviour and appearance of the game at a given moment.
	/// Examples of States (not implemented by ArchGame; game-specific): LoadingState, MenuState, OptionState, GameState.
	/// The states' Update and Draw methods are almost the only calls to implementation code during normal program execution.
	/// See the github wiki for more info.
	/// </summary>
	public abstract class State : ComponentListUser {
		/// <summary>
		/// Intialize the State base object.
		/// </summary>
		protected State() {
		}

		/// <summary>
		/// Load the content of the State.
		/// The base implementation loads the content of the componentList items.
		/// </summary>
		/// <param name="contentManager">The ContentManager</param>
		public override void LoadContent(ContentManager contentManager) {
			base.LoadContent(contentManager);
		}

		/// <summary>
		/// Update the State.
		/// The base implementation updates the componentList items.
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		/// <summary>
		/// Prompt the components in the State to obstruct their area.
		/// </summary>
		/// <param name="inputManager">The InputManager to register obstructions in</param>
		public override void ObstructArea(InputManager inputManager) {
			base.ObstructArea(inputManager);
		}

		/// <summary>
		/// Draw the State.
		/// The base implementation draws the componentList items.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);
		}

		/// <summary>
		/// Dispose the State.
		/// The base implementation disposes the componentList items.
		/// </summary>
		public override void Dispose() {
			base.Dispose();
		}
	}
}