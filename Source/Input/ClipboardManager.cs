using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace ArchGame.Input {
	/// <summary>
	/// Class that allows access to the system clipboard.
	/// </summary>
	public static class ClipboardManager {
		[DllImport("user32.dll")]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);
		[DllImport("user32.dll")]
		static extern bool EmptyClipboard();
		[DllImport("user32.dll")]
		static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
		[DllImport("user32.dll")]
		static extern IntPtr GetClipboardData(uint uFormat);
		[DllImport("user32.dll")]
		static extern bool CloseClipboard();

		const int TextFormat = 13;

		static IntPtr windowHandle;

		/// <summary>
		/// Intialize the ClipboardManager with the GameWindow handle.
		/// </summary>
		/// <param name="window">The window to initalize the ClipboardManager with</param>
		internal static void Initialize(GameWindow window) {
			windowHandle = window.Handle;
		}

		/// <summary>
		/// Save a string to the clipboard.
		/// </summary>
		/// <param name="value">The string to save</param>
		public static void SaveToClipboard(string value) {
			OpenClipboard(windowHandle);
			EmptyClipboard();
			SetClipboardData(TextFormat, Marshal.StringToHGlobalUni(value));

			CloseClipboard();
		}

		/// <summary>
		/// Get a string from the clipboard.
		/// </summary>
		/// <returns>The string currently on the clipboard</returns>
		public static string GetClipboard() {
			OpenClipboard(windowHandle);

			IntPtr resultHandle = GetClipboardData(TextFormat);
			string resultString = Marshal.PtrToStringUni(resultHandle);

			CloseClipboard();

			return resultString;
		}



	}

}