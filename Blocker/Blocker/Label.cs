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
    public class Label : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private SpriteFont font;

        private String text;

        private Rectangle destination;
        Vector2 textLocation;

        public Label(Game game, SpriteBatch spriteBatch, Rectangle destination, SpriteFont font, String text)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.text = text;
            this.destination = destination;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            setTextLocation();

            base.Initialize();
        }

        private void setTextLocation()
        {
            Vector2 size = font.MeasureString(text);
            textLocation = new Vector2();
            textLocation.X = destination.X + ((destination.Width / 2) - (size.X / 2));
            textLocation.Y = destination.Y + ((destination.Height / 2) - (size.Y / 2));
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
            spriteBatch.DrawString(font, text, textLocation, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
