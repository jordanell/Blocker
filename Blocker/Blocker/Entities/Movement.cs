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
    /// The different types of movement allowed in Block3r.
    /// </summary>
    public enum Direction { None, Up, Down, Left, Right }

    /// <summary>
    /// This is a game component that implements IUpdateable. A movement can be applied to 
    /// any entity inside of Block3r. A movement causes a block in the grid world to move
    /// from A to B smoothly.
    /// </summary>
    public class Movement : Microsoft.Xna.Framework.GameComponent
    {
        // Xna componenets
        private Game game;

        // Start and end locations of movement
        public Rectangle Start { get; set; }
        public Rectangle End { get; set; }

        // Current position of movement (needs to be a variable to allow easy changes)
        public Rectangle Position;

        // Direction of movement
        public Direction Direction { get; set; }

        // Constant tile sizes
        private const int tileWidth = 40;
        private const int tileHeight = 40;

        // Milliseconds to move from one tile to the next
        private const int timePerTile = 150;

        // Timing variables
        private int startTime = -1;
        private int maxTime;
        private int runTime = 0;

        // Movement complete flag
        public bool Complete { get; set; }

        public Movement(Game game, Rectangle start, Rectangle end)
            : base(game)
        {
            this.game = game;
            this.Start = start;
            this.End = end;
            this.Position = start;
        }

        /// <summary>
        /// Set the direction of the movement and set the maximum time for
        /// the movement from A to B.
        /// </summary>
        public override void Initialize()
        {
            if (Start.X != End.X)
            {
                maxTime = Math.Abs(((Start.X - End.X) / tileWidth) * timePerTile);
                if (Start.X > End.X)
                    Direction = Direction.Left;
                else
                    Direction = Direction.Right;
            }
            else if (Start.Y != End.Y)
            {
                maxTime = Math.Abs(((Start.Y - End.Y) / tileHeight) * timePerTile);
                if (Start.Y > End.Y)
                    Direction = Direction.Up;
                else
                    Direction = Direction.Down;
            }

            base.Initialize();
        }

        /// <summary>
        /// Allows the movement to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Set the start time of the movement if needed
            if (startTime == -1)
                startTime = gameTime.ElapsedGameTime.Milliseconds;

            // Add to the run time of the movement
            runTime += gameTime.ElapsedGameTime.Milliseconds;

            // Check and set for a completed movement
            float completed = ((float)runTime - (float)startTime) / (float)maxTime;
            if (completed >= 1.0f)
            {
                Complete = true;
                return;
            }

            // Update movement position
            switch (Direction)
            {
                case Direction.Up:
                    Position.Y = Start.Y - (int)((Start.Y - End.Y) * completed);
                    break;
                case Direction.Down:
                    Position.Y = Start.Y + (int)((End.Y - Start.Y) * completed);
                    break;
                case Direction.Left:
                    Position.X = Start.X - (int)((Start.X - End.X) * completed);
                    break;
                case Direction.Right:
                    Position.X = Start.X + (int)((End.X - Start.X) * completed);
                    break;
            }

            base.Update(gameTime);
        }
    }
}
