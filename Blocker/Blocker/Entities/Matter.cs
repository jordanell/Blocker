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
    /// This is a game component that implements IUpdateable. Matter represents the matter
    /// than can be found inside of the levels of Block3r.
    /// </summary>
    public class Matter : Block
    {
        // Color of the matter
        public Color Color { get; set; }

        // Animation of the matter block
        private Animation animation;

        public Matter(Game game, SpriteBatch spriteBatch, Texture2D texture, Rectangle position, Color color)
            : base(game, spriteBatch, texture, position)
        {
            this.Color = color;
        }

        /// <summary>
        /// Load the textures for the different types of matter
        /// </summary>
        public override void Initialize()
        {
            List<Texture2D> slides = new List<Texture2D>();

            // Load red matter textures
            if (Color == Color.Red)
            {
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter1"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter2"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter3"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter4"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\RedMatter\\RedMatter5"));
            }
            // Load blue matter textures
            else if (Color == Color.Blue)
            {
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter1"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter2"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter3"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter4"));
                slides.Add(game.Content.Load<Texture2D>("Tiles\\Matter\\BlueMatter\\BlueMatter5"));
            }

            // Create animation
            animation = new Animation(game, spriteBatch, this, slides, 4);

            base.Initialize();
        }

        /// <summary>
        /// Allows the matter to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Allows the matter to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the animation
            animation.Draw(gameTime);
        }
    }
}
