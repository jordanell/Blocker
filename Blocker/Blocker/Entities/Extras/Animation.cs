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
using Blocker.Entities;


namespace Blocker
{
    /// <summary>
    /// This is a game component that implements IUpdateable. Animation is used to play
    /// a series of textures in order at a given speed to create the illusion of
    /// animation.
    /// </summary>
    public class Animation : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Entity to animate (block animations are used for game components 
        // which are part of the grid like space shit and matter)
        private Entity entity;

        // The textures that will be used for animation
        private List<Texture2D> slides;

        // Current texture number
        private int slide = 0;

        // Frames to switch texture on
        private int speed;

        // Frame count between texture switches
        private int count = 0;

        public Animation(Game game, SpriteBatch spriteBatch, Entity entity, List<Texture2D> slides, int speed)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.entity = entity;
            this.slides = slides;
            this.speed = speed;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  Set the start of the animation to a random number between 0 and the number of textures.
        /// </summary>
        public override void Initialize()
        {
            Random rnd = new Random();
            count = rnd.Next(0, slides.Count);

            base.Initialize();
        }

        /// <summary>
        /// Allows the Animation to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Set the animation frame
            count++;
            if (count % speed == 0)
            {
                slide++;
                if (slide == slides.Count)
                    slide = 0;
                count = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the Animation to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw based on block or played animation
            spriteBatch.Begin();
            if (entity != null)
                spriteBatch.Draw(slides[slide], entity.Position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
