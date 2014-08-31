using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace ArchGame.Input {
	public static class EventInputManager {
		/// <summary>
		/// Event raised when a character has been entered.
		/// </summary>
		public static event CharEnteredHandler CharacterEntered;

		delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		static bool initialized;
		static IntPtr prevWndProc;
		static WndProc hookProcDelegate;
		// ReSharper disable InconsistentNaming
		static IntPtr hIMC;


		//various Win32 constants that we need
		const int GWL_WNDPROC = -4;
		const int WM_KEYDOWN = 0x100;
		const int WM_KEYUP = 0x101;
		const int WM_CHAR = 0x102;
		const int WM_IME_SETCONTEXT = 0x0281;
		const int WM_INPUTLANGCHANGE = 0x51;
		const int WM_GETDLGCODE = 0x87;
		const int DLGC_WANTALLKEYS = 4;


		//Win32 functions that we're using
		[DllImport("Imm32.dll")]
		static extern IntPtr ImmGetContext(IntPtr hWnd);

		[DllImport("Imm32.dll")]
		static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

		[DllImport("user32.dll")]
		static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
		// ReSharper restore InconsistentNaming

		/// <summary>
		/// Initialize the TextInput with the given GameWindow.
		/// </summary>
		/// <param name="window">The XNA window to which text input should be linked.</param>
		public static void Initialize(GameWindow window) {
			if (initialized)
				throw new InvalidOperationException("TextInput.Initialize can only be called once!");

			hookProcDelegate = HookProc;
			prevWndProc = (IntPtr)SetWindowLong(window.Handle, GWL_WNDPROC,
				(int)Marshal.GetFunctionPointerForDelegate(hookProcDelegate));

			hIMC = ImmGetContext(window.Handle);
			initialized = true;
		}

		static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) {
			IntPtr returnCode = CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);

			switch (msg) {
				case WM_GETDLGCODE:
					returnCode = (IntPtr)(returnCode.ToInt32() | DLGC_WANTALLKEYS);
					break;

				case WM_KEYDOWN:
					break;

				case WM_KEYUP:
					break;

				case WM_CHAR:
					if (CharacterEntered != null)
						CharacterEntered(null, new CharacterEventArgs((char)wParam));
					break;

				case WM_IME_SETCONTEXT:
					if (wParam.ToInt32() == 1)
						ImmAssociateContext(hWnd, hIMC);
					break;

				case WM_INPUTLANGCHANGE:
					ImmAssociateContext(hWnd, hIMC);
					returnCode = (IntPtr)1;
					break;
			}

			return returnCode;
		}
	}


	public class CharacterEventArgs :EventArgs {
		private readonly char character;

		public CharacterEventArgs(char character) {
			this.character = character;
		}

		public char Character {
			get { return character; }
		}
	}

	public delegate void CharEnteredHandler(object sender, CharacterEventArgs e);
}