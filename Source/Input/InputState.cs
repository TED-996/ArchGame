using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ArchGame.Extensions;

namespace ArchGame.Input {
	public delegate void KeyEnteredDelegate();

	public class InputState {
		KeyboardState keyboardState;
		KeyboardState oldKeyboardState;

		MouseState mouseState;
		MouseState oldMouseState;

		public int MouseX { get { return (int)((mouseState.X - mouseOffsetX) / mouseScale); } }
		public int TimeSinceLastKeypress { get; private set; }
		public int MouseY { get { return (int)((mouseState.Y - mouseOffsetY) / mouseScale); } }

		int mouseOffsetX;
		int mouseOffsetY;
		float mouseScale;

		readonly List<ObstructedSpot> obstructions;

		public readonly StringInputProcessor StringInputProcessor;

		readonly int[] keyTimeOut;
		const int DefaultTimeout = 500;
		const int ContinuousTimeout = 25;

		bool isPaused;

		internal InputState() {
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

		public bool IsKeyPressed(Keys key) {
			return keyboardState.IsKeyDown(key);
		}

		public bool HasKeyBeenPressed(Keys key) {
			if (isPaused) {
				return false;
			}
			return keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key);
		}

		public bool HasKeyBeenReleased(Keys key) {
			if (isPaused) {
				return false;
			}
			return oldKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
		}

		public bool HasKeyBeenPressedContinuous(Keys key) {
			if (isPaused) {
				return false;
			}
			int index = (int)key;

			return keyTimeOut[index] == 0 || keyTimeOut[index] == DefaultTimeout;
		}

		public bool IsPixelObstructed(int x, int y, int zIndex) {
			return obstructions.Any(spot => spot.ZIndex > zIndex && spot.Rectangle.Contains(x, y));
		}

		public void ObstructArea(Rectangle rectangle, int zIndex) {
			obstructions.Add(new ObstructedSpot { Rectangle = rectangle, ZIndex = zIndex });
		}


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

		public bool HasMouseButtonBeenReleased(int mouseButton, bool obstructable = false, int zIndex = 0,
			bool defaultObstructed = true) {
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

		public void GetMouseCoordinates(out int mouseX, out int mouseY) {
			mouseX = MouseX;
			mouseY = MouseY;
		}

		public Vector2 GetMouseDisplacement() {
			return new Vector2(mouseState.X - oldMouseState.X, mouseState.Y - oldMouseState.Y) / mouseScale;
		}

		public int GetScrollWheelDisplacement() {
			return mouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue;
		}

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

		public void SetFocus(bool hasFocus) {
			isPaused = !hasFocus;
		}

		public Vector2 GetMouseVector() {
			return new Vector2(MouseX, MouseY);
		}

		struct ObstructedSpot {
			public Rectangle Rectangle;
			public int ZIndex;
		}
 
	}
}