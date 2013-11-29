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
    public class Matter : Block
    {
        private Color color;

        private Animation animation;

        public Matter(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle position, Color color)
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
            List<Texture2D> slides = new List<Texture2D>();

            if (color == Color.Red)
            {
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter1"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter2"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter3"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter4"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter5"));
            }
            else if (color == Color.Blue)
            {
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter1"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter2"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter3"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter4"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter5"));
            }

            animation = new Animation(game, spriteBatch, this, slides, 4);
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
