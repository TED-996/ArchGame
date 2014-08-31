using System.Globalization;
using Microsoft.Xna.Framework.Input;

namespace ArchGame.Input {
	public class StringInputProcessor {
		public bool IsRecording { get; private set; }
		public bool IsPaused { get; private set; }
		public int CaretPosition { get; set; }
		public string RecordedKeys { get; set; }

		public bool Updated { get; private set; }

		string recentKeys = "";

		readonly InputState inputState;

		bool deleteRecentKeys;

		int selectionStart;
		int selectionLength;

		public StringInputProcessor(InputState newInputState) {
			IsRecording = false;
			IsPaused = false;
			RecordedKeys = "";
			selectionLength = 1;
			selectionStart = -1;

			inputState = newInputState;

			EventInputManager.CharacterEntered += OnCharacterEntered;
		}

		public void Update() {
			if (deleteRecentKeys) {
				ResetRecentKeys();
			}
			if (inputState.HasKeyBeenPressedContinuous(Keys.Delete) && CaretPosition < RecordedKeys.Length) {
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

		public void StartRecording() {
			IsRecording = true;
			IsPaused = false;
			RecordedKeys = "";
		}

		public void PauseRecording(out string text, out int outCaretPosition) {
			IsPaused = true;
			text = RecordedKeys;
			outCaretPosition = CaretPosition;
		}

		public void UnpauseRecording(string text, int newCaretPosition) {
			IsPaused = false;
			
			RecordedKeys = text;
			CaretPosition = newCaretPosition;
		}

		public string GetRecentString() {
			return recentKeys;
		}

		public string GetAllString() {
			return RecordedKeys;
		}

		public void ResetRecentKeys() {
			recentKeys = "";
			deleteRecentKeys = false;
			Updated = false;
		}

		public void SetSelection(int start, int length) {
			selectionStart = start;
			selectionLength = length;
		}

		public void OnCharacterEntered(object sender, CharacterEventArgs eventArgs) {
			CharacterEntered(eventArgs.Character);
		}

		public void CharacterEntered(char character) {
			if (deleteRecentKeys) {
				ResetRecentKeys();
			}
			if (IsRecording && !IsPaused &&
				!inputState.IsModifierPressed(Keys.LeftAlt) && !inputState.IsModifierPressed(Keys.LeftControl)) {
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