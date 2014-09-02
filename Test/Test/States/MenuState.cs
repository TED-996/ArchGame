using System.Collections.Generic;
using ArchGame;
using ArchGame.Components.XnaComponents;
using ArchGame.Input;
using ArchGame.Modules;
using ArchGame.Services;
using ArchGame.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Test.States {
	public class MenuState : State, IModuleRequester {
		InputManager inputManager;
		StateManager stateManager;

		public MenuState() {
			componentList.Add(new Text("font", "Press Enter to play", new Vector2(100, 100), Color.White));
		}

		public IEnumerable<string> GetRequestedModules() {
			return new[] {"InputManager", "StateManager"};
		}

		public void SetModules(ModuleCollection collection, ModuleFactory factory) {
			inputManager = collection.GetModule<InputManager>();
			stateManager = collection.GetModule<StateManager>();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (inputManager.HasKeyBeenPressed(Keys.Enter)) {
				ServiceLocator.GetService<ILogger>().Log("Requesting state push: stateManager.RequestPushState(new GameState())",
					"MenuState");
				stateManager.RequestPushState(new GameState());
			}
		}
	}
}