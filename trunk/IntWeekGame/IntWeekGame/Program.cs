namespace IntWeekGame
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            using (var game = new IntWeekGame())
            {
                game.Run();
            }
        }
    }
}