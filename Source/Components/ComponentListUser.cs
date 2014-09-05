using System;
using ArchGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Components {
	/// <summary>
	/// The ComponentListUser is an abstract class which wraps a ComponentList. Inherit it to easily create entities.
	/// Do not use it if you only need, for example, to draw the entity, with no update or obstruction step; the unnecessry calls may
	/// slow down your application.
	/// </summary>
	public abstract class ComponentListUser :IArchLoadable, IArchUpdateable, IArchObstruction, IArchDrawable, IDisposable {
		/// <summary>
		/// The ComponentList
		/// </summary>
		protected ComponentList componentList;

		public int UpdatePriority { get; private set; }
		public int ZIndex { get; private set; }

		/// <summary>
		/// Intialize the ComponentListUser base object.
		/// </summary>
		protected ComponentListUser(int newUpdatePriority = 0, int newZIndex = 0) {
			componentList = new ComponentList();
			UpdatePriority = newUpdatePriority;
			ZIndex = newZIndex;
		}

		/// <summary>
		/// Load the content of the ComponentList components.
		/// </summary>
		/// <param name="contentManager">The ContentManager</param>
		public virtual void LoadContent(ContentManager contentManager) {
			componentList.LoadContent(contentManager);
		}

		/// <summary>
		/// Update the ComponentList components.
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		public virtual void Update(GameTime gameTime) {
			componentList.Update(gameTime);
		}

		/// <summary>
		/// Prompt the components in the ComponentList to obstruct their area.
		/// </summary>
		/// <param name="inputManager">The InputManager to register obstructions to</param>
		public virtual void ObstructArea(InputManager inputManager) {
			componentList.ObstructArea(inputManager);
		}

		/// <summary>
		/// Draw the ComponentList components.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		public virtual void Draw(SpriteBatch spriteBatch) {
			componentList.Draw(spriteBatch);
		}

		/// <summary>
		/// Dispose the ComponentList components.
		/// </summary>
		public virtual void Dispose() {
			componentList.Dispose();
		}
	}
}