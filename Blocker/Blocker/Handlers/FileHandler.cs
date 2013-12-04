using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Blocker.Handlers
{
    class FileHandler
    {
        private static String level = "completedLevels";
        private static String settings = "settings";

        public static void CheckFiles()
        {
            byte[] bytes = System.BitConverter.GetBytes(0);
            CreateFileIfDoesNotExist(level, bytes);

            bytes = System.BitConverter.GetBytes(1);
            CreateFileIfDoesNotExist(settings, bytes);
        }

        private static void CreateFileIfDoesNotExist(string file, byte[] bytes)
        {
            IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;

            if (!gameStorage.FileExists(file))
            {
                using (fs = gameStorage.CreateFile(file))
                {
                    if (fs != null)
                        fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static int TopLevel()
        {
            int topLevel = 0;

            using (IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (gameStorage.FileExists(level))
                {
                    using (IsolatedStorageFileStream fs = gameStorage.OpenFile(level, System.IO.FileMode.Open))
                    {
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

        public static void Save(int levelFinished)
        {
            IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;

            using (fs = gameStorage.CreateFile(level))
            {
                if (fs != null)
                {
                    byte[] bytes = System.BitConverter.GetBytes(levelFinished);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static void ResetLevels()
        {
            IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;

            using (fs = gameStorage.CreateFile(level))
            {
                if (fs != null)
                {
                    byte[] bytes = System.BitConverter.GetBytes(0);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}
