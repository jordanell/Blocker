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
    /// This is a game component that implements IUpdateable. Label is used to render
    /// text to the screen.
    /// </summary>
    public class Label : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // XNA Components
        private Game game;
        private SpriteBatch spriteBatch;

        // Font to be used
        private SpriteFont font;

        // Text to be rendered
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

        // Color of the text
        public Color TextColor { get; set; }

        // Location of label box
        private Rectangle destination;

        // Location of text in label box
        private Vector2 textLocation;

        // Text alignment
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
            TextColor = Color.White;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  
        /// </summary>
        public override void Initialize()
        {
            SetTextLocation();

            base.Initialize();
        }

        /// <summary>
        /// Based on the position state of the text, position the text inside of the text box.
        /// </summary>
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
        /// Allows the Label to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the Label to draw itself
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, Text, textLocation, TextColor);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
