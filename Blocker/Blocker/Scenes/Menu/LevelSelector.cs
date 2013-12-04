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
using Microsoft.Xna.Framework.Input.Touch;
using Blocker.Handlers;


namespace Blocker
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LevelSelector : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private List<List<Button>> selector;

        public bool LoadLevel { get; private set; }
        public int LevelNumber { get; private set; }

        public LevelSelector(Game game, SpriteBatch spriteBatch)
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
            Texture2D blueButton = game.Content.Load<Texture2D>("Buttons\\BlueButton");
            SpriteFont menuFont = game.Content.Load<SpriteFont>("Fonts\\Pericles36");

            selector = new List<List<Button>>();

            int topLevel = FileHandler.TopLevel();

            for (int y = 0; y <= 4; y++)
            {
                List<Button> row = new List<Button>();
                for (int x = 0; x < 5; x++)
                {
                    Button button = new Button(game, spriteBatch, 
                                               new Rectangle((30+(x*90)), (300+(y*90)), 60, 60), blueButton, menuFont,
                                               Convert.ToString((y * 5) + x + 1));
                    if ((y * 5) + x + 1 > topLevel + 1)
                        button.enabled = false;
                    row.Add(button);
                }
                selector.Add(row);
            }

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int y = 0; y <= 4; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    (selector[y])[x].Update(gameTime);
                    if ((selector[y])[x].state == TouchButtonState.Clicked)
                    {
                        LoadLevel = true;
                        LevelNumber = ((y * 5) + x + 1);
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(List<Button> row in selector)
            {
                foreach(Button level in row)
                {
                    level.Draw(gameTime);
                }
            }
        }
    }
}
