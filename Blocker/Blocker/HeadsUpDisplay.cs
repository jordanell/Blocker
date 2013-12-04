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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HeadsUpDisplay : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        // Level
        private Label levelLabel;
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

        // Fuel
        private Label fuelLabel;
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

        // Matter
        private Texture2D redMatterTexture;
        private Texture2D blueMatterTexture;
        private Label redMatterLabel;
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
        private Label blueMatterLabel;
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

        // Buttons
        public Button resetButton;
        private bool reset = false;
        public bool Reset
        {
            get { return (resetButton.state == TouchButtonState.Clicked); }
        }
        public Button exitButton;
        public bool Exit
        {
            get { return (exitButton.state == TouchButtonState.Clicked); }
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
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Texture2D greenButton = game.Content.Load<Texture2D>("Buttons\\GreenButton");
            Texture2D redButton = game.Content.Load<Texture2D>("Buttons\\RedButton");

            redMatterTexture = game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter1");
            blueMatterTexture = game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter1");

            SpriteFont hudFont = game.Content.Load<SpriteFont>("Fonts\\Pericles28");
            SpriteFont hudFont2 = game.Content.Load<SpriteFont>("Fonts\\Pericles24");
            SpriteFont hudFont3 = game.Content.Load<SpriteFont>("Fonts\\Pericles18");

            resetButton = new Button(game, spriteBatch, new Rectangle(250, 10, 105, 60), greenButton, hudFont2, "Reset");
            resetButton.Initialize();

            exitButton = new Button(game, spriteBatch, new Rectangle(365, 10, 105, 60), redButton, hudFont, "Exit");
            exitButton.Initialize();

            fuelLabel = new Label(game, spriteBatch, new Rectangle(10, 10, 115, 25), hudFont3, "Fuel: " + Convert.ToString(Fuel));
            fuelLabel.TextPosition = Label.LabelPosition.Left;

            levelLabel = new Label(game, spriteBatch, new Rectangle(125, 10, 115, 25), hudFont3, "Level: " + Convert.ToString(Level));
            levelLabel.TextPosition = Label.LabelPosition.Left;

            blueMatterLabel = new Label(game, spriteBatch, new Rectangle(25, 45, 40, 25), hudFont3, "0");
            redMatterLabel = new Label(game, spriteBatch, new Rectangle(98, 45, 40, 25), hudFont3, "0");

            base.Initialize();
        }

        public void ResetHud(int fuel)
        {
            this.Fuel = fuel;
            RedMatter = 0;
            BlueMatter = 0;
        }

        public void DecreaseFuel()
        {
            Fuel--;
            if (Fuel == 0)
                fuelLabel.TextColor = Color.Red;
        }

        public void AddRedMatter()
        {
            RedMatter++;
        }

        public void AddBlueMatter()
        {
            BlueMatter++;
        }

        public void DecreaseRedMatter()
        {
            RedMatter--;
        }

        public void DecreaseBlueMatter()
        {
            BlueMatter--;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            resetButton.Update(gameTime);
            exitButton.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            resetButton.Draw(gameTime);
            exitButton.Draw(gameTime);
            fuelLabel.Draw(gameTime);
            levelLabel.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(blueMatterTexture, new Rectangle(2, 36, 40, 40), Color.White);
            spriteBatch.Draw(redMatterTexture, new Rectangle(75, 36, 40, 40), Color.White);
            spriteBatch.End();

            redMatterLabel.Draw(gameTime);
            blueMatterLabel.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
