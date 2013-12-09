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

        private Button exit;
        private Texture2D inst;

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

            Texture2D button = game.Content.Load<Texture2D>("Instructions\\X");
            exit = new Button(game, spriteBatch, new Rectangle(370, 280, 40, 40), button, null, "");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (exit.state == TouchButtonState.Clicked)
                Complete = true;

            exit.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(inst, new Rectangle(90, 300, inst.Width, inst.Height), Color.White);
            spriteBatch.End();

            exit.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
