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


namespace Blocker.Particle_System
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Particle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        public Texture2D Texture { get; set; }

        public int BirthTime { get; set; }
        public int MaxAge { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        public float Scale { get; set; }
        public Color Color { get; set; }
        private float alpha;

        public Particle(Game game, SpriteBatch spriteBatch, Texture2D texture, GameTime gameTime)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            this.Texture = texture;

            this.BirthTime = (int)gameTime.TotalGameTime.TotalMilliseconds;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public bool IsDead(GameTime gameTime)
        {
            return ((BirthTime + MaxAge) < gameTime.TotalGameTime.TotalMilliseconds);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            alpha = 1f - ((float)gameTime.TotalGameTime.TotalMilliseconds - (float)BirthTime) / ((float)MaxAge);
            Position += (Velocity * alpha);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color * alpha, Angle, origin, Scale, SpriteEffects.None, 0f); 
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
