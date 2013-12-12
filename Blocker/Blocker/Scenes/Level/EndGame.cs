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
using Blocker.Particle_System;


namespace Blocker
{
    /// <summary>
    /// The end game object is a single scene used to display a congragulatory message
    /// once a player beats level 25.
    /// </summary>
    public class EndGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Label for cograts text
        private Label congragulations;

        // Scene complete flag
        public bool Complete { get; private set; }

        // Particle controller and texture
        private ParticleController pc;
        private Texture2D particleTexture;

        // Timer for explosions
        private Timer timer;

        public EndGame(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            Initialize();
        }

        /// <summary>
        /// Creates the particle controller, loads textures needed, loads font and text needed as well.
        /// </summary>
        public override void Initialize()
        {
            // Create particle controller and load particle texture
            pc = new ParticleController(game, spriteBatch);
            particleTexture = game.Content.Load<Texture2D>("Particles\\Line");

            // Create congrats string
            string message = "Congragulations!#You have beaten#all the levels in#Block3r";
            message = message.Replace("#", System.Environment.NewLine);

            // Create congrats label
            SpriteFont font = game.Content.Load<SpriteFont>("Fonts\\Pericles28");
            congragulations = new Label(game, spriteBatch, new Rectangle(0, 0, 480, 800), font, message);

            base.Initialize();
        }

        /// <summary>
        /// Returns a random position on the screen
        /// </summary>
        /// <returns>The position to return</returns>
        private Vector2 RandomPosition()
        {
            Random r = new Random();
            return new Vector2 (r.Next(480), r.Next(800));
        }

        /// <summary>
        /// Returns a random RGB color.
        /// </summary>
        /// <returns>The color to be returned</returns>
        private Color RandomColor()
        {
            Random r = new Random();
            return new Color(
                (byte)r.Next(0, 255),
                (byte)r.Next(0, 255),
                (byte)r.Next(0, 255));
        }

        /// <summary>
        /// Allows the end game to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Random r = new Random();

            // Create timer and first explosion on first update
            if (timer == null)
            {
                pc.AddExplosion(particleTexture, RandomPosition(), 10, r.Next(3, 10), 1000, RandomColor(), gameTime);
                timer = new Timer(game, 1000);
            }

            // Monitor touches to complete the scene
            foreach (GestureSample gs in InputHandler.Instance.Taps())
                Complete = true;

            // Clear input every frame
            InputHandler.Instance.Clear();

            // Monitor the back button
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Complete = true;
                return;
            }

            // Update time and particles
            timer.Update(gameTime);
            pc.Update(gameTime);

            // Add explosions and reset timer when timer is up
            if (timer.IsDone())
            {
                pc.AddExplosion(particleTexture, RandomPosition(), 10, r.Next(3, 10), 1500, RandomColor(), gameTime);
                timer = new Timer(game, 1000);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the end game to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw componenets
            pc.Draw(gameTime);
            congragulations.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
