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
    /// This is a game component that implements IUpdateable. Block is the core component
    /// of the level world. Block is an imoveable object.
    /// </summary>
    public class Block : Entity
    {
        // Texture for block
        protected Texture2D blockTexture;

        public Block(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle position)
            : base(game, spriteBatch, position)
        {
            this.blockTexture = texture;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Allows the block to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the block to draw itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blockTexture, Position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
