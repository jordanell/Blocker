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
    public class Animation : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Block block;

        private List<Texture2D> slides;
        private int slide = 0;

        private int speed;
        private int count = 0;

        public Animation(Game game, SpriteBatch spriteBatch, Block block, List<Texture2D> slides, int speed)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.block = block;
            this.slides = slides;
            this.speed = speed;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Random rnd = new Random();
            count = rnd.Next(0, 4);

            base.Initialize();
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
            count++;
            if (count % speed == 0)
            {
                slide++;
                if (slide == slides.Count)
                    slide = 0;
                count = 0;
            }

            spriteBatch.Begin();
            spriteBatch.Draw(slides[slide], block.GetPosition(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
