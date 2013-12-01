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
using Microsoft.Xna.Framework.Input.Touch;


namespace Blocker
{
    /// <summary>
    /// 
    /// </summary>
    public enum TouchButtonState { Up, Down, Clicked }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Button : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Rectangle destination;
        private Texture2D texture;
        private SpriteFont font;

        private String text;
        public String Text
        {
            get { return text; }
            set { text = value; }
        }

        private Color color = Color.White;
        private Vector2 textLocation;

        public TouchButtonState state = TouchButtonState.Up;

        public bool enabled = true;

        public Button(Game game, SpriteBatch spriteBatch, Rectangle destination, Texture2D texture, SpriteFont font, String text)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            this.destination = destination;
            this.texture = texture;
            this.font = font;
            this.text = text;

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
            Vector2 size = font.MeasureString(text);
            textLocation = new Vector2();
            textLocation.X = destination.X + ((destination.Width / 2) - (size.X / 2));
            textLocation.Y = destination.Y + ((destination.Height / 2) - (size.Y / 2));
        }

        public void setFontColor(Color color)
        {
            this.color = color;
        }

        private bool isButtonTouched(TouchLocation touch) 
        {
            return (touch.Position.X >= destination.Left && touch.Position.X <= destination.Right &&
                    touch.Position.Y >= destination.Top  && touch.Position.Y <= destination.Bottom);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (!enabled)
                return;

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation touch in touchCollection)
            {
                if (isButtonTouched(touch))
                    state = TouchButtonState.Clicked;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (enabled)
            {
                spriteBatch.Draw(texture, destination, Color.White);
                spriteBatch.DrawString(font, text, textLocation, color);
            }
            else
            {
                spriteBatch.Draw(texture, destination, Color.Silver);
                spriteBatch.DrawString(font, text, textLocation, Color.Red);
            }

            spriteBatch.End();
        }
    }
}
