using System;

namespace Test {
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
			//Start the game the XNA way.
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

