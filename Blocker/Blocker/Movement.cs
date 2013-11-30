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
    public enum MovementDirection { Up, Down, Left, Right }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Movement : Microsoft.Xna.Framework.GameComponent
    {
        private Game game;

        private Rectangle start;
        public Rectangle end;
        public Rectangle position;

        private MovementDirection direction;

        private int tileWidth = 40;
        private int tileHeight = 40;

        private int timePerTile = 300;

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
                    direction = MovementDirection.Left;
                else
                    direction = MovementDirection.Right;
            }
            else if (start.Y != end.Y)
            {
                maxTime = Math.Abs(((start.Y - end.Y) / tileHeight) * timePerTile);
                if (start.Y > end.Y)
                    direction = MovementDirection.Up;
                else
                    direction = MovementDirection.Down;
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
                case MovementDirection.Up:
                    position.Y = start.Y - (int)((start.Y - end.Y) * completed);
                    break;
                case MovementDirection.Down:
                    position.Y = start.Y + (int)((end.Y - start.Y) * completed);
                    break;
                case MovementDirection.Left:
                    position.X = start.X - (int)((start.X - end.X) * completed);
                    break;
                case MovementDirection.Right:
                    position.X = start.X + (int)((end.X - start.X) * completed);
                    break;
            }

            base.Update(gameTime);
        }
    }
}
