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
        public String Text
        {
            get { return text; }
            set 
            { 
                text = value;
                SetTextLocation(); 
            }
        }

        private Color textColor = Color.White;
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        private Rectangle destination;
        Vector2 textLocation;

        public enum LabelPosition { Left, Center }
        private LabelPosition textPosition = LabelPosition.Center;
        public LabelPosition TextPosition
        {
            get { return textPosition; }
            set 
            { 
                textPosition = value;
                SetTextLocation(); 
            }
        }

        public Label(Game game, SpriteBatch spriteBatch, Rectangle destination, SpriteFont font, String text)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.Text = text;
            this.destination = destination;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            SetTextLocation();

            base.Initialize();
        }

        private void SetTextLocation()
        {
            if (textPosition == LabelPosition.Center)
            {
                Vector2 size = font.MeasureString(Text);
                textLocation = new Vector2();
                textLocation.X = destination.X + ((destination.Width / 2) - (size.X / 2));
                textLocation.Y = destination.Y + ((destination.Height / 2) - (size.Y / 2));
            }
            else
            {
                Vector2 size = font.MeasureString(Text);
                textLocation = new Vector2();
                textLocation.X = destination.X;
                textLocation.Y = destination.Y + ((destination.Height / 2) - (size.Y / 2));

            }
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
            spriteBatch.DrawString(font, Text, textLocation, TextColor);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}