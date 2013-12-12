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


namespace Blocker.Entities
{
    /// <summary>
    /// This is a game component that implements IUpdateable. An instruction can be shown
    /// at the start of any level and contains information on how to play Block3r.
    /// </summary>
    public class Instruction : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Number of instruction
        private int instNumber;

        // Text for exit button
        private Texture2D exit;

        // Texture of instruction
        private Texture2D inst;

        // Position of instruction
        private Rectangle position;

        // Instruction is closed (completed) flag
        public bool Complete { get; private set; }

        public Instruction(Game game, SpriteBatch spriteBatch, int inst)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.instNumber = inst;

            Initialize();
        }

        /// <summary>
        /// Load all textures needed for instruction and set the position of it
        /// relative to height of instruction texture.
        /// </summary>
        public override void Initialize()
        {
            // File name of instruction texture
            String file = "Instructions\\Inst" + Convert.ToString(instNumber);

            // Load textures
            inst = game.Content.Load<Texture2D>(file);
            exit = game.Content.Load<Texture2D>("Instructions\\X");

            // Set texture position
            SetPosition();

            base.Initialize();
        }

        /// <summary>
        /// Sets the position of the instruction textures based on the height
        /// of the instruction texture.
        /// </summary>
        private void SetPosition()
        {
            int y = 80 + (360 - (inst.Height / 2));
            position = new Rectangle(90, y, inst.Width, inst.Height);
        }

        /// <summary>
        /// Returns true if the position given is in a 40+ pixel padded
        /// box of the exit texture.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool isXTouched(Vector2 position)
        {
            return (position.X >= 340 && position.X <= 460 &&
                    position.Y >= this.position.Y - 70 && position.Y <= this.position.Y + 50);
        }

        /// <summary>
        /// Allows the instruction to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Handle new input taps
            foreach (GestureSample gs in InputHandler.Instance.Taps())
            {
                // Handle tap on the exit texture
                if (isXTouched(gs.Position))
                {
                    Complete = true;

                    // Play sounds and vibrate as needed.
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
        /// Allows the instruction to draw itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the instruction and exit textures.
            spriteBatch.Begin();
            spriteBatch.Draw(inst, position, Color.White);
            spriteBatch.Draw(exit, new Rectangle(380, position.Y - 30, 40, 40), Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
