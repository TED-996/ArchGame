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

		/// <summary>
		/// Change the window style (normal, minimized, maximized)
		/// </summary>
		/// <param name="window">The window to change</param>
		/// <param name="windowStyle">The style to apply</param>
		public static void ChangeWindowSize(GameWindow window, WindowStyle windowStyle) {
			ShowWindowAsync(window.Handle, (int) windowStyle);
		} 

		public enum WindowStyle { Normal = 1, Minimized = 2, Maximized = 3 }
	}
}