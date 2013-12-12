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


namespace Blocker.ParticleSystem
{
    /// <summary>
    /// This is a game component that implements IUpdateable. The lightning controller is used
    /// to creating the particle effects seen in Block3r when moving a block. 
    /// </summary>
    public class LightningController : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Lightning bolts to be drawn
        private List<LightningBolt> bolts = new List<LightningBolt>();

        public LightningController(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
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
        /// Adds a lightning bolt to the game.
        /// </summary>
        /// <param name="source">The source of the lightning bolt.</param>
        /// <param name="dest">The destination of the lightning bolt.</param>
        /// <param name="color">The color of the lightning bolt.</param>
        public void CreateLightning(Vector2 source, Vector2 dest, Color color)
        {
            // Can't have a bolt that goes no where
            if (source == dest)
                return;

            // Create and add the bolt
            LightningBolt bolt = new LightningBolt(game, spriteBatch, source, dest, color);
            bolts.Add(bolt);

        }

        /// <summary>
        /// Allows the lightning controller to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update all bolts
            for (int b = 0; b < bolts.Count; b++)
            {

                bolts[b].Update(gameTime);

                // Remove the bolt when it is dead
                if (bolts[b].Complete)
                {
                    bolts.RemoveAt(b);
                    b--;
                }

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the lightning controller to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw all bolts
            foreach (LightningBolt b in bolts)
                b.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
