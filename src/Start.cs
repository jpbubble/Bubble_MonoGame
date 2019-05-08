using System;

namespace Bubble
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program_Start
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Bubble_MonoGame_Init())
                game.Run();
        }
    }
}
