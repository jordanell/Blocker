using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Blocker.Handlers
{
    /// <summary>
    /// FileHandler is a class which can staticly handle all file level requests
    /// that need to be made by Block3r.
    /// </summary>
    class FileHandler
    {
        // File names to be used
        private static String level = "completedLevels";
        private static String settings = "settings";

        /// <summary>
        /// Attempts to check whether the settings and levels files exist. Creates new
        /// files if needed.
        /// </summary>
        public static void CheckFiles()
        {
            // Check level file
            byte[] bytes = System.BitConverter.GetBytes(0);
            CreateFileIfDoesNotExist(level, bytes);

            // Check settings file
            bytes = System.BitConverter.GetBytes(1);
            CreateFileIfDoesNotExist(settings, bytes);
        }

        /// <summary>
        /// Checks to see where a given file exists. If the files does not exist,
        /// it creates a new one and writes in the given bytes.
        /// </summary>
        /// <param name="file">File to the checked or added</param>
        /// <param name="bytes">Input to write in new file</param>
        private static void CreateFileIfDoesNotExist(string file, byte[] bytes)
        {
            // File storage initialization
            IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;

            // Check if file exists
            if (!gameStorage.FileExists(file))
            {
                using (fs = gameStorage.CreateFile(file))
                {
                    // Write new bytes
                    if (fs != null)
                        fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// Returns the top level saved in the level file.
        /// </summary>
        /// <returns></returns>
        public static int TopLevel()
        {
            int topLevel = 0;

            // Storage initialization
            using (IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (gameStorage.FileExists(level))
                {
                    using (IsolatedStorageFileStream fs = gameStorage.OpenFile(level, System.IO.FileMode.Open))
                    {
                        // Get the top level
                        if (fs != null)
                        {
                            byte[] bytes = new byte[10];
                            int x = fs.Read(bytes, 0, 10);
                            if (x > 0)
                                topLevel = System.BitConverter.ToInt32(bytes, 0);
                        }
                    }
                }
            }

            return topLevel;
        }

        /// <summary>
        /// Saves a level to the level file. 
        /// </summary>
        /// <param name="levelFinished">The level number that was completed</param>
        public static void Save(int levelFinished)
        {
            // Storage initialization
            IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;

            using (fs = gameStorage.CreateFile(level))
            {
                // Write bytes out to file
                if (fs != null)
                {
                    byte[] bytes = System.BitConverter.GetBytes(levelFinished);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// Sets the max level back to 0.
        /// </summary>
        public static void ResetLevels()
        {
            FileHandler.Save(0);
        }

        /// <summary>
        /// Returns a 1 if sound is enabled. Returns a 0 if disabled. Returns a
        /// -1 if there was an error reading the settings file.
        /// </summary>
        /// <returns>The number to return.</returns>
        public static int SoundEnabled()
        {
            // Open settings file
            using (IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (gameStorage.FileExists(settings))
                {
                    using (IsolatedStorageFileStream fs = gameStorage.OpenFile(settings, System.IO.FileMode.Open))
                    {
                        if (fs != null)
                        {
                            byte[] bytes = new byte[10];
                            int x = fs.Read(bytes, 0, 10);
                            return System.BitConverter.ToInt32(bytes, 0);
                        }
                    }
                }
            }

            // There was an error reading the settings file
            return -1;
        }

        /// <summary>
        /// Save the settings file to indicate that game has been
        /// muted or not.
        /// </summary>
        /// <param name="muted">Flag for game mute. True will mute the game.</param>
        public static void SaveSettings(bool muted)
        {
            // Initialize storage
            IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;

            // Sabe settings file
            using (fs = gameStorage.CreateFile(settings))
            {
                if (fs != null)
                {
                    // Save new setting
                    byte[] bytes;
                    if (muted)
                        bytes = System.BitConverter.GetBytes(1);
                    else
                        bytes = System.BitConverter.GetBytes(0);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}
