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


namespace Blocker.Entities
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Instruction : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private int instNumber;

        private Texture2D exit;
        private Texture2D inst;

        private Rectangle position;

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
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            String file = "Instructions\\Inst" + Convert.ToString(instNumber);

            inst = game.Content.Load<Texture2D>(file);
            exit = game.Content.Load<Texture2D>("Instructions\\X");

            SetPosition();

            base.Initialize();
        }

        private void SetPosition()
        {
            int y = 80 + (360 - (inst.Height / 2));
            position = new Rectangle(90, y, inst.Width, inst.Height);
        }

        private bool isXTouched(Vector2 position)
        {
            return (position.X >= 340 && position.X <= 460 &&
                    position.Y >= this.position.Y - 70 && position.Y <= this.position.Y + 50);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (GestureSample gs in InputHandler.Instance.Taps())
            {
                if (isXTouched(gs.Position))
                {
                    Complete = true;
                    SoundMixer.Instance(game).PlayEffect("Audio\\Button");
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(inst, position, Color.White);
            spriteBatch.Draw(exit, new Rectangle(380, position.Y - 30, 40, 40), Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
