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
    public class SplashScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Texture2D texture;
        private float alpha = 0.0f;

        private bool complete;
        public bool Complete { get { return complete; } private set { complete = value; } }

        private enum SplashState { FadeIn, Display, FadeOut }
        private SplashState state = SplashState.FadeIn;

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
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            texture = game.Content.Load<Texture2D>("Splash");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            if (touchCollection.Count > 0)
            {
                complete = true;
                return;
            }

            switch (state)
            {
                case SplashState.FadeIn:
                    alpha += 0.05f;
                    if (alpha >= 1)
                        state = SplashState.Display;
                    break;
                case SplashState.Display:
                    if (timer == null)
                        timer = new Timer(game, totalTime);
                    timer.Update(gameTime);
                    if (timer.IsDone())
                        state = SplashState.FadeOut; 
                    break;
                case SplashState.FadeOut:
                    alpha -= 0.05f;
                    if (alpha <= 0)
                        complete = true;
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Vector2.Zero, Color.White * alpha);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
