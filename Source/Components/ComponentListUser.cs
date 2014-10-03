using System;
using System.Collections.Generic;
using ArchGame.Input;
using ArchGame.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.Components {
	/// <summary>
	/// The ComponentListUser is an abstract class which wraps a ComponentList. Inherit it to easily create entities.
	/// Do not use it if you only need, for example, to draw the entity, with no update or obstruction step; the unnecessry calls may
	/// slow down your application.
	/// </summary>
	public abstract class ComponentListUser : IModuleRequester, IArchLoadable, IArchUpdateable, IArchObstruction, IArchDrawable,
		IDisposable {
		/// <summary>
		/// The ComponentList
		/// </summary>
		protected ComponentList componentList;

		/// <summary>
		/// The ZIndex of the ComponentListUser
		/// </summary>
		public int ZIndex { get; private set; }

		/// <summary>
		/// The UpdatePriority of the ComponentListUser
		/// </summary>
		public int UpdatePriority { get; private set; }

		/// <summary>
		/// Intialize the ComponentListUser base object.
		/// </summary>
		protected ComponentListUser(int newUpdatePriority = 0, int newZIndex = 0) {
			componentList = new ComponentList();
			UpdatePriority = newUpdatePriority;
			ZIndex = newZIndex;
		}

		/// <summary>
		/// Gets the requested modules of the ComponentList.
		/// Returns an empty array.
		/// </summary>
		public virtual IEnumerable<string> GetRequestedModules() {
			return componentList.GetRequestedModules();
		}

		/// <summary>
		/// Sets the modules of the ComponentList components.
		/// </summary>
		/// <param name="collection">The empty ModuleCollection built by the ModuleFactory.</param>
		/// <param name="factory">The ModuleFactory</param>
		public virtual void SetModules(ModuleCollection collection, ModuleFactory factory) {
			componentList.SetModules(collection, factory);
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
		/// <param name="obstructionManager">The IObstructionManager to register obstructions to</param>
		public virtual void ObstructArea(IObstructionManager obstructionManager) {
			componentList.ObstructArea(obstructionManager);
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