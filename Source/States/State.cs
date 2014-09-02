using System;
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
	public abstract class State : IArchLoadable, IArchUpdateable, IArchObstruction, IArchDrawable, IDisposable {
		/// <summary>
		/// The ComponentList
		/// </summary>
		protected ComponentList componentList;

		public int UpdatePriority {
			get { return 0; }
		}
		public int ZIndex {
			get { return 0; }
		}

		/// <summary>
		/// Intialize the State base object.
		/// </summary>
		protected State() {
			componentList = new ComponentList();
		}

		/// <summary>
		/// Load the content of the State.
		/// The base implementation loads the content of the componentList items.
		/// </summary>
		/// <param name="contentManager">The ContentManager</param>
		public virtual void LoadContent(ContentManager contentManager) {
			componentList.LoadContent(contentManager);
		}

		/// <summary>
		/// Update the State.
		/// The base implementation updates the componentList items.
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		public virtual void Update(GameTime gameTime) {
			componentList.Update(gameTime);
		}

		/// <summary>
		/// Prompt the components in the State to obstruct their area.
		/// </summary>
		/// <param name="inputManager">The InputManager to register obstructions in</param>
		public void ObstructArea(InputManager inputManager) {
			componentList.ObstructArea(inputManager);
		}

		/// <summary>
		/// Draw the State.
		/// The base implementation draws the componentList items.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		public virtual void Draw(SpriteBatch spriteBatch) {
			componentList.Draw(spriteBatch);
		}

		/// <summary>
		/// Dispose the State.
		/// The base implementation disposes the componentList items.
		/// </summary>
		public virtual void Dispose() {
			componentList.Dispose();
		}
	}
}