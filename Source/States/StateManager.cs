using System;
using System.Collections.Generic;
using ArchGame.Components;
using ArchGame.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchGame.States {
	public class StateManager : IArchLoadable, IArchUpdateable, IArchDrawable, IDisposable {
		readonly Stack<State> stateStack;
		readonly ModuleFactory factory;
		ContentManager contentManager;

		Command command;
		State commandData;

		public int ZIndex { get { return 0; } }
		public int UpdatePriority { get { return 0; } }

		internal StateManager(ModuleFactory newFactory) {
			factory = newFactory;
			stateStack = new Stack<State>();
		}

		internal void PushState(State state) {
			stateStack.Push(state);

			IModuleRequester stateAsRequester = state as IModuleRequester;
			if (stateAsRequester != null) {
				factory.FullfillRequestNow(stateAsRequester);
			}

			state.LoadContent(contentManager);
		}

		internal void PopState() {
			if (stateStack.Count != 0) {
				stateStack.Peek().Dispose();
				stateStack.Pop();
			}
		}

		internal void ReplaceTopState(State state) {
			PopState();
			PushState(state);
		}

		public void RequestPushState(State state) {
			command = Command.Push;
			commandData = state;
		}

		public void RequestPopState() {
			command = Command.Pop;
		}

		public void RequestReplaceState(State state) {
			command = Command.Replace;
			commandData = state;
		}

		void FullfillCommand() {
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

		public void LoadContent(ContentManager newContentManager) {
			contentManager = newContentManager;			
		}

		public void Update(GameTime gameTime) {
			FullfillCommand();

			if (stateStack.Count != 0) {
				stateStack.Peek().Update(gameTime);
			}
		}

		public void Draw(SpriteBatch spriteBatch) {
			if (stateStack.Count != 0) {
				stateStack.Peek().Draw(spriteBatch);
			}
		}

		public void Dispose() {
			if (stateStack.Count != 0) {
				stateStack.Peek().Dispose();
			}
		}

		enum Command {
			None, Push, Pop, Replace
		}
	}
}