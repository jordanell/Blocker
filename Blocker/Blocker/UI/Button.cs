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
using Microsoft.Devices;


namespace Blocker
{
    /// <summary>
    /// The various states a button can be found in.
    /// Up = no contact on the button
    /// Down = contact on the button
    /// Clicked = button has had the tap gesture applied to it.
    /// </summary>
    public enum TouchButtonState { Up, Down, Clicked }

    /// <summary>
    /// This is a game component that implements IUpdateable. Button is used as a tap
    /// interface button throughout the game. A button needs a observer class to act once the
    /// button indicates that it has been clicked.
    /// </summary>
    public class Button : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Button position
        private Rectangle position;

        // Texture of button
        private Texture2D texture;

        // Font for text
        public SpriteFont Font { get; set; }

        // Text for the button
        public String Text { get; set; }

        // Color of text
        private Color color = Color.White;

        // Text location in button
        private Vector2 textLocation;

        // State of the button
        private TouchButtonState state = TouchButtonState.Up;
        public TouchButtonState State
        {
            get { return state; }
            private set { state = value; }
        }

        // Flag for button enabled
        public bool enabled = true;

        public Button(Game game, SpriteBatch spriteBatch, Rectangle destination, Texture2D texture, SpriteFont font, String text)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            this.position = destination;
            this.texture = texture;
            this.Font = font;
            this.Text = text;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  Set the location of the text if needed.
        /// </summary>
        public override void Initialize()
        {
            if (Font != null)
                SetTextLocation();

            base.Initialize();
        }

        /// <summary>
        /// Set the location of the text based on button position and font.
        /// </summary>
        private void SetTextLocation()
        {
            Vector2 size = Font.MeasureString(Text);
            textLocation = new Vector2();
            textLocation.X = position.X + ((position.Width / 2) - (size.X / 2));
            textLocation.Y = position.Y + ((position.Height / 2) - (size.Y / 2));
        }

        /// <summary>
        /// Return true if position is within the position of the button.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool isButtonTouched(Vector2 position) 
        {
            return (position.X >= this.position.Left && position.X <= this.position.Right &&
                    position.Y >= this.position.Top  && position.Y <= this.position.Bottom);
        }

        /// <summary>
        /// Allows the Button to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Only update if the button is enabled
            if (!enabled)
                return;

            // Reset the state
            state = TouchButtonState.Up;

            // Handle gestures
            foreach (GestureSample gs in InputHandler.Instance.Taps())
            {
                // Handle button when tapped
                if (isButtonTouched(gs.Position))
                {
                    // Clear other input
                    InputHandler.Instance.Clear();
                    state = TouchButtonState.Clicked;

                    // Play sound and vibrate as needed
                    if (SoundMixer.Instance(game).Muted)
                    {
                        VibrateController vibrate = VibrateController.Default;
                        vibrate.Start(TimeSpan.FromMilliseconds(25));
                    }
                    else
                        SoundMixer.Instance(game).PlayEffect("Audio\\Button");
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the button to draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Draw based on enabled
            if (enabled)
            {
                // Draw normally
                spriteBatch.Draw(texture, position, Color.White);
                if (Font != null)
                    spriteBatch.DrawString(Font, Text, textLocation, color);
            }
            else
            {
                // Draw tinted silver
                spriteBatch.Draw(texture, position, Color.Silver);
                if (Font != null)
                    spriteBatch.DrawString(Font, Text, textLocation, Color.Red);
            }

            spriteBatch.End();
        }
    }
}
