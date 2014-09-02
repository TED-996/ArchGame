using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArchGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ArchGame.Extensions;

namespace ArchGame.Components {
	/// <summary>
	/// The ComponentList keeps lists of IArchLoadables, IArchUpdateables, IArchObstructables IArchDrawables and IDisposables 
	/// Performs the requested operations on all components. Use it to declaratively add behaviour and appearance to your states.
	/// </summary>
	public class ComponentList : IArchLoadable, IArchUpdateable, IArchDrawable, IArchObstruction, IDisposable {
		readonly List<IArchLoadable> loadables;
		readonly List<IArchUpdateable> updateables;
		readonly List<IArchObstruction> obstructions; 
		readonly List<IArchDrawable> drawables;
		readonly List<IDisposable> disposables;

		/// <summary>
		/// Get the IArchLoadables as a read-only collection.
		/// </summary>
		public ReadOnlyCollection<IArchLoadable> Loadables { get { return loadables.AsReadOnly(); } }

		/// <summary>
		/// Get the IArchUpdateables as a read-only collection.
		/// </summary>
		public ReadOnlyCollection<IArchUpdateable> Updateables { get { return updateables.AsReadOnly(); } }

		/// <summary>
		/// Get the IArchObstructions as a read-only collection.
		/// </summary>
		public ReadOnlyCollection<IArchObstruction> Obstructions { get { return obstructions.AsReadOnly(); } }

		/// <summary>
		/// Get the IArchDrawables as a read-only collection.
		/// </summary>
		public ReadOnlyCollection<IArchDrawable> Drawables { get { return drawables.AsReadOnly(); } }

		/// <summary>
		/// Get the IArchDisposables as a read-only collection.
		/// </summary>
		public ReadOnlyCollection<IDisposable> Disposables { get { return disposables.AsReadOnly(); } }

		public int ZIndex { get { return 0; } }
		public int UpdatePriority { get { return 0; } }

		bool updateablesDirty;
		bool drawablesDirty;

		/// <summary>
		/// Initialize a new instance of type ComponentList
		/// </summary>
		public ComponentList() {
			loadables = new List<IArchLoadable>();
			updateables = new List<IArchUpdateable>();
			obstructions = new List<IArchObstruction>();
			drawables = new List<IArchDrawable>();
			disposables = new List<IDisposable>();

			updateablesDirty = false;
			drawablesDirty = false;
		}

		/// <summary>
		/// Initialize a new instance of type ComponentList, as a copy of another.
		/// </summary>
		/// <param name="other">The list to copy from</param>
		public ComponentList(ComponentList other) : this() {
			AppendList(other);
		}

		/// <summary>
		/// Add a new component to the ComponentList
		/// </summary>
		/// <param name="component">The component to add</param>
		public void Add(Object component) {
			IArchLoadable loadable = component as IArchLoadable;
			if (loadable != null) {
				loadables.Add(loadable);
			}
			IArchUpdateable updateable = component as IArchUpdateable;
			if (updateable != null) {
				updateables.Add(updateable);
				updateablesDirty = true;
			}
			IArchObstruction obstruction = component as IArchObstruction;
			if (obstruction != null) {
				obstructions.Add(obstruction);
			}
			IArchDrawable drawable = component as IArchDrawable;
			if (drawable != null) {
				drawables.Add(drawable);
				drawablesDirty = true;
			}
			IDisposable disposable = component as IDisposable;
			if (disposable != null) {
				disposables.Add(disposable);
			}
		}

		/// <summary>
		/// Append a ComponentList to this list.
		/// </summary>
		/// <param name="other">The ComponentList to append</param>
		public void AppendList(ComponentList other) {
			loadables.AddRange(other.Loadables);
			updateables.AddRange(other.Updateables);
			obstructions.AddRange(other.obstructions);
			drawables.AddRange(other.Drawables);
			disposables.AddRange(other.Disposables);

			drawablesDirty = true;
			updateablesDirty = true;
		}

		/// <summary>
		/// Remove a component from the ComponentList
		/// </summary>
		/// <param name="component">The component to remove</param>
		public void Remove(Object component) {
			IArchLoadable loadable = component as IArchLoadable;
			if (loadable != null) {
				loadables.Remove(loadable);
			}
			IArchUpdateable updateable = component as IArchUpdateable;
			if (updateable != null) {
				updateables.Remove(updateable);
			}
			IArchObstruction obstruction = component as IArchObstruction;
			if (obstruction != null) {
				obstructions.Remove(obstruction);
			}
			IArchDrawable drawable = component as IArchDrawable;
			if (drawable != null) {
				drawables.Remove(drawable);
			}
			IDisposable disposable = component as IDisposable;
			if (disposable != null) {
				disposables.Remove(disposable);
			}
		}

		/// <summary>
		/// Loads the content of each IArchLoadable in the ComponentList
		/// </summary>
		/// <param name="contentManager">The content manager</param>
		public void LoadContent(ContentManager contentManager) {
			loadables.ForEach(loadable => loadable.LoadContent(contentManager));
		}

		/// <summary>
		/// Updates each IArchUpdateable
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		public void Update(GameTime gameTime) {
			if (updateablesDirty) {
				CleanUpdateables();
			}
			updateables.ForEach(updateable => updateable.Update(gameTime));
		}

		void CleanUpdateables() {
			updateables.StableSort((u1, u2) => u2.UpdatePriority.CompareTo(u1.UpdatePriority));
			updateablesDirty = false;
		}

		/// <summary>
		/// Prompt each IArchObstruction to obstruct its area.
		/// </summary>
		public void ObstructArea(InputManager inputManager) {
			obstructions.ForEach(obstruction => obstruction.ObstructArea(inputManager));
		}

		/// <summary>
		/// Draw each IArchDrawable
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch</param>
		public void Draw(SpriteBatch spriteBatch) {
			if (drawablesDirty) {
				CleanDrawables();
			}
			drawables.ForEach(drawable => drawable.Draw(spriteBatch));
		}

		void CleanDrawables() {
			drawables.StableSort((d1, d2) => d1.ZIndex.CompareTo(d2.ZIndex));
			drawablesDirty = false;
		}

		/// <summary>
		/// Dispose each IDisposable
		/// </summary>
		public void Dispose() {
			disposables.ForEach(disposable => disposable.Dispose());
		}
	}
}