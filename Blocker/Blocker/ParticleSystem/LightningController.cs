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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LightningController : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

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
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void CreateLightning(Vector2 source, Vector2 dest, Color color)
        {
            if (source == dest)
                return;

            LightningBolt bolt = new LightningBolt(game, spriteBatch, source, dest, color);
            bolts.Add(bolt);

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int b = 0; b < bolts.Count; b++)
            {

                bolts[b].Update(gameTime);
                if (bolts[b].Complete)
                {
                    bolts.RemoveAt(b);
                    b--;
                }

            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (LightningBolt b in bolts)
                b.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
