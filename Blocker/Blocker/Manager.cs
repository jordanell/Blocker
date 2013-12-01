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


namespace Blocker
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Manager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Background background;

        private Menu menu;
        private Level level;

        private enum ManagerState { Menu, Level }
        private ManagerState state = ManagerState.Menu;

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
            // Create the background entity
            background = new Background(game, spriteBatch);

            // Create the menu entity
            menu = new Menu(game, spriteBatch);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update the background
            background.Update(gameTime);

            // Update based on state
            if (state == ManagerState.Menu)
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
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the background
            background.Draw(gameTime);

            // Draw based on state
            if (state == ManagerState.Menu)
            {
                menu.Draw(gameTime);
            }

            else if (state == ManagerState.Level)
                level.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
