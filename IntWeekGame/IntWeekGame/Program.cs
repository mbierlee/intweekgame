using System;

namespace IntWeekGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (IntWeekGame game = new IntWeekGame())
            {
                game.Run();
            }
        }
    }
}

