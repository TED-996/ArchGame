using ArchGame;
using ArchGame.Components;
using ArchGame.States;
using Microsoft.Xna.Framework.Graphics;
using Test.States;

namespace Test {
	public class Game1 : ArchGame.ArchGame {
		public Game1() : base("Testing", 800, 600) {
			
		}

		protected override LoadableSet GetLoadableSet() {
			return new LoadableSet(new IArchLoadable[] {new ContentToIArchLoadable<Texture2D>("logo")},
				new IArchLoadable[] {new ContentToIArchLoadable<SpriteFont>("font")});
		}

		protected override State GetLoadingState() {
			return new LoadingState();
		}

		protected override State GetAfterLoadState() {
			return new MenuState();
		}
	}
}
