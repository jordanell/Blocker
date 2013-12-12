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
    /// This is a game component that implements IUpdateable. The particle controller is used
    /// to create the explosions seen in the end game scene.
    /// </summary>
    public class ParticleController : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // List of particles in system
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

            base.Initialize();
        }

        /// <summary>
        /// Creates an explosion inside the particle system.
        /// </summary>
        /// <param name="texture">The texture to use for particles.</param>
        /// <param name="position">The position of the explosion center.</param>
        /// <param name="numberOfParticles">The number of particles for the explosion.</param>
        /// <param name="scale">The scale of all particles in the explosion.</param>
        /// <param name="maxAge">The max age of all particles in the explosion.</param>
        /// <param name="color">The color tint of all particles in the explosion.</param>
        /// <param name="gameTime">The gameTime of Block3r.</param>
        public void AddExplosion(Texture2D texture, Vector2 position, int numberOfParticles, float scale, int maxAge, Color color, GameTime gameTime)
        {
            // Roatate all particles to complete a circle
            float degrees = 360 / numberOfParticles;

            // Add the particles
            for (int i = 0; i < numberOfParticles; i++)
                AddExplosionParticle(texture, position, i * degrees, scale, maxAge, color, gameTime);
        }

        /// <summary>
        /// Adds a particles to the system.
        /// </summary>
        /// <param name="texture">The texture to use for particle.</param>
        /// <param name="position">The position of the particle.</param>
        /// <param name="degrees">The degrees of rotation for the particle.</param>
        /// <param name="scale">The scale of the particle.</param>
        /// <param name="maxAge">The max age of the particle.</param>
        /// <param name="color">The color tint of the particle.</param>
        /// <param name="gameTime">The gameTime of Block3r.</param>
        private void AddExplosionParticle(Texture2D texture, Vector2 position, float degrees, float scale, int maxAge, Color color, GameTime gameTime)
        {
            // Create the particle
            Particle particle = new Particle(game, spriteBatch, texture, gameTime);

            // Set all the properties from the parameters
            particle.Position = position;
            particle.Velocity = VelocityOnDegrees(degrees);
            // Convert to radians for the particle
            particle.Angle = MathHelper.ToRadians(degrees);
            particle.Color = color;
            particle.Scale = scale;
            particle.MaxAge = maxAge;

            // Add it to system
            particles.Add(particle);
        }

        /// <summary>
        /// Returns a vector along the angle of rotation.
        /// </summary>
        /// <param name="degrees">The angle of rotation in degrees.</param>
        /// <returns>The vector to be returned.</returns>
        private Vector2 VelocityOnDegrees(float degrees)
        {
            return new Vector2 ((float)Math.Cos(MathHelper.ToRadians(degrees)), 
                (float)Math.Sin(MathHelper.ToRadians(degrees)));
        }

        /// <summary>
        /// Allows the particle controller to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update all particles
            for (int p = 0; p < particles.Count; p++)
            {
                
                particles[p].Update(gameTime);

                // Remove particle when dead
                if (particles[p].IsDead(gameTime))
                {
                    particles.RemoveAt(p);
                    p--;
                }
         
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the particle controller to draw itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw all particles
            foreach (Particle p in particles)
                p.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
