using System.Collections.Generic;
using System.Linq;
using ArchGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ArchGame.Extensions;

namespace ArchGame.Input {
	/// <summary>
	/// Represents the current state of mouse and keyboard input.
	/// See the wiki for help on how the input in ArchGame works.
	/// </summary>
	public class InputManager {
		KeyboardState keyboardState;
		KeyboardState oldKeyboardState;

		MouseState mouseState;
		MouseState oldMouseState;

		/// <summary>
		/// The X coordinate of the mouse
		/// </summary>
		public int MouseX { get { return (int)((mouseState.X - mouseOffsetX) / mouseScale); } }

		/// <summary>
		/// The Y coordinate of the mouse
		/// </summary>
		public int MouseY { get { return (int)((mouseState.Y - mouseOffsetY) / mouseScale); } }

		/// <summary>
		/// How much time has passed since the last keypress
		/// </summary>
		public int TimeSinceLastKeypress { get; private set; }

		int mouseOffsetX;
		int mouseOffsetY;
		float mouseScale;

		readonly List<ObstructedSpot> obstructions;

		/// <summary>
		/// The StringInputProcessor
		/// </summary>
		public readonly StringInputProcessor StringInputProcessor;

		readonly int[] keyTimeOut;
		const int DefaultTimeout = 500;
		const int ContinuousTimeout = 25;

		bool isPaused;

		/// <summary>
		/// Initializes a new instance of type InputManager.
		/// </summary>
		internal InputManager() {
			oldKeyboardState = new KeyboardState();
			oldMouseState = new MouseState();

			keyboardState = new KeyboardState();
			mouseState = new MouseState();

			keyTimeOut = new int[256];
			for (int i = 0; i < keyTimeOut.Length; i++) {
				keyTimeOut[i] = -1;
			}

			StringInputProcessor = new StringInputProcessor(this);

			isPaused = false;
			obstructions = new List<ObstructedSpot>(100);

			mouseOffsetX = 0;
			mouseOffsetY = 0;
			mouseScale = 1;
		}

		/// <summary>
		/// Updates the InputManager
		/// </summary>
		/// <param name="newKeyboardState">The keyboard state at this moment</param>
		/// <param name="newMouseState">The mouse state at this moment</param>
		/// <param name="gameTime">The GameTime</param>
		internal void Update(KeyboardState newKeyboardState, MouseState newMouseState, GameTime gameTime) {
			obstructions.Clear();
			if (isPaused) {
				return;
			}
			oldKeyboardState = keyboardState;
			oldMouseState = mouseState;

			keyboardState = newKeyboardState;
			mouseState = newMouseState;

			for (int i = 0; i < 256; i++) {
				if (keyTimeOut[i] == 0) {
					keyTimeOut[i] = ContinuousTimeout;
				}
			}
			for (int i = 0; i < 256; i++) {
				Keys thisKey = (Keys)i;
				if (HasKeyBeenPressed(thisKey)) {
					keyTimeOut[i] = DefaultTimeout;
				}
				else if (IsKeyPressed(thisKey)) {
					keyTimeOut[i] -= gameTime.ElapsedGameTime.Milliseconds.AtMost(keyTimeOut[i]);
				}
				else {
					keyTimeOut[i] = -1;
				}
			}

			if (keyTimeOut.All(time => time == -1)) {
				TimeSinceLastKeypress += gameTime.ElapsedGameTime.Milliseconds;
			}
			else {
				TimeSinceLastKeypress = 0;
			}

			StringInputProcessor.Update();
		}

		/// <summary>
		/// Returns true if a key is currently down.
		/// </summary>
		public bool IsKeyPressed(Keys key) {
			return keyboardState.IsKeyDown(key);
		}

		/// <summary>
		/// Returns true if a key has been pressed during this update cycle.
		/// </summary>
		public bool HasKeyBeenPressed(Keys key) {
			if (isPaused) {
				return false;
			}
			return keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key);
		}

		/// <summary>
		/// Returns true if a key has been depressed during this update cycle.
		/// </summary>
		public bool HasKeyBeenReleased(Keys key) {
			if (isPaused) {
				return false;
			}
			return oldKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
		}

		/// <summary>
		/// Returns true if a key has been pressed during this update cycle.
		/// Also returns true if the key is held and a certain time interval has passed; the timer is then reset to allow for another 
		/// keypress.
		/// This attempts to simulate Windows' behaviour with held keys.
		/// Press -> longer interval -> auto-press -> shorter interval -> auto-press (repeat until key is up)
		/// </summary>
		public bool HasKeyBeenPressedContinuous(Keys key) {
			if (isPaused) {
				return false;
			}
			int index = (int)key;

			return keyTimeOut[index] == 0 || keyTimeOut[index] == DefaultTimeout;
		}

		/// <summary>
		/// Returns true if the pixel at the given coordinates is currently obstructed.
		/// </summary>
		public bool IsPixelObstructed(int x, int y, int zIndex) {
			return obstructions.Any(spot => spot.ZIndex > zIndex && spot.Rectangle.Contains(x, y));
		}

		/// <summary>
		/// Adds a rectangle to the obstruction list.
		/// </summary>
		/// <param name="rectangle">The rectangle to be obstructed</param>
		/// <param name="zIndex">The ZIndex of the rectangle</param>
		public void ObstructArea(Rectangle rectangle, int zIndex) {
			obstructions.Add(new ObstructedSpot { Rectangle = rectangle, ZIndex = zIndex });
		}

		/// <summary>
		/// Prompt an IArchObstruction to register its obstruction.
		/// </summary>
		public void ObstructArea(IArchObstruction obstruction) {
			obstruction.ObstructArea(this);
		}

		/// <summary>
		/// Returns true if the mouse button is currently pressed.
		/// TODO: Use an enum for the mouse button
		/// </summary>
		/// <param name="mouseButton">The pressed mouse button</param>
		/// <param name="obstructable">Wheteher obstructions should be considered</param>
		/// <param name="zIndex">The ZIndex of the clicked element, used if obstructions are considered</param>
		public bool IsMouseButtonPressed(int mouseButton, bool obstructable = false, int zIndex = 0) {
			if (obstructable && IsPixelObstructed(MouseX, MouseY, zIndex)) {
				return false;
			}
			if (mouseButton == 1) {
				return mouseState.LeftButton == ButtonState.Pressed;
			}
			if (mouseButton == 2) {
				return mouseState.RightButton == ButtonState.Pressed;
			}
			if (mouseButton == 3) {
				return mouseState.MiddleButton == ButtonState.Pressed;
			}
			if (mouseButton == 4) {
				return mouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue;
			}
			if (mouseButton == 5) {
				return mouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue;
			}
			return false;
		}

		/// <summary>
		/// Returns true if the mouse button has been pressed in the last update cycle.
		/// </summary>
		/// <param name="mouseButton">The pressed mouse button</param>
		/// <param name="obstructable">Wheteher obstructions should be considered</param>
		/// <param name="zIndex">The ZIndex of the clicked element, used if obstructions are considered</param>
		public bool HasMouseButtonBeenPressed(int mouseButton, bool obstructable = false, int zIndex = 0) {
			if (isPaused) {
				return false;
			}
			if (obstructable && IsPixelObstructed(MouseX, MouseY, zIndex)) {
				return false;
			}
			if (mouseButton == 1) {
				return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
			}
			if (mouseButton == 2) {
				return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
			}
			if (mouseButton == 3) {
				return mouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Released;
			}
			if (mouseButton == 4) {
				return mouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue;
			}
			if (mouseButton == 5) {
				return mouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue;
			}
			return false;
		}

		/// <summary>
		/// Returns true if the mouse button has been released in the last update cycle
		/// </summary>
		/// <param name="mouseButton">The pressed mouse button</param>
		/// <param name="obstructable">Wheteher obstructions should be considered</param>
		/// <param name="zIndex">The ZIndex of the clicked element, used if obstructions are considered</param>
		/// <param name="defaultObstructed">The returned value if the element is obstructed</param>
		public bool HasMouseButtonBeenReleased(int mouseButton, bool obstructable = false, int zIndex = 0, bool defaultObstructed = true) {
			if (isPaused) {
				return false;
			}
			if (obstructable && IsPixelObstructed(MouseX, MouseY, zIndex)) {
				return defaultObstructed;
			}
			if (mouseButton == 1) {
				return mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
			}
			if (mouseButton == 2) {
				return mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed;
			}
			if (mouseButton == 3) {
				return mouseState.MiddleButton == ButtonState.Released && oldMouseState.MiddleButton == ButtonState.Pressed;
			}
			if (mouseButton == 4) {
				return mouseState.ScrollWheelValue > oldMouseState.ScrollWheelValue;
			}
			if (mouseButton == 5) {
				return mouseState.ScrollWheelValue < oldMouseState.ScrollWheelValue;
			}
			return false;
		}

		internal void SetMouseOffset(int offsetX, int offsetY) {
			mouseOffsetX = offsetX;
			mouseOffsetY = offsetY;
		}

		internal void SetMouseScale(float scale) {
			mouseScale = scale;
		}

		/// <summary>
		/// Get the mouse coordinates.
		/// </summary>
		public void GetMouseCoordinates(out int mouseX, out int mouseY) {
			mouseX = MouseX;
			mouseY = MouseY;
		}

		/// <summary>
		/// Get the mouse position as a Vector2
		/// </summary>
		public Vector2 GetMouseVector() {
			return new Vector2(MouseX, MouseY);
		}

		/// <summary>
		/// Get the movement of the mouse in the last update cycle as a Vector2
		/// </summary>
		public Vector2 GetMouseDisplacement() {
			return new Vector2(mouseState.X - oldMouseState.X, mouseState.Y - oldMouseState.Y) / mouseScale;
		}

		/// <summary>
		/// Get the movement of the scroll wheel in the last update cycle.
		/// </summary>
		public int GetScrollWheelDisplacement() {
			return mouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue;
		}

		/// <summary>
		/// Returns true if one of the modifier is pressed.
		/// It considers both keys in the case of pairs (for example, IsModifierPressed(Keys.LeftAlt) will also consider Keys.RightAlt)
		/// </summary>
		public bool IsModifierPressed(Keys modifier) {
			if (modifier == Keys.LeftAlt || modifier == Keys.RightAlt) {
				return IsKeyPressed(Keys.LeftAlt) || IsKeyPressed(Keys.RightAlt);
			}
			if (modifier == Keys.LeftShift || modifier == Keys.RightShift) {
				return IsKeyPressed(Keys.LeftShift) || IsKeyPressed(Keys.RightShift);
			}
			if (modifier == Keys.LeftControl || modifier == Keys.RightControl) {
				return IsKeyPressed(Keys.LeftControl) || IsKeyPressed(Keys.RightControl);
			}
			if (modifier == Keys.LeftWindows || modifier == Keys.RightWindows) {
				return IsKeyPressed(Keys.LeftWindows) || IsKeyPressed(Keys.RightWindows);
			}
			return false;
		}

		/// <summary>
		/// Returns an array of keys that were pressed during the last update cycle.
		/// </summary>
		public Keys[] GetKeysJustPressed() {
			Keys[] currentKeys = keyboardState.GetPressedKeys();
			Keys[] oldKeys = oldKeyboardState.GetPressedKeys();

			LinkedList<Keys> recentKeys = new LinkedList<Keys>(currentKeys);
			foreach (Keys oldKey in oldKeys.Where(recentKeys.Contains)) {
				recentKeys.Remove(oldKey);
			}
			Keys[] recentKeysArray = new Keys[recentKeys.Count];
			recentKeys.CopyTo(recentKeysArray, 0);

			return recentKeysArray;
		}

		/// <summary>
		/// Sets whether the window has focus (is currently active). If not, the InputManager is paused.
		/// </summary>
		/// <param name="hasFocus"></param>
		internal void SetFocus(bool hasFocus) {
			isPaused = !hasFocus;
		}

		struct ObstructedSpot {
			public Rectangle Rectangle;
			public int ZIndex;
		}
 
	}
}