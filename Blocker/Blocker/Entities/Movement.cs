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
    public enum Direction { None, Up, Down, Left, Right }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Movement : Microsoft.Xna.Framework.GameComponent
    {
        private Game game;

        public Rectangle start;
        public Rectangle end;
        public Rectangle position;

        public Direction direction;

        private int tileWidth = 40;
        private int tileHeight = 40;

        private int timePerTile = 150;

        private int startTime = -1;
        private int maxTime;
        private int runTime = 0;

        public bool complete;

        public Movement(Game game, Rectangle start, Rectangle end)
            : base(game)
        {
            this.game = game;
            this.start = start;
            this.end = end;
            this.position = start;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            if (start.X != end.X)
            {
                maxTime = Math.Abs(((start.X - end.X) / tileWidth) * timePerTile);
                if (start.X > end.X)
                    direction = Direction.Left;
                else
                    direction = Direction.Right;
            }
            else if (start.Y != end.Y)
            {
                maxTime = Math.Abs(((start.Y - end.Y) / tileHeight) * timePerTile);
                if (start.Y > end.Y)
                    direction = Direction.Up;
                else
                    direction = Direction.Down;
            }

            base.Initialize();
        }

        public Rectangle GetEnd()
        {
            return end;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (startTime == -1)
                startTime = gameTime.ElapsedGameTime.Milliseconds;

            runTime += gameTime.ElapsedGameTime.Milliseconds;

            float completed = ((float)runTime - (float)startTime) / (float)maxTime;

            if (completed >= 1.0f)
            {
                complete = true;
                return;
            }

            switch (direction)
            {
                case Direction.Up:
                    position.Y = start.Y - (int)((start.Y - end.Y) * completed);
                    break;
                case Direction.Down:
                    position.Y = start.Y + (int)((end.Y - start.Y) * completed);
                    break;
                case Direction.Left:
                    position.X = start.X - (int)((start.X - end.X) * completed);
                    break;
                case Direction.Right:
                    position.X = start.X + (int)((end.X - start.X) * completed);
                    break;
            }

            base.Update(gameTime);
        }
    }
}
