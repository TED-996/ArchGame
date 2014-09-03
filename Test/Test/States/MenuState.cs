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
	/// <summary>
	/// This will be shown right after the content has been loaded.
	/// </summary>
	public class MenuState : State, IModuleRequester {
		InputManager inputManager;
		StateManager stateManager;

		public MenuState() {
			componentList.Add(new Text("font", "Press Enter to play", new Vector2(100, 100), Color.White));
		}

		public IEnumerable<string> GetRequestedModules() {
			//We need the InputManager to read the Enter key and the StateManager to request to show the GameState.
			return new[] {"InputManager", "StateManager"};
		}

		public void SetModules(ModuleCollection collection, ModuleFactory factory) {
			inputManager = collection.GetModule<InputManager>();
			stateManager = collection.GetModule<StateManager>();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (inputManager.HasKeyBeenPressed(Keys.Enter)) {
				//We use the ServiceLocator to get the logger from anywhere. See that we're getting ILogger and not Logger; the logger 
				//may be a ThreadedLogger or a ConsoleLogger or a NullLogger (not logging anything), but we don't care.
				ServiceLocator.GetService<ILogger>().Log("Requesting state push: stateManager.RequestPushState(new GameState())",
					"MenuState");
				//Request the StateManager to show the GameState instead of this.
				stateManager.RequestPushState(new GameState());
			}
		}
	}
}