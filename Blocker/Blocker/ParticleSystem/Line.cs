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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Line : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        public Vector2 PointA { get; set; }
        public Vector2 PointB { get; set; }

        public float Thickness { get; set; }
        private const float ImageThickness = 3;

        public Color Color { get; set; }

        private Texture2D leftCap;
        private Texture2D rightCap;
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
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            leftCap = game.Content.Load<Texture2D>("Particles\\Lightning\\LeftEnd");
            rightCap = game.Content.Load<Texture2D>("Particles\\Lightning\\RightEnd");
            middle = game.Content.Load<Texture2D>("Particles\\Lightning\\Middle");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, float alpha)
        {
            Vector2 tangent = PointB - PointA;
            float rotation = (float)Math.Atan2(tangent.Y, tangent.X);

            float thicknessScale = Thickness / ImageThickness;

            Vector2 capOrigin = new Vector2(leftCap.Width, leftCap.Height / 2f);
            Vector2 middleOrigin = new Vector2(0, middle.Height / 2f);
            Vector2 middleScale = new Vector2(tangent.Length(), thicknessScale);

            spriteBatch.Draw(middle, PointA, null, Color*alpha, rotation, middleOrigin, middleScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(leftCap, PointA, null, Color*alpha, rotation, capOrigin, thicknessScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(leftCap, PointB, null, Color*alpha, rotation + MathHelper.Pi, capOrigin, thicknessScale, SpriteEffects.None, 0f);
            
            base.Draw(gameTime);
        }
    }
}
