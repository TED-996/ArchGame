using System;
using System.Collections.Generic;
using ArchGame.Components;
using ArchGame.Input;
using ArchGame.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.States {
	/// <summary>
	/// The stateManager keeps a stack of States and manages operations on that stack.
	/// It also delegates calls to Update(), Draw() and ObstructArea() to the top state
	/// Also, it loads and disposes states when pushed or popped from the stack.
	/// The StateManager is accesible to implementation code through the ModuleFactory, where it's requestable.
	/// See the wiki for more information.
	/// </summary>
	public class StateManager : IArchLoadable, IArchUpdateable, IArchObstruction, IArchDrawable, IDisposable {
		readonly Stack<State> stateStack;
		readonly ModuleFactory factory;
		ContentManager contentManager;

		Command command;
		State commandData;

		public int ZIndex { get { return 0; } }
		public int UpdatePriority { get { return 0; } }

		/// <summary>
		/// Initializes a new instance of type StateManager.
		/// </summary>
		/// <param name="newFactory">The ModuleFactory to use to fulfill the requests of states.</param>
		internal StateManager(ModuleFactory newFactory) {
			factory = newFactory;
			stateStack = new Stack<State>();
		}

		/// <summary>
		/// Pushes a state to the stack. Its content will be loaded and its requests will be fulfilled.
		/// </summary>
		/// <param name="state">The state to push</param>
		internal void PushState(State state) {
			stateStack.Push(state);

			IModuleRequester stateAsRequester = state as IModuleRequester;
			if (stateAsRequester != null) {
				factory.FulfillRequestNow(stateAsRequester);
			}

			state.LoadContent(contentManager);
		}

		/// <summary>
		/// Pops the top state from the stack. It will be disposed properly.
		/// </summary>
		internal void PopState() {
			if (stateStack.Count != 0) {
				stateStack.Peek().Dispose();
				stateStack.Pop();
			}
		}

		/// <summary>
		/// Replaces the top state in a stack.
		/// </summary>
		/// <param name="state">The state to replace the top one with</param>
		internal void ReplaceTopState(State state) {
			PopState();
			PushState(state);
		}

		/// <summary>
		/// Registers a request to push a state on the next update cycle.
		/// </summary>
		/// <param name="state">The state to push</param>
		public void RequestPushState(State state) {
			command = Command.Push;
			commandData = state;
		}

		/// <summary>
		/// Registers a request to pop the top state on the next update cycle.
		/// </summary>
		public void RequestPopState() {
			command = Command.Pop;
		}

		/// <summary>
		/// Registers a request to replace the top state in the stack on the next update cycle.
		/// </summary>
		/// <param name="state">The state to replace the top one with</param>
		public void RequestReplaceState(State state) {
			command = Command.Replace;
			commandData = state;
		}

		void FulfillCommand() {
			if (command == Command.None) {
				return;
			}

			if (command == Command.Push) {
				PushState(commandData);
			}
			if (command == Command.Pop) {
				PopState();
			}
			if (command == Command.Replace) {
				ReplaceTopState(commandData);
			}

			command = Command.None;
		}

		/// <summary>
		/// Saves the ContentManager to be used later. Since no state is pushed yet, no content is loaded.
		/// </summary>
		/// <param name="newContentManager">The ContentManager</param>
		public void LoadContent(ContentManager newContentManager) {
			contentManager = newContentManager;			
		}

		/// <summary>
		/// Updates the current state.
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		public void Update(GameTime gameTime) {
			if (stateStack.Count != 0) {
				stateStack.Peek().Update(gameTime);
			}
		}

		/// <summary>
		/// Prompts the current state to obstruct its area.
		/// First, it fulfills the stack operation requests.
		/// </summary>
		/// <param name="inputManager">The InputManager to register the obstructions to</param>
		public void ObstructArea(InputManager inputManager) {
			FulfillCommand();

			if (stateStack.Count != 0) {
				stateStack.Peek().ObstructArea(inputManager);
			}
		}

		/// <summary>
		/// Draws the current state.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		public void Draw(SpriteBatch spriteBatch) {
			if (stateStack.Count != 0) {
				stateStack.Peek().Draw(spriteBatch);
			}
		}

		/// <summary>
		/// Disposes all the screens.
		/// </summary>
		public void Dispose() {
			while (stateStack.Count > 0) {
				stateStack.Pop().Dispose();
			}
		}

		enum Command {
			None, Push, Pop, Replace
		}
	}
}