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
    public class ParticleController : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private List<Particle> particles = new List<Particle>();

        public ParticleController(Game game, SpriteBatch spriteBatch)
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
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void AddExplosion(Texture2D texture, Vector2 position, int numberOfParticles, float scale, int maxAge, Color color, GameTime gameTime)
        {
            float degrees = 360 / numberOfParticles;
            for (int i = 0; i < numberOfParticles; i++)
            {
                AddExplosionParticle(texture, position, i*degrees, scale, maxAge, color, gameTime);
            }
        }

        private void AddExplosionParticle(Texture2D texture, Vector2 position, float degrees, float scale, int maxAge, Color color, GameTime gameTime)
        {
            Random randomizer = new Random();
            Particle particle = new Particle(game, spriteBatch, texture, gameTime);

            particle.Position = position;
            particle.Velocity = VelocityOnDegrees(degrees);
            particle.Angle = MathHelper.ToRadians(degrees);
            particle.Color = color;
            particle.Scale = scale;
            particle.MaxAge = maxAge;

            particles.Add(particle);
        }

        private Vector2 VelocityOnDegrees(float degrees)
        {
            return new Vector2 ((float)Math.Cos(MathHelper.ToRadians(degrees)), 
                (float)Math.Sin(MathHelper.ToRadians(degrees)));
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int p = 0; p < particles.Count; p++)
            {
                
                particles[p].Update(gameTime);
                if (particles[p].IsDead(gameTime))
                {
                    particles.RemoveAt(p);
                    p--;
                }
         
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Particle p in particles)
                p.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
