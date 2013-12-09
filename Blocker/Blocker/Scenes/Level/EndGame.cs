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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EndGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Label congragulations;

        private bool complete;
        public bool Complete { get { return complete; } private set { complete = value; } }

        private enum EndGameState { FadeIn, Display }
        private EndGameState state = EndGameState.FadeIn;

        private ParticleController pc;
        private Texture2D particleTexture;
        private Timer timer;

        public EndGame(Game game, SpriteBatch spriteBatch)
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
            pc = new ParticleController(game, spriteBatch);
            particleTexture = game.Content.Load<Texture2D>("Particles\\Line");

            string message = "Congragulations!#You have beaten#all the levels in#Block3r";
            message = message.Replace("#", System.Environment.NewLine);

            SpriteFont font = game.Content.Load<SpriteFont>("Fonts\\Pericles28");
            congragulations = new Label(game, spriteBatch, new Rectangle(0, 0, 480, 800), font, message);

            base.Initialize();
        }

        private Vector2 RandomPosition()
        {
            Random r = new Random();
            return new Vector2 (r.Next(480), r.Next(800));
        }

        private Color RandomColor()
        {
            Random r = new Random();
            return new Color(
                (byte)r.Next(0, 255),
                (byte)r.Next(0, 255),
                (byte)r.Next(0, 255));
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Random r = new Random();
            if (timer == null)
            {
                pc.AddExplosion(particleTexture, RandomPosition(), 10, r.Next(3, 10), 1000, RandomColor(), gameTime);
                timer = new Timer(game, 1000);
            }

            // Monitor touches
            foreach (GestureSample gs in InputHandler.Instance.Taps())
            {
                Complete = true;
            }
            InputHandler.Instance.Clear();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                complete = true;
                return;
            }

            timer.Update(gameTime);
            pc.Update(gameTime);
            if (timer.IsDone())
            {
                pc.AddExplosion(particleTexture, RandomPosition(), 10, r.Next(3, 10), 1500, RandomColor(), gameTime);
                timer = new Timer(game, 1000);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            pc.Draw(gameTime);
            congragulations.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
