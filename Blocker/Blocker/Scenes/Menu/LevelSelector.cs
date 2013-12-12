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
    /// This is a game component that implements IUpdateable. Level selector is used for the player
    /// to pick from a list of available levels to play. It is part of the menu system.
    /// </summary>
    public class LevelSelector : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // List of buttons for levels
        private List<List<Button>> selector;

        // Load a level flag
        public bool LoadLevel { get; private set; }

        // Level number to load
        public int LevelNumber { get; private set; }

        public LevelSelector(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            Initialize();
        }

        /// <summary>
        /// Load textures needed for the selector as well as enable buttons as needed
        /// from the save file. 
        /// </summary>
        public override void Initialize()
        {
            // Load textures
            Texture2D blueButton = game.Content.Load<Texture2D>("Buttons\\BlueButton");
            SpriteFont menuFont = game.Content.Load<SpriteFont>("Fonts\\Pericles36");

            selector = new List<List<Button>>();

            // Get the top level the user has beaten
            int topLevel = FileHandler.TopLevel();

            // Create the buttons
            for (int y = 0; y <= 4; y++)
            {
                List<Button> row = new List<Button>();
                for (int x = 0; x < 5; x++)
                {
                    Button button = new Button(game, spriteBatch, 
                                               new Rectangle((30+(x*90)), (300+(y*90)), 60, 60), blueButton, menuFont,
                                               Convert.ToString((y * 5) + x + 1));
                    // Disable the button if the user has not beaten a high enough level
                    // to unlock it yet
                    if ((y * 5) + x + 1 > topLevel + 1)
                        button.enabled = false;
                    row.Add(button);
                }
                selector.Add(row);
            }

            base.Initialize();
        }

        /// <summary>
        /// Allows the level selector to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update all buttons
            for (int y = 0; y <= 4; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    (selector[y])[x].Update(gameTime);

                    // Check for a button press
                    if ((selector[y])[x].State == TouchButtonState.Clicked)
                    {
                        // Set load level flags
                        LoadLevel = true;
                        LevelNumber = ((y * 5) + x + 1);
                    }
                }
            }

            base.Update(gameTime);
        }
        
        /// <summary>
        /// Allows the level selector to draw itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw each button
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
