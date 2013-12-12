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


namespace Blocker.ParticleSystem
{
    /// <summary>
    /// This is a game component that implements IUpdateable. The line object is a primitive
    /// object used for later drawing the lightning effects found in the game.
    /// </summary>
    public class Line : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Start and end points of the line
        public Vector2 PointA { get; set; }
        public Vector2 PointB { get; set; }

        // Thickness of the line
        public float Thickness { get; set; }
        private const float ImageThickness = 3;

        // Color tint for the line
        public Color Color { get; set; }

        // Texture for end of line segments
        private Texture2D cap;

        // Texture for middle of line
        private Texture2D middle;

        public Line(Game game, SpriteBatch spriteBatch, Vector2 pointA, Vector2 pointB, float thickness, Color color)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            this.PointA = pointA;
            this.PointB = pointB;

            this.Thickness = thickness;

            this.Color = color;

            Initialize();
        }

        /// <summary>
        /// Load the textures needed for the line object.
        /// </summary>
        public override void Initialize()
        {
            cap = game.Content.Load<Texture2D>("Particles\\Lightning\\LeftEnd");
            middle = game.Content.Load<Texture2D>("Particles\\Lightning\\Middle");

            base.Initialize();
        }

        /// <summary>
        /// Allows the line to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the line to draw itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.></param>
        /// <param name="alpha">The transparency of the line</param>
        public void Draw(GameTime gameTime, float alpha)
        {
            Vector2 tangent = PointB - PointA;
            float rotation = (float)Math.Atan2(tangent.Y, tangent.X);

            float thicknessScale = Thickness / ImageThickness;

            Vector2 capOrigin = new Vector2(cap.Width, cap.Height / 2f);
            Vector2 middleOrigin = new Vector2(0, middle.Height / 2f);
            Vector2 middleScale = new Vector2(tangent.Length(), thicknessScale);

            // Draw the line middle
            spriteBatch.Draw(middle, PointA, null, Color*alpha, rotation, middleOrigin, middleScale, SpriteEffects.None, 0f);

            // Draw the ends of the line
            spriteBatch.Draw(cap, PointA, null, Color*alpha, rotation, capOrigin, thicknessScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(cap, PointB, null, Color*alpha, rotation + MathHelper.Pi, capOrigin, thicknessScale, SpriteEffects.None, 0f);
            
            base.Draw(gameTime);
        }
    }
}
