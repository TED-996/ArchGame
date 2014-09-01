using System;
using System.Collections.Generic;
using ArchGame.Components.XnaComponents;
using ArchGame.Input;
using ArchGame.Modules;
using ArchGame.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Test.States {
	/// <summary>
	/// The purpose of this state is to have an image on screen movable by arrow keys. Every few seconds, a trace, a copy of this image,
	/// is left on screen. Also, another trace can be left by pressing Space.
	/// </summary>
	public class GameState : State, IModuleRequester {
		readonly Sprite sprite;
		readonly Text text;

		InputState inputState;

		TimeSpan accumulator;

		static readonly TimeSpan Interval = TimeSpan.FromSeconds(1);

		public GameState() {
			//sprite taken from http://opengameart.org/content/6-sci-fi-symbols; under CC-BY-SA 3.0
			sprite = new Sprite("logo", 2) {Color = Color.LightBlue};
			text = new Text("font", "SPAACE", new Vector2(0, 50), 2) {Color = Color.White};
			componentList.Add(sprite);
			componentList.Add(text);
			accumulator = new TimeSpan();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			Vector2 direction = new Vector2();
			const float velocity = 2;
			if (inputState.IsKeyPressed(Keys.Up)) {
				direction += new Vector2(0, -1);
			}
			if (inputState.IsKeyPressed(Keys.Down)) {
				direction += new Vector2(0, 1);
			}
			if (inputState.IsKeyPressed(Keys.Left)) {
				direction += new Vector2(-1, 0);
			}
			if (inputState.IsKeyPressed(Keys.Right)) {
				direction += new Vector2(1, 0);
			}

			sprite.Position = sprite.Position + direction * velocity;
			text.Position = text.Position + direction * velocity;

			if (inputState.HasKeyBeenPressed(Keys.Space)) {
				componentList.Add(new Sprite(sprite) {Color = Color.White, ZIndex = 0});
			}

			if (inputState.HasKeyBeenPressed(Keys.C)) {
				sprite.Center = !sprite.Center;
				text.Center = !text.Center;
			}

			UpdateAccumulator(gameTime);
		}

		void UpdateAccumulator(GameTime gameTime) {
			accumulator += gameTime.ElapsedGameTime;

			if (accumulator > Interval) {
				accumulator -= Interval;
				componentList.Add(new Sprite(sprite) {Color = Color.LightGreen, ZIndex = 1});
			}
		}

		public IEnumerable<string> GetRequestedModules() {
			return new[] {"InputState"};
		}

		public void SetModules(ModuleCollection collection, ModuleFactory factory) {
			inputState = collection.GetModule<InputState>();
		}
	}
}