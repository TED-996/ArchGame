using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace ArchGame.Misc {
	/// <summary>
	/// Static class with methods working with the Win32 API
	/// </summary>
	public static class Win32Utils {
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

		/// <summary>
		/// Represents the way a window should appear.
		/// </summary>
		public enum WindowStyle {
			/// <summary>
			/// The window should not be maximized; it should fill as much space as it requests.
			/// </summary>
			Normal = 1,
			/// <summary>
			/// The window should be minimized to the taskbar.
			/// </summary>
			Minimized = 2, 
			/// <summary>
			/// The window should fill the entire desktop.
			/// </summary>
			Maximized = 3
		}
	}
}