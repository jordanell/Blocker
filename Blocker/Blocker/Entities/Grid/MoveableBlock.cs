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
    public enum MoveableBlockState { Idle, Moving }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MoveableBlock : Block
    {
        public Color color;

        private MoveableBlockState state = MoveableBlockState.Idle;

        private Movement movement;

        public MoveableBlock(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle position, Color color)
            : base(game, spriteBatch, texture, position)
        {
            this.color = color;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        public void Move(Movement movement)
        {
            this.movement = movement;
            position = movement.GetEnd();
            state = MoveableBlockState.Moving;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (state == MoveableBlockState.Idle)
            {
                // TODO
            }
            else if (state == MoveableBlockState.Moving)
            {
                movement.Update(gameTime);

                // Check if movement is complete
                if (movement.complete)
                {
                    state = MoveableBlockState.Idle;
                    movement = null;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (state == MoveableBlockState.Idle)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(blockTexture, position, Color.White);
                spriteBatch.End();
            }
            else if (state == MoveableBlockState.Moving)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(blockTexture, movement.position, Color.White);
                spriteBatch.End();
            }
        }
    }
}
