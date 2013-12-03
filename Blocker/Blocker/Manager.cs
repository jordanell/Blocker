using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;


namespace Blocker
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Manager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private SplashScreen splash;

        private Background background;

        private Menu menu;
        private Level level;

        private EndGame endGame;

        private enum ManagerState { Splash, Menu, Level, EndGame }
        private ManagerState state = ManagerState.Splash;

        public Manager(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            Initialize();
        }

        /// <summary>
        /// Allows the Manager component to perform initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // Check for needed files
            CheckFiles();

            // Create the splash screen
            splash = new SplashScreen(game, spriteBatch, 3000);

            // Create the background entity
            background = new Background(game, spriteBatch);

            // Create the menu entity
            menu = new Menu(game, spriteBatch);

            base.Initialize();
        }

        private void CheckFiles()
        {
            byte[] bytes = System.BitConverter.GetBytes(0);
            CreateFileIfDoesNotExist("completedLevels", bytes);

            bytes = System.BitConverter.GetBytes(1);
            CreateFileIfDoesNotExist("settings", bytes);
        }

        private void CreateFileIfDoesNotExist(string file, byte[] bytes)
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update the background
            if (state != ManagerState.Splash)
                background.Update(gameTime);

            // Update based on state
            if (state == ManagerState.Splash)
            {
                splash.Update(gameTime);

                if (splash.Complete)
                {
                    menu = new Menu(game, spriteBatch);
                    splash = null;
                    state = ManagerState.Menu;
                    return;
                }
            }
            else if (state == ManagerState.Menu)
            {
                menu.Update(gameTime);

                if (menu.LoadLevel)
                {
                    level = new Level(game, spriteBatch, menu.LevelNumber);
                    menu = null;
                    state = ManagerState.Level;
                    return;
                }
            }

            else if (state == ManagerState.Level)
            {
                level.Update(gameTime);

                if (level.Quit)
                {
                    menu = new Menu(game, spriteBatch);
                    level = null;
                    state = ManagerState.Menu;
                    return;
                }

                else if (level.Complete)
                {
                    LevelComplete();
                }
            }
            else if (state == ManagerState.EndGame)
            {
                endGame.Update(gameTime);

                if (endGame.Complete)
                {
                    menu = new Menu(game, spriteBatch);
                    splash = null;
                    state = ManagerState.Menu;
                    return;
                }
            }

            base.Update(gameTime);
        }

        private void LevelComplete()
        {
            Save(level.LevelNumber);

            if (level.LevelNumber == 25)
            {
                level = null;
                endGame = new EndGame(game, spriteBatch);
                state = ManagerState.EndGame;
            }
            else
            {
                int nextLevel = level.LevelNumber + 1;
                level = null;
                level = new Level(game, spriteBatch, nextLevel);
                state = ManagerState.Level;
            }
        }

        private void Save(int levelFinished)
        {
            IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;

            using (fs = gameStorage.CreateFile("completedLevels"))
            {
                if (fs != null)
                {
                    byte[] bytes = System.BitConverter.GetBytes(levelFinished);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the background
            if (state != ManagerState.Splash)
                background.Draw(gameTime);

            // Draw based on state
            if (state == ManagerState.Splash)
                splash.Draw(gameTime);
            else if (state == ManagerState.Menu)
                menu.Draw(gameTime);
            else if (state == ManagerState.Level)
                level.Draw(gameTime);
            else if (state == ManagerState.EndGame)
                endGame.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
