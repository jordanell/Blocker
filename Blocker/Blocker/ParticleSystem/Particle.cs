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
    /// This is a game component that implements IUpdateable. The particle class is used to
    /// create the rectangular particles seen in the end game scene.
    /// </summary>
    public class Particle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // The texture for the particle
        public Texture2D Texture { get; set; }

        // Age properties
        public int BirthTime { get; set; }
        public int MaxAge { get; set; }

        // Physics properties
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        // The scale of the particle
        public float Scale { get; set; }

        // Color properties
        public Color Color { get; set; }
        private float alpha;

        public Particle(Game game, SpriteBatch spriteBatch, Texture2D texture, GameTime gameTime)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            this.Texture = texture;

            // Set the birth time of the particle
            this.BirthTime = (int)gameTime.TotalGameTime.TotalMilliseconds;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Returns true if the particle is considered dead.
        /// </summary>
        /// <param name="gameTime">The time the game has been running</param>
        /// <returns>The flag to return with.</returns>
        public bool IsDead(GameTime gameTime)
        {
            return ((BirthTime + MaxAge) < gameTime.TotalGameTime.TotalMilliseconds);
        }

        /// <summary>
        /// Allows the particle to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Fade the particle out as it dies
            alpha = 1f - ((float)gameTime.TotalGameTime.TotalMilliseconds - (float)BirthTime) / ((float)MaxAge);

            // Update physics
            // Alpha is used here to indicate its life left
            Position += (Velocity * alpha);

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the particle to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Need these properties for rotation to work properly
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            // Draw the particle
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color * alpha, Angle, origin, Scale, SpriteEffects.None, 0f); 
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
