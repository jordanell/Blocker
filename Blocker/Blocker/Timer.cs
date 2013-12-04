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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Timer : Microsoft.Xna.Framework.GameComponent
    {
        private Game game;

        private int start = -1;
        private int current;
        private int duration;
        private int end;

        public Timer(Game game, int duration)
            : base(game)
        {
            this.game = game;
            this.duration = duration;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public bool IsDone()
        {
            return (current > end);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (start == -1)
            {
                start = (int)gameTime.TotalGameTime.TotalMilliseconds;
                current = start;
                end = start + duration;
                return;
            }

            if (IsDone())
                return;
            
            current += gameTime.ElapsedGameTime.Milliseconds;

            base.Update(gameTime);
        }
    }
}
