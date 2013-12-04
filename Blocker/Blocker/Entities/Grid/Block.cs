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
    public class Block : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Game game;
        protected SpriteBatch spriteBatch;

        protected Texture2D blockTexture;

        protected Rectangle position;

        public Block(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle position)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.blockTexture = texture;
            this.position = position;
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

        public Rectangle GetPosition()
        {
            return position;
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
            spriteBatch.Begin();
            spriteBatch.Draw(blockTexture, position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
