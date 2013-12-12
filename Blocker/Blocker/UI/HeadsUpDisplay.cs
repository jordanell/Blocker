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
using Microsoft.Xna.Framework.Input.Touch;


namespace Blocker
{
    /// <summary>
    /// This is a game component that implements IUpdateable. The Heads Up Display (HUD) is used
    /// inside of levels to give meta information to the player as well as provide reset and
    /// exit funtionality.
    /// </summary>
    public class HeadsUpDisplay : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // XNA components
        private Game game;
        private SpriteBatch spriteBatch;

        // Label for level number
        private Label levelLabel;

        // Level number
        private int level;
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                levelLabel.Text = "Level: " + Convert.ToString(value);
            } 
        }

        // Label for fuel indication
        private Label fuelLabel;

        // Amount of fuel left
        private int fuel;
        public int Fuel 
        {
            get { return fuel; }
            set 
            { 
                fuel = value;
                fuelLabel.Text = "Fuel: " + Convert.ToString(value);
            } 
        }

        // Matter textures
        private Texture2D redMatterTexture;
        private Texture2D blueMatterTexture;

        // Red matter label
        private Label redMatterLabel;

        // Red matter number
        private int redMatter = 0;
        public int RedMatter 
        {
            get { return redMatter; }
            private set 
            { 
                redMatter = value;
                redMatterLabel.Text = Convert.ToString(value);
            }
        }

        // Blue matter labels
        private Label blueMatterLabel;

        // Blue matter number
        private int blueMatter = 0;
        public int BlueMatter 
        {
            get { return blueMatter; }
            private set 
            { 
                blueMatter = value;
                blueMatterLabel.Text = Convert.ToString(value);
            }
        }

        // Reset level button
        public Button resetButton;

        // Reset flag
        public bool Reset
        {
            get { return (resetButton.State == TouchButtonState.Clicked); }
        }

        // Exit level button
        public Button exitButton;

        // Exit flag
        public bool Exit
        {
            get { return (exitButton.State == TouchButtonState.Clicked); }
        }

        public HeadsUpDisplay(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  Initialized all textures needed, all fonts needed, all labels needed, 
        /// and all buttons needed.
        /// </summary>
        public override void Initialize()
        {
            // Load button textures
            Texture2D greenButton = game.Content.Load<Texture2D>("Buttons\\GreenButton");
            Texture2D redButton = game.Content.Load<Texture2D>("Buttons\\RedButton");

            // Load matter textures
            redMatterTexture = game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter1");
            blueMatterTexture = game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter1");

            // Load fonts for labels
            SpriteFont hudFont = game.Content.Load<SpriteFont>("Fonts\\Pericles28");
            SpriteFont hudFont2 = game.Content.Load<SpriteFont>("Fonts\\Pericles24");
            SpriteFont hudFont3 = game.Content.Load<SpriteFont>("Fonts\\Pericles18");

            // Create reset button
            resetButton = new Button(game, spriteBatch, new Rectangle(250, 10, 105, 60), greenButton, hudFont2, "Reset");

            // Create exit button
            exitButton = new Button(game, spriteBatch, new Rectangle(365, 10, 105, 60), redButton, hudFont, "Exit");

            // Create fuel label
            fuelLabel = new Label(game, spriteBatch, new Rectangle(10, 10, 115, 25), hudFont3, "Fuel: " + Convert.ToString(Fuel));
            fuelLabel.TextPosition = Label.LabelPosition.Left;

            // Create level label
            levelLabel = new Label(game, spriteBatch, new Rectangle(125, 10, 115, 25), hudFont3, "Level: " + Convert.ToString(Level));
            levelLabel.TextPosition = Label.LabelPosition.Left;

            // Create matter labels
            blueMatterLabel = new Label(game, spriteBatch, new Rectangle(25, 45, 40, 25), hudFont3, "0");
            redMatterLabel = new Label(game, spriteBatch, new Rectangle(98, 45, 40, 25), hudFont3, "0");

            base.Initialize();
        }

        /// <summary>
        /// Resets the HUD with the amount of fuel given. All matter is set back to 0.
        /// </summary>
        /// <param name="fuel"></param>
        public void ResetHud(int fuel)
        {
            this.Fuel = fuel;
            RedMatter = 0;
            BlueMatter = 0;
        }

        /// <summary>
        /// Decreases the fuel in the HUD by 1. Turns the fuel label red if player 
        /// is out of fuel.
        /// </summary>
        public void DecreaseFuel()
        {
            Fuel--;
            if (Fuel == 0)
                fuelLabel.TextColor = Color.Red;
        }

        /// <summary>
        /// Adds 1 to red matter count.
        /// </summary>
        public void AddRedMatter()
        {
            RedMatter++;
        }

        /// <summary>
        /// Adds 1 to blue matter count.
        /// </summary>
        public void AddBlueMatter()
        {
            BlueMatter++;
        }

        /// <summary>
        /// Removes 1 from red matter count.
        /// </summary>
        public void DecreaseRedMatter()
        {
            RedMatter--;
        }

        /// <summary>
        /// Removes 1 from blue matter count.
        /// </summary>
        public void DecreaseBlueMatter()
        {
            BlueMatter--;
        }

        /// <summary>
        /// Allows the HUD to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update the buttons
            resetButton.Update(gameTime);
            exitButton.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the HUD to draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw buttons
            resetButton.Draw(gameTime);
            exitButton.Draw(gameTime);
            
            // Draw fuel and level labels
            fuelLabel.Draw(gameTime);
            levelLabel.Draw(gameTime);

            // Draw matter textures
            spriteBatch.Begin();
            spriteBatch.Draw(blueMatterTexture, new Rectangle(2, 36, 40, 40), Color.White);
            spriteBatch.Draw(redMatterTexture, new Rectangle(75, 36, 40, 40), Color.White);
            spriteBatch.End();

            // Draw matter labels
            redMatterLabel.Draw(gameTime);
            blueMatterLabel.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
