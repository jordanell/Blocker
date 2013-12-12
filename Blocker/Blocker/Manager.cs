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
using Blocker.Handlers;


namespace Blocker
{
    /// <summary>
    /// This is a game component that implements IUpdateable. Manager is used to control the
    /// flow of the game between different scenes.
    /// </summary>
    public class Manager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // XNA Components
        private Game game;
        private SpriteBatch spriteBatch;

        // Splash screen scene
        private SplashScreen splash;

        // Background to be used for whole game
        private Background background;

        // Main menu scene
        private Menu menu;

        // Level scene
        private Level level;

        // End of the game scene
        private EndGame endGame;

        // The states that the manager can be in
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
        /// to run. 
        /// </summary>
        public override void Initialize()
        {
            // Check for needed files
            FileHandler.CheckFiles();

            // Create the splash screen scene
            splash = new SplashScreen(game, spriteBatch, 3000);

            // Create the background entity
            background = new Background(game, spriteBatch);

            // Create the menu scene
            menu = new Menu(game, spriteBatch);

            base.Initialize();
        }

        /// <summary>
        /// Allows the Manager to update itself. Updates are dependant on the 
        /// current state of the Manager
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update the background when not in splash screen
            if (state != ManagerState.Splash)
                background.Update(gameTime);

            // Update based on state
            if (state == ManagerState.Splash)
            {
                splash.Update(gameTime);

                // Go to main menu
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

                // Go to level selected
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

                // Go to main menu
                if (level.Quit)
                {
                    menu = new Menu(game, spriteBatch);
                    level = null;
                    state = ManagerState.Menu;
                    return;
                }

                // Go to next level
                else if (level.Complete)
                    LevelComplete();
            }
            else if (state == ManagerState.EndGame)
            {
                endGame.Update(gameTime);

                // Go to main menu
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

        /// <summary>
        /// Performs a transistion from level to level. Save the highest level if needed and
        /// move onto next level or end game scene.
        /// </summary>
        private void LevelComplete()
        {
            // Save if highest level has been obtained
            if (level.LevelNumber > FileHandler.TopLevel())
                FileHandler.Save(level.LevelNumber);

            // Go to end game scene
            if (level.LevelNumber == 25)
            {
                level = null;
                endGame = new EndGame(game, spriteBatch);
                state = ManagerState.EndGame;
                SoundMixer.Instance(game).PlayWinner(false);
            }
            // Go to next level
            else
            {
                int nextLevel = level.LevelNumber + 1;
                level = null;
                level = new Level(game, spriteBatch, nextLevel);
                state = ManagerState.Level;
                SoundMixer.Instance(game).PlayLevelComplete(false);
            }
        }

        /// <summary>
        /// Draw based on the current state of the Manager.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the background if needed
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
