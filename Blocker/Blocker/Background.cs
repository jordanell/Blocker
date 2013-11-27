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
    public class Background : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Texture2D staticBG;
        private Texture2D stars;
        private Texture2D planet;

        private Vector2 scroll;

        public Background(Game game, SpriteBatch sb)
            : base(game)
        {
            this.game =  game;
            this.spriteBatch = sb;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            staticBG = game.Content.Load<Texture2D>("Background\\StarsBlue");
            stars = game.Content.Load<Texture2D>("Background\\Stars");
            planet = game.Content.Load<Texture2D>("Background\\Planet");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
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
            // Draw the static background
            spriteBatch.Begin();
            spriteBatch.Draw(staticBG, new Rectangle(0, 0, 480, 800), Color.White);
            spriteBatch.Draw(planet, new Rectangle(220, -50, 300, 300), Color.White);
            spriteBatch.End();

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
