using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace ArchGame.Input {
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

		public static void Initialize(GameWindow window) {
			windowHandle = window.Handle;
		}

		public static void SaveToClipboard(string value) {
			OpenClipboard(windowHandle);
			EmptyClipboard();
			SetClipboardData(TextFormat, Marshal.StringToHGlobalUni(value));

			CloseClipboard();
		}

		public static string GetClipboard() {
			OpenClipboard(windowHandle);

			IntPtr resultHandle = GetClipboardData(TextFormat);
			string resultString = Marshal.PtrToStringUni(resultHandle);

			CloseClipboard();

			return resultString;
		}



	}

}