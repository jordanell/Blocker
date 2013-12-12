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
    /// This is a game component that implements IUpdateable. Exit is the space shuttle seen in
    /// the game which the player must get to.
    /// </summary>
    public class Exit : Block
    {
        private Animation animation;

        public Exit(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle position)
            : base(game, spriteBatch, texture, position)
        {
            Initialize();
        }

        /// <summary>
        /// Load the textures needed to create the animation for the space shuttle.
        /// </summary>
        public override void Initialize()
        {
            List<Texture2D> slides = new List<Texture2D>();
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle1"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle2"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle3"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle4"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle5"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle6"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle7"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle8"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle9"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle8"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle7"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle6"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle5"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle4"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle3"));
            slides.Add(game.Content.Load<Texture2D>("Tiles\\Shuttle\\Shuttle2"));

            // Create the animation
            animation = new Animation(game, spriteBatch, this, slides, 2);

            base.Initialize();
        }

        /// <summary>
        /// Allows the Exit to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Allows the Exit to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the animation
            animation.Draw(gameTime);
        }
    }
}
