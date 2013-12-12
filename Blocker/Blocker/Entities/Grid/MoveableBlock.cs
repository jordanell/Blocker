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
    /// The states in which a moving block can be in.
    /// </summary>
    public enum MoveableBlockState { Idle, Moving }

    /// <summary>
    /// This is a game component that implements IUpdateable. MoveableBlock inherits from
    /// block but is used for red and blue blocks which can move.
    /// </summary>
    public class MoveableBlock : Block
    {
        // Block color
        public Color Color { get; set; }

        // Block state
        private MoveableBlockState state = MoveableBlockState.Idle;

        // Movement for the block
        private Movement movement;

        public MoveableBlock(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle position, Color color)
            : base(game, spriteBatch, texture, position)
        {
            this.Color = color;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Move a block based on a provided movement
        /// </summary>
        /// <param name="movement">A movement which will be applied to the block</param>
        public void Move(Movement movement)
        {
            this.movement = movement;
            Position = movement.End;
            state = MoveableBlockState.Moving;
        }

        /// <summary>
        /// Allows the MoveableBlock to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Update a moving block
            if (state == MoveableBlockState.Moving)
            {
                movement.Update(gameTime);

                // Check if movement is complete
                if (movement.Complete)
                {
                    state = MoveableBlockState.Idle;
                    movement = null;
                }
            }
        }

        /// <summary>
        /// Allows the MoveableBlock to draw itself.
        /// </summary>
        /// <param name="gameTime">>Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // When idle, just draw in position
            if (state == MoveableBlockState.Idle)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(blockTexture, Position, Color.White);
                spriteBatch.End();
            }
            // When moving draw at movement position
            else if (state == MoveableBlockState.Moving)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(blockTexture, movement.Position, Color.White);
                spriteBatch.End();
            }
        }
    }
}
