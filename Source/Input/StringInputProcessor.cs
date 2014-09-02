using System.Globalization;
using Microsoft.Xna.Framework.Input;

namespace ArchGame.Input {
	/// <summary>
	/// Facilitates character-based input.
	/// </summary>
	public class StringInputProcessor {
		/// <summary>
		/// True if the StringInputProcessor is currently recording, false otherwise.
		/// </summary>
		public bool IsRecording { get; private set; }

		/// <summary>
		/// True if the StringInputProcessor is currently paused, false otherwise. This is independent to IsRecording.
		/// </summary>
		public bool IsPaused { get; private set; }

		/// <summary>
		/// The caret position in the recorded string.
		/// </summary>
		public int CaretPosition { get; set; }

		/// <summary>
		/// The recorded string.
		/// </summary>
		public string RecordedKeys { get; set; }

		/// <summary>
		/// True if the StringInputProcessor has been updated during this update cycle.
		/// </summary>
		public bool Updated { get; private set; }

		string recentKeys = "";

		readonly InputManager inputManager;

		bool deleteRecentKeys;

		int selectionStart;
		int selectionLength;

		/// <summary>
		/// Initialize a new instance of type StringInputProcessor
		/// </summary>
		/// <param name="newInputManager">The InputManager to use</param>
		public StringInputProcessor(InputManager newInputManager) {
			IsRecording = false;
			IsPaused = false;
			RecordedKeys = "";
			selectionLength = 1;
			selectionStart = -1;

			inputManager = newInputManager;

			EventInputManager.CharacterEntered += OnCharacterEntered;
		}

		/// <summary>
		/// Updates the StringInputProcessor.
		/// </summary>
		public void Update() {
			if (deleteRecentKeys) {
				ResetRecentKeys();
			}
			if (inputManager.HasKeyBeenPressedContinuous(Keys.Delete) && CaretPosition < RecordedKeys.Length) {
				if (selectionStart == -1) {
					RecordedKeys = RecordedKeys.Remove(CaretPosition, 1);
					Updated = true;
				}
				else {
					RecordedKeys = RecordedKeys.Remove(selectionStart, selectionLength);
					if (CaretPosition > selectionStart) {
						if (CaretPosition < selectionStart + selectionLength) {
							CaretPosition = selectionStart;
						}
						else {
							CaretPosition -= selectionLength;
						}
					}
					Updated = true;
				}
			}
			deleteRecentKeys = true;
		}

		/// <summary>
		/// Starts recording.
		/// </summary>
		public void StartRecording() {
			IsRecording = true;
			IsPaused = false;
			RecordedKeys = "";
		}

		/// <summary>
		/// Pauses recording.
		/// </summary>
		/// <param name="text">The text recorded so far</param>
		/// <param name="outCaretPosition">The caret position at this moment</param>
		public void PauseRecording(out string text, out int outCaretPosition) {
			IsPaused = true;
			text = RecordedKeys;
			outCaretPosition = CaretPosition;
		}

		/// <summary>
		/// Resume the recording
		/// </summary>
		/// <param name="text">The text that was recorded when the StringInputProcessor was paused.</param>
		/// <param name="newCaretPosition"></param>
		public void UnpauseRecording(string text, int newCaretPosition) {
			IsPaused = false;
			
			RecordedKeys = text;
			CaretPosition = newCaretPosition;
		}

		/// <summary>
		/// Returns the characters written during the last update cycle.
		/// </summary>
		/// <returns></returns>
		public string GetRecentString() {
			return recentKeys;
		}

		/// <summary>
		/// Resets the characters written during the last update cycle.
		/// </summary>
		public void ResetRecentKeys() {
			recentKeys = "";
			deleteRecentKeys = false;
			Updated = false;
		}

		/// <summary>
		/// Sets the selection.
		/// </summary>
		public void SetSelection(int start, int length) {
			selectionStart = start;
			selectionLength = length;
		}

		/// <summary>
		/// Event handler called when a character has been entered.
		/// </summary>
		public void OnCharacterEntered(object sender, CharacterEventArgs eventArgs) {
			CharacterEntered(eventArgs.Character);
		}

		/// <summary>
		/// Updates the recent keys when a character is entered.
		/// </summary>
		/// <param name="character"></param>
		public void CharacterEntered(char character) {
			if (deleteRecentKeys) {
				ResetRecentKeys();
			}
			if (IsRecording && !IsPaused &&
				!inputManager.IsModifierPressed(Keys.LeftAlt) && !inputManager.IsModifierPressed(Keys.LeftControl)) {
				Updated = true;
				if (character == '\r') {
					RecordedKeys = RecordedKeys.Insert(CaretPosition, '\n'.ToString(CultureInfo.InvariantCulture));
					CaretPosition++;
					recentKeys += '\n';
				}
				else if (character == '\b') {
					if (selectionStart != -1) {
						RecordedKeys = RecordedKeys.Remove(selectionStart, selectionLength);
						if (CaretPosition > selectionStart) {
							if (CaretPosition < selectionStart + selectionLength) {
								CaretPosition = selectionStart;
							}
							else {
								CaretPosition -= selectionLength;
							}
						}
						recentKeys += '\b';
					}
					else if (CaretPosition != 0) {
						RecordedKeys = RecordedKeys.Remove(CaretPosition - 1, 1);
						CaretPosition--;
						recentKeys += '\b';
					}
				}
				else if (character == '\t') {
					RecordedKeys = RecordedKeys.Insert(CaretPosition, "  ");
					CaretPosition += 2;
					recentKeys += "  ";
				}
				else if (!char.IsControl(character)) {
					RecordedKeys = RecordedKeys.Insert(CaretPosition, character.ToString(CultureInfo.InvariantCulture));
					CaretPosition++;
					recentKeys += character;
				}
				else {
					Updated = false;
				}
				if (Updated) {
					selectionStart = -1;
				}
			}
		}
	}
}