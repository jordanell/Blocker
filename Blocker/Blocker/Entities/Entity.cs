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


namespace Blocker.Entities
{
    /// <summary>
    /// Entity is the base class needed for game objects.
    /// </summary>
    public class Entity : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        protected Game game;
        protected SpriteBatch spriteBatch;

        // Position of entity
        public Rectangle Position { get; protected set; }

        public Entity(Game game, SpriteBatch spriteBatch, Rectangle position)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.Position = position;
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
        /// Allows the entity to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the entity to draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);
        }
    }
}
