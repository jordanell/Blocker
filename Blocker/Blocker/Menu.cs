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
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private enum State { main, levelSelect, settings };
        private State state = State.main;

        private Texture2D title;

        // Buttons
        private Button startButton;
        private Button levelSelectButton;
        private Button settingsButton;

        public Menu(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            title = game.Content.Load<Texture2D>("Logo");

            Texture2D redBlock = game.Content.Load<Texture2D>("RedBlock");
            Texture2D blueBlock = game.Content.Load<Texture2D>("BlueBlock");

            SpriteFont menuFont = game.Content.Load<SpriteFont>("Fonts\\Pericles36");
            SpriteFont menuFont2 = game.Content.Load<SpriteFont>("Fonts\\Pericles28");

            startButton = new Button(game, spriteBatch, new Rectangle(103, 375, 275, 70), blueBlock, menuFont, "Play");
            startButton.Initialize();

            levelSelectButton = new Button(game, spriteBatch, new Rectangle(103, 475, 275, 70), blueBlock, menuFont2, "Level Select");
            levelSelectButton.Initialize();

            settingsButton = new Button(game, spriteBatch, new Rectangle(103, 575, 275, 70), redBlock, menuFont, "Settings");
            settingsButton.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the static title
            spriteBatch.Begin();
            spriteBatch.Draw(title, new Vector2(88, 190), Color.White);
            spriteBatch.End();

            if (state == State.main)
            {
                startButton.Draw(gameTime);
                levelSelectButton.Draw(gameTime);
                settingsButton.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
