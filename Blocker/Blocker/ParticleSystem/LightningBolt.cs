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
    /// This is a game component that implements IUpdateable. The lightning bolt object 
    /// is a series of lines which are used to create a lightning bolt effect.
    /// </summary>
    public class LightningBolt : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Transparency of the bolt to be used for death animation
        private float alpha;
        private float fadeOutRate;

        // Complete flag
        public bool Complete { get { return alpha <= 0; } }

        // Lines that make up lightning bolt
        private List<Line> segments = new List<Line>();

        public LightningBolt(Game game, SpriteBatch spriteBatch, Vector2 source, Vector2 dest, Color color)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            // Call to create the lines for the bolt
            segments = CreateBolt(source, dest, 4, color);

            alpha = 1f;
            fadeOutRate = 0.03f;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Creates a list of line that become a lightning bolt.
        /// </summary>
        /// <param name="source">The start of the lightning bolt.</param>
        /// <param name="dest">The end of the lightning bolt.</param>
        /// <param name="thickness">The thickness of the lightning bolt.</param>
        /// <param name="color">The color of the lightning bolt.</param>
        /// <returns>The line segments of the lightning bolt.</returns>
        private List<Line> CreateBolt(Vector2 source, Vector2 dest, float thickness, Color color)
        {
            var results = new List<Line>();
            Vector2 tangent = dest - source;
            Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
            float length = tangent.Length();

            List<float> positions = new List<float>();
            positions.Add(0);

            Random r = new Random();

            for (int i = 0; i < length / 4; i++)
                positions.Add((float)r.NextDouble());

            positions.Sort();

            const float Sway = 80;
            const float Jaggedness = 1 / Sway;

            Vector2 prevPoint = source;
            float prevDisplacement = 0;
            for (int i = 1; i < positions.Count; i++)
            {
                float pos = positions[i];

                // Used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
                float scale = (length * Jaggedness) * (pos - positions[i - 1]);

                // Defines an envelope. Points near the middle of the bolt can be further from the central line.
                float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

                float displacement = r.Next((int)-Sway, (int)Sway);
                displacement -= (displacement - prevDisplacement) * (1 - scale);
                displacement *= envelope;

                Vector2 point = source + pos * tangent + displacement * normal;
                results.Add(new Line(game, spriteBatch, prevPoint, point, thickness, color));
                prevPoint = point;
                prevDisplacement = displacement;
            }

            results.Add(new Line(game, spriteBatch, prevPoint, dest, thickness, color));

            return results;
        }

        /// <summary>
        /// Allows the lightning bolt to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update alhpa for death
            alpha -= fadeOutRate;

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the lightning bolt to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Don't draw when dead
            if (Complete)
                return;

            // Draw each line segment
            // We need to change the draw mode so we have to begin spritebatch
            // here instead of in the line itself.
            foreach (Line segment in segments)
            {
                spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
                segment.Draw(gameTime, alpha);
                spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }
    }
}
