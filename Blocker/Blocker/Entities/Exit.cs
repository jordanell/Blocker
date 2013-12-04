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
    public class Exit : Block
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Animation animation;

        public Exit(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle position)
            : base(game, spriteBatch, texture, position)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
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

            animation = new Animation(game, spriteBatch, this, slides, 2);
            animation.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

        }

        public override void Draw(GameTime gameTime)
        {
            animation.Draw(gameTime);
        }
    }
}
