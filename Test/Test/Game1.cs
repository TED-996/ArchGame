using ArchGame;
using ArchGame.Components;
using ArchGame.Content;
using ArchGame.States;
using Microsoft.Xna.Framework.Graphics;
using Test.Network;
using Test.States;

namespace Test {
	/// <summary>
	/// This is the type of our game.
	/// </summary>
	public class Game1 : ArchGame.ArchGame {
		public Game1() : base("Testing", 800, 600) {
			//We want to register the NetworkManager as a module. This will automatically construct and dispose it and make it requestable.
			moduleFactory.RegisterModule<NetworkManager, INetworkManager>();
		}

		protected override LoadableSet GetLoadableSet() {
			//The font must be loaded before because the LoadingState uses it. The logo can be loaded during the loading phase,
			//it will not be needed until later.
			return new LoadableSet(new IArchLoadable[] {new ContentToIArchLoadable<Texture2D>("logo")},
				new IArchLoadable[] {new ContentToIArchLoadable<SpriteFont>("font")});
		}

		protected override State GetLoadingState() {
			//When loading, we want to see the LoadingState that we made.
			return new LoadingState();
		}

		protected override State GetAfterLoadState() {
			//After we're done loading, we want to see the menu.
			return new MenuState();
		}
	}
}
