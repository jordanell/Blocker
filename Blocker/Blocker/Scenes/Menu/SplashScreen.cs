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
    /// This is a game component that implements IUpdateable. The splash screen is used for game start
    /// up to display the company logo.
    /// </summary>
    public class SplashScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Texture for screen
        private Texture2D texture;

        // Alpha used for fading the screen
        private float alpha = 0.0f;

        public bool Complete { get; set; }

        // The possible states of the splash screen
        private enum SplashState { FadeIn, Display, FadeOut }

        // The state of the splash screen
        private SplashState state = SplashState.FadeIn;

        // Timing objects
        private int totalTime;
        private Timer timer;

        public SplashScreen(Game game, SpriteBatch spriteBatch, int time)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.totalTime = time;

            Initialize();
        }

        /// <summary>
        /// Load the texture needed for the splash screen.
        /// </summary>
        public override void Initialize()
        {
            // Load the texture
            texture = game.Content.Load<Texture2D>("Splash");

            base.Initialize();
        }

        /// <summary>
        /// Allows the splash screen to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // If there is any touch or gesture on the screen, complete the splash screen
            foreach (GestureSample gs in InputHandler.Instance.Taps())
            {
                InputHandler.Instance.Clear();
                Complete = true;
            }

            // Update the splash screen for alpha fading
            switch (state)
            {
                // Fade in the screen
                case SplashState.FadeIn:
                    alpha += 0.05f;
                    if (alpha >= 1)
                        state = SplashState.Display;
                    break;
                // Display normally 
                case SplashState.Display:
                    // Create and update timer
                    if (timer == null)
                        timer = new Timer(game, totalTime);
                    timer.Update(gameTime);

                    // Time to fade out?
                    if (timer.IsDone())
                        state = SplashState.FadeOut; 
                    break;
                // Fade out screen
                case SplashState.FadeOut:
                    alpha -= 0.05f;
                    if (alpha <= 0)
                        Complete = true;
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the splash screen to draw itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the texture with appropriate alpha
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Vector2.Zero, Color.White * alpha);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
