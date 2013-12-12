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
    /// The states a player object can be in.
    /// </summary>
    public enum PlayerState { Idle, Moving }

    /// <summary>
    /// This is a game component that implements IUpdateable. The player object is the main
    /// character object in Block3r.
    /// </summary>
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Player position
        public Rectangle Position { get; private set; }

        // Animation for the idle state
        private Animation idle;

        // Textures for moving in various directions
        private Texture2D up;
        private Texture2D down;
        private Texture2D left;
        private Texture2D right;

        // Player state
        public PlayerState State { get; private set; }

        // Movements to be applied
        private Movement movement;

        // Reference to the level the player is in
        private Level level;

        public Player(Game game, SpriteBatch spriteBatch, Rectangle position, Level level)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.Position = position;
            this.level = level;
            this.State = PlayerState.Idle;
        }

        /// <summary>
        /// Loads all textures needed for player animations and movements.
        /// </summary>
        public override void Initialize()
        {
            // Load idle animation slides
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

            // Create idle animation
            idle = new Animation(game, spriteBatch, this, slides, 6);

            // Load all other directional textures
            up = game.Content.Load<Texture2D>("Player\\Up\\Astronaut");
            down = game.Content.Load<Texture2D>("Player\\Idle\\Idle1");
            right = game.Content.Load<Texture2D>("Player\\Right\\Astronaut");
            left = game.Content.Load<Texture2D>("Player\\Left\\Astronaut");

            base.Initialize();
        }

        /// <summary>
        /// Apply a movement to the player.
        /// </summary>
        /// <param name="movement"></param>
        public void Move(Movement movement)
        {
            this.movement = movement;
            Position = movement.End;
            State = PlayerState.Moving;
        }

        /// <summary>
        /// Allows the player to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update movement if moving
            if (State == PlayerState.Moving)
            {
                movement.Update(gameTime);

                // Check collisions with matter
                for (int y = 0; y < level.Map.GetLength(0); y++)
                {
                    for (int x = 0; x < level.Map.GetLength(1); x++)
                    {
                        if (level.Map[y, x] != null && level.Map[y, x].GetType() == typeof(Matter))
                        {
                            // Add matter when colidding
                            if (movement.Position.Intersects(level.Map[y, x].Position))
                                level.AddMatterAt(x, y);
                        }
                    }
                }
           
                // Check if movement is complete
                if (movement.Complete)
                {
                    State = PlayerState.Idle;
                    movement = null;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the player to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw idle animation when needed
            if(State == PlayerState.Idle)
                idle.Draw(gameTime);
            // Draw a directional texture when needed
            else if (State == PlayerState.Moving)
            {
                spriteBatch.Begin();
                // Draw texture based on movement direction
                switch(movement.Direction)
                {
                    case Direction.Up:
                        spriteBatch.Draw(up, movement.Position, Color.White);
                        break;
                    case Direction.Right:
                        spriteBatch.Draw(right, new Rectangle(movement.Position.X, movement.Position.Y - 6, 40, 40), null, Color.White, MathHelper.ToRadians(30f),
                            Vector2.Zero, SpriteEffects.None, 0f);
                        break;
                    case Direction.Left:
                        spriteBatch.Draw(left, new Rectangle(movement.Position.X, movement.Position.Y + 12, 40, 40), null, Color.White, MathHelper.ToRadians(-30f),
                            Vector2.Zero, SpriteEffects.None, 0f);
                        break;
                    case Direction.Down:
                        spriteBatch.Draw(down, movement.Position, Color.White);
                        break;
                }
                spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }
    }
}
