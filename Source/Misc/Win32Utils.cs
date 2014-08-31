using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace ArchGame.Misc {
	public static class Win32Utils {
		private const int SwShownormal = 1;
		private const int SwShowminimized = 2;
		private const int SwShowmaximized = 3;

		[DllImport("user32.dll")]
		private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
		public static void ChangeWindow(GameWindow window, WindowStyle windowStyle) {
			ShowWindowAsync(window.Handle, (int) windowStyle);
		} 

		public enum WindowStyle { Normal = 1, Minimized = 2, Maximized = 3 }
	}
}