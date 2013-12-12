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
    /// This is a game component that implements IUpdateable. Background is used throughout
    /// the game to render a parallax star field and a planet.
    /// </summary>
    public class Background : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Texture for blue stars
        private Texture2D staticBG;

        // Texture for moving stars
        private Texture2D stars;

        // Texture for planet
        private Texture2D planet;

        // Location for star scrolling
        private Vector2 scroll;

        public Background(Game game, SpriteBatch sb)
            : base(game)
        {
            this.game =  game;
            this.spriteBatch = sb;

            Initialize();
        }

        /// <summary>
        /// Allows the Background to perform initialization it needs to before starting
        /// to run. Load all the textures needed
        /// </summary>
        public override void Initialize()
        {
            staticBG = game.Content.Load<Texture2D>("Background\\StarsBlue");
            stars = game.Content.Load<Texture2D>("Background\\Stars");
            planet = game.Content.Load<Texture2D>("Background\\Planet");

            base.Initialize();
        }

        /// <summary>
        /// Allows the background to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update the parallax location
            scroll.X += 0.5f;
            scroll.Y += 0.5f;

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the background to draw itself
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the static background and planet
            spriteBatch.Begin();
            spriteBatch.Draw(staticBG, new Rectangle(0, 0, 480, 800), Color.White);
            spriteBatch.Draw(planet, new Rectangle(220, -50, 300, 300), Color.White);
            spriteBatch.End();

            // Draw the parallax stars at their scrolled location
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw(stars,
                    new Rectangle(0, 0, 480, 800),
                    new Rectangle((int)(-scroll.X), (int)(-scroll.Y), stars.Width, stars.Height),
                    Color.White * 0.3f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
