using System;
using ArchGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.States {
	public abstract class State : IArchLoadable, IArchUpdateable, IArchDrawable, IDisposable {
		protected ComponentList componentList;

		public int UpdatePriority {
			get { return 0; }
		}

		public int ZIndex {
			get { return 0; }
		}

		protected State() {
			componentList = new ComponentList();
		}

		public virtual void LoadContent(ContentManager contentManager) {
			componentList.LoadContent(contentManager);
		}

		public virtual void Update(GameTime gameTime) {
			componentList.Update(gameTime);
		}

		public virtual void Draw(SpriteBatch spriteBatch) {
			componentList.Draw(spriteBatch);
		}

		public virtual void Dispose() {
			componentList.Dispose();
		}
	}
}