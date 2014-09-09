using System;
using System.Collections.Generic;
using ArchGame.Components.XnaComponents;
using ArchGame.Input;
using ArchGame.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ArchGame.Extensions;

namespace ArchGame.Components.UiComponents {
	/// <summary>
	/// A button is a UI component that performs an action when clicked.
	/// This requests the InputManager, you must fullfill its request through the module factory (FulfillRequestNow works well)
	/// </summary>
	public class Button : IArchLoadable, IArchUpdateable, IArchDrawable, IArchObstruction, IModuleRequester {
		readonly Sprite sprite;
		readonly Text text;

		readonly Action clickAction;

		InputManager inputManager;

		/// <summary>
		/// The UpdatePriority of the button.
		/// </summary>
		public int UpdatePriority { get; set; }

		/// <summary>
		/// The ZIndex of the button.
		/// </summary>
		public int ZIndex { get; set; }

		/// <summary>
		/// Initialize a new instance of type Button.
		/// This button will draw a texture, without text.
		/// **The texture is centered**
		/// </summary>
		/// <param name="position">The position of the button. This position is centered.</param>
		/// <param name="textureFilename">The filename of the texture</param>
		/// <param name="newClickAction">The action to perfom on mouse click</param>
		/// <param name="newZIndex">The ZIndex of the button</param>
		/// <param name="newUpdatePriority">The UpdatePriority of the button</param>
		public Button(Vector2 position, string textureFilename, Action newClickAction, int newZIndex = 0, int newUpdatePriority = 0) {
			sprite = new Sprite(textureFilename, position) {Center = true};
			text = null;

			clickAction = newClickAction;

			ZIndex = newZIndex;
			UpdatePriority = newUpdatePriority;
		}

		/// <summary>
		/// Initialize a new instance of type Button.
		/// This button will draw a texture and text.
		/// **The texture and text are centered**
		/// </summary>
		/// <param name="position">The position of the button. This position is centered.</param>
		/// <param name="textureFilename">The filename of the texture</param>
		/// <param name="fontFilename">The filename of the font to use for the text</param>
		/// <param name="textToDraw">The text to be drawn</param>
		/// <param name="newClickAction">The action to perfom on mouse click</param>
		/// <param name="newZIndex">The ZIndex of the button</param>
		/// <param name="newUpdatePriority">The UpdatePriority of the button</param>
		public Button(Vector2 position, string textureFilename, string fontFilename, string textToDraw, Action newClickAction,
			int newZIndex = 0, int newUpdatePriority = 0)
			: this(position, textureFilename, newClickAction, newZIndex, newUpdatePriority) {
			text = null;
			if (!fontFilename.IsNullOrEmpty() && !textToDraw.IsNullOrEmpty()) {
				text = new Text(fontFilename, textToDraw, position) {Center = true};
			}
		}
		
		/// <summary>
		/// Load the content of the Button.
		/// </summary>
		/// <param name="contentManager">The ContentManager to load content from.</param>
		public void LoadContent(ContentManager contentManager) {
			sprite.LoadContent(contentManager);
			if (text != null) {
				text.LoadContent(contentManager);
			}
		}

		/// <summary>
		/// Update the Button.
		/// </summary>
		/// <param name="gameTime">The GameTime</param>
		public void Update(GameTime gameTime) {
			if (inputManager.HasMouseButtonBeenPressed(MouseAction.Left, true, ZIndex) &&
				sprite.GetGlobalBounds().Contains(inputManager.MouseX, inputManager.MouseY)) {
				clickAction();
			}
		}

		/// <summary>
		/// Draw the Button.
		/// </summary>
		/// <param name="spriteBatch"></param>
		public void Draw(SpriteBatch spriteBatch) {
			sprite.Draw(spriteBatch);
			if (text != null) {
				text.Draw(spriteBatch);
			}
		}

		/// <summary>
		/// Obstruct the area behind the Button.
		/// </summary>
		/// <param name="obstructionManager"></param>
		public void ObstructArea(IObstructionManager obstructionManager) {
			obstructionManager.ObstructArea(sprite.GetGlobalBounds(), ZIndex);
		}

		/// <summary>
		/// Request the InputManager.
		/// </summary>
		IEnumerable<string> IModuleRequester.GetRequestedModules() {
			return new[] {"InputManager"};
		}

		/// <summary>
		/// Set the InputManager in the instance.
		/// </summary>
		void IModuleRequester.SetModules(ModuleCollection collection, ModuleFactory factory) {
			inputManager = collection.GetModule<InputManager>();
		}
	}
}