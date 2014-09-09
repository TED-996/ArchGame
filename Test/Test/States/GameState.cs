using System;
using System.Collections.Generic;
using ArchGame.Components.XnaComponents;
using ArchGame.Input;
using ArchGame.Modules;
using ArchGame.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Test.Network;

namespace Test.States {
	/// <summary>
	/// The purpose of this state is to have an image on screen movable by arrow keys. Every few seconds, a trace, a copy of this image,
	/// is left on screen. Also, another trace can be left by pressing Space.
	/// In the top-left corner of the screen, a random number received from RANDOM.ORG will be shown.
	/// </summary>
	public class GameState : State, IModuleRequester {
		readonly Sprite sprite;
		readonly Text text;

		InputManager inputManager;
		INetworkManager networkManager;

		TimeSpan accumulator;

		static readonly TimeSpan Interval = TimeSpan.FromSeconds(1);

		public GameState() {
			//The sprite was taken from http://opengameart.org/content/6-sci-fi-symbols; It is licensed under CC-BY-SA 3.0
			//The sprite was cropped from a larger image.

			// Initialize a Sprite and a Text object.
			sprite = new Sprite("logo", 2) {Color = Color.LightBlue};
			text = new Text("font", "ArrowKeys & Space", new Vector2(0, 180), 2) {Color = Color.White};
			//And add them to the ComponentList. This eliminates any worries of manually updating and drawing them.
			componentList.Add(sprite);
			componentList.Add(text);

			accumulator = new TimeSpan();
		}
		public override void Update(GameTime gameTime) {
			//Update the ComponentList.
			base.Update(gameTime);

			//Get the input from the InputManager. We are using the IsKeyPressed methods, which return true as long as the key is pressed.
			Vector2 direction = new Vector2();
			const float velocity = 2;
			if (inputManager.IsKeyPressed(Keys.Up)) {
				direction += new Vector2(0, -1);
			}
			if (inputManager.IsKeyPressed(Keys.Down)) {
				direction += new Vector2(0, 1);
			}
			if (inputManager.IsKeyPressed(Keys.Left)) {
				direction += new Vector2(-1, 0);
			}
			if (inputManager.IsKeyPressed(Keys.Right)) {
				direction += new Vector2(1, 0);
			}

			//Apply the movement to the movable objects.
			sprite.Position = sprite.Position + direction * velocity;
			text.Position = text.Position + direction * velocity;

			//HasKeyBeenPressed returns true only in the update cycle when the key was presed (only one time per key hold)
			//This avoids leaving behind a huge number of Sprite objects.
			if (inputManager.HasKeyBeenPressed(Keys.Space)) {
				//Add a copy of the sprite to the ComponentList. We want another color and another ZIndex for those though.
				componentList.Add(new Sprite(sprite) {Color = Color.White, ZIndex = 0});
			}

			if (inputManager.HasKeyBeenPressed(Keys.C)) {
				//Center or de-center the sprite and the text. Try the game to see the difference.
				sprite.Center = !sprite.Center;
				text.Center = !text.Center;
			}

			UpdateAccumulator(gameTime);
		}

		void UpdateAccumulator(GameTime gameTime) {
			accumulator += gameTime.ElapsedGameTime;

			if (accumulator > Interval) {
				accumulator -= Interval;
				//Every Interval we leave behind a copy of the sprite. We want another color and another ZIndex for those though.
				componentList.Add(new Sprite(sprite) {Color = Color.LightGreen, ZIndex = 1});
			}
		}

		IEnumerable<string> IModuleRequester.GetRequestedModules() {
			//This state requests the InputManager and the NetworkManager.
			return new[] {"InputManager", "INetworkManager"};
		}

		void IModuleRequester.SetModules(ModuleCollection collection, ModuleFactory factory) {
			//Save the modules from the ModuleCollection
			inputManager = collection.GetModule<InputManager>();
			networkManager = collection.GetModule<INetworkManager>();

			//Initialize the NetworkManager
			networkManager.Request();

			//Create a new NetworkResponseRenderer with the NetworkManager that we just got.
			componentList.Add(new NetworkResponseRenderer(networkManager));
		}
	}
}