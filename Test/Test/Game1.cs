using ArchGame.States;
using Test.States;

namespace Test {
	public class Game1 : ArchGame.ArchGame {
		public Game1() : base("Testing", 800, 600) {
			
		}

		protected override State GetLoadingState() {
			return new LoadingState();
		}

		protected override State GetAfterLoadState() {
			return new GameState();
		}
	}
}
