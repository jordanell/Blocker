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
    public class HeadsUpDisplay : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        // Fuel
        private Label fuel;
        private int fuelLeft;

        // Matter
        private Texture2D redMatter;
        private Texture2D blueMatter;
        private Label red;
        private int redCount = 0;
        private Label blue;
        private int blueCount = 0;

        // Reset
        private Button resetButton;

        // Exit
        private Button exitButton;

        public HeadsUpDisplay(Game game, SpriteBatch spriteBatch)
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
            Texture2D greenButton = game.Content.Load<Texture2D>("Buttons\\GreenButton");
            Texture2D redButton = game.Content.Load<Texture2D>("Buttons\\RedButton");

            redMatter = game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter1");
            blueMatter = game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter1");

            SpriteFont hudFont = game.Content.Load<SpriteFont>("Fonts\\Pericles28");
            SpriteFont hudFont2 = game.Content.Load<SpriteFont>("Fonts\\Pericles24");
            SpriteFont hudFont3 = game.Content.Load<SpriteFont>("Fonts\\Pericles18");

            resetButton = new Button(game, spriteBatch, new Rectangle(250, 10, 105, 60), greenButton, hudFont2, "Reset");
            resetButton.Initialize();

            exitButton = new Button(game, spriteBatch, new Rectangle(365, 10, 105, 60), redButton, hudFont, "Exit");
            exitButton.Initialize();

            fuel = new Label(game, spriteBatch, new Rectangle(10, 10, 115, 25), hudFont3, "Fuel: " + Convert.ToString(fuelLeft));
            fuel.Initialize();

            red = new Label(game, spriteBatch, new Rectangle(45, 45, 40, 25), hudFont3, "0");
            red.Initialize();

            blue = new Label(game, spriteBatch, new Rectangle(125, 45, 40, 25), hudFont3, "0");
            blue.Initialize();

            base.Initialize();
        }

        public void resetHud(int fuel)
        {
            this.fuelLeft = fuel;
            red.setText("0");
            blue.setText("0");
        }

        public void DecreaseFuel()
        {
            this.fuelLeft--;
            this.fuel.setText("Fuel: " + "Fuel: " + Convert.ToString(fuelLeft));
        }

        public void AddRedMatter()
        {
            redCount++;
            red.setText(Convert.ToString(redCount));
        }

        public void AddBlueMatter()
        {
            blueCount++;
            blue.setText(Convert.ToString(blueCount));
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

        public override void Draw(GameTime gameTime)
        {
            resetButton.Draw(gameTime);
            exitButton.Draw(gameTime);
            fuel.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(redMatter, new Rectangle(10, 45, 30, 30), Color.White);
            spriteBatch.Draw(blueMatter, new Rectangle(90, 45, 30, 30), Color.White);
            spriteBatch.End();

            red.Draw(gameTime);
            blue.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
