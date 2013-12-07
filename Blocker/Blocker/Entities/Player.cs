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
    public enum PlayerState { Idle, Moving }
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Rectangle position;

        private Animation idle;
        private Texture2D up;
        private Texture2D down;
        private Texture2D left;
        private Texture2D right;

        private PlayerState state = PlayerState.Idle;
        private Movement movement;

        private Level level;

        public Player(Game game, SpriteBatch spriteBatch, Rectangle position, Level level)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.position = position;
            this.level = level;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            List<Texture2D> slides = new List<Texture2D>();
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle1"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle2"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle3"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle4"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle5"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle6"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle7"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle8"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle9"));
            slides.Add(game.Content.Load<Texture2D>("Player\\Idle\\Idle10"));

            idle = new Animation(game, spriteBatch, this, slides, 6);
            idle.Initialize();

            up = game.Content.Load<Texture2D>("Player\\Up\\Astronaut");
            down = game.Content.Load<Texture2D>("Player\\Idle\\Idle1");
            right = game.Content.Load<Texture2D>("Player\\Right\\Astronaut");
            left = game.Content.Load<Texture2D>("Player\\Left\\Astronaut");

            base.Initialize();
        }

        public PlayerState Getstate()
        {
            return this.state;
        }

        public Rectangle GetPosition()
        {
            return this.position;
        }

        public void Move(Movement movement)
        {
            this.movement = movement;
            position = movement.GetEnd();
            state = PlayerState.Moving;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (state == PlayerState.Idle)
            {
                // TODO
            }
            else if (state == PlayerState.Moving)
            {
                movement.Update(gameTime);

                // Check collisions with matter
                for (int y = 0; y < level.Map.GetLength(0); y++)
                {
                    for (int x = 0; x < level.Map.GetLength(1); x++)
                    {
                        if (level.Map[y, x] != null && level.Map[y, x].GetType() == typeof(Matter))
                        {
                            if (movement.position.Intersects(level.Map[y, x].GetPosition()))
                                level.AddMatterAt(x, y);
                        }
                    }
                }
           
                // Check if movement is complete
                if (movement.complete)
                {
                    state = PlayerState.Idle;
                    movement = null;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if(state == PlayerState.Idle)
                idle.Draw(gameTime);
            else if (state == PlayerState.Moving)
            {
                spriteBatch.Begin();
                switch(movement.direction)
                {
                    case Direction.Up:
                        spriteBatch.Draw(up, movement.position, Color.White);
                        break;
                    case Direction.Right:
                        spriteBatch.Draw(right, new Rectangle(movement.position.X, movement.position.Y - 6, 40, 40), null, Color.White, MathHelper.ToRadians(30f),
                            Vector2.Zero, SpriteEffects.None, 0f);
                        break;
                    case Direction.Left:
                        spriteBatch.Draw(left, new Rectangle(movement.position.X, movement.position.Y + 12, 40, 40), null, Color.White, MathHelper.ToRadians(-30f),
                            Vector2.Zero, SpriteEffects.None, 0f);
                        break;
                    case Direction.Down:
                        spriteBatch.Draw(down, movement.position, Color.White);
                        break;
                }
                spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }
    }
}
