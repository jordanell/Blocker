using System;

namespace Blocker
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BlockerGame game = new BlockerGame())
            {
                game.Run();
            }
        }
    }
#endif
}

