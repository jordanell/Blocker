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
    public class Background : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch sb;

        private Texture2D staticBG;
        private Texture2D layer1;

        private Vector2 scroll;

        public Background(Game game, SpriteBatch sb)
            : base(game)
        {
            this.game =  game;
            this.sb = sb;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            staticBG = game.Content.Load<Texture2D>("StarFieldBlue1600");

            base.Initialize();
        }

        public void setLayer(Texture2D layer)
        {
            this.layer1 = layer;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            scroll.X += 0.5f;
            scroll.Y += 0.5f;

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the background to draw itself
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the static background
            sb.Begin();
            sb.Draw(staticBG, new Rectangle(0, 0, 480, 800), Color.White);
            sb.End();

            //if (layer1 != null)
            //{
            //    sb.Draw(layer1, Vector2.Zero,
            //            new Rectangle((int)-scroll.X, (int)-scroll.Y, layer1.Width, layer1.Height),
            //            Color.White);
            //}

            base.Draw(gameTime);
        }
    }
}
