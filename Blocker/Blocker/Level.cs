using System;
using System.IO;
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
    public class Level : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private int levelNumber;

        private int blockWidth = 40;
        private int blockHeight = 40;

        private int hudBuffer = 80;

        private HeadsUpDisplay HUD;

        private Block[,] map = new Block[18,12];

        public Level(Game game, SpriteBatch spriteBatch, int levelNumber)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.levelNumber = levelNumber;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            LoadLevel();

            HUD = new HeadsUpDisplay(game, spriteBatch);
            HUD.Initialize();

            base.Initialize();
        }

        private void LoadLevel()
        {
            String levelFile = "Levels\\level" + Convert.ToString(levelNumber) + ".level";

            List<string> textureNames = new List<string>();
            List<List<int>> tempLayout = new List<List<int>>();

            Stream stream = TitleContainer.OpenStream(levelFile);
            StreamReader sreader = new System.IO.StreamReader(stream);
            bool readingTextures = false;
            bool readingLayout = false;
            while (!sreader.EndOfStream)
            {
                string line = sreader.ReadLine().Trim();
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line.Contains("[Textures]"))
                {
                    readingTextures = true;
                    readingLayout = false;
                }
                else if (line.Contains("[Layout]"))
                {
                    readingLayout = true;
                    readingTextures = false;
                }
                else if (readingTextures)
                    textureNames.Add(line);
                else if (readingLayout)
                {
                    List<int> row = new List<int>();
                    string[] split = line.Split(' ');
                    foreach (string block in split)
                    {
                        if (!string.IsNullOrEmpty(block))
                            row.Add(int.Parse(block));
                    }
                    tempLayout.Add(row);
                }
            }
            stream.Close();

            GenerateMap(textureNames, tempLayout);
        }

        private void GenerateMap(List<string> textureNames, List<List<int>> tempLayout)
        {
            List<Texture2D> textures = new List<Texture2D>();
            foreach (string tex in textureNames)
                textures.Add(game.Content.Load<Texture2D>(tex));

            for(int y = 0; y < tempLayout.Count(); y++)
            {
                for(int x = 0; x < tempLayout[y].Count(); x++)
                {
                    switch(tempLayout[y][x])
                    {
                        case 0:
                            break;
                        case 1:
                            map[y,x] = new Block(game, spriteBatch, textures[tempLayout[y][x]-1], 
                                new Rectangle((x*blockWidth), (hudBuffer+(y*blockHeight)), blockWidth, blockHeight));
                            break;
                        case 2:
                            map[y,x] = new MoveableBlock(game, spriteBatch, textures[tempLayout[y][x]-1], 
                                new Rectangle((x*blockWidth), (hudBuffer+(y*blockHeight)), blockWidth, blockHeight),
                                Color.Blue);
                            break;
                        case 3:
                            map[y, x] = new MoveableBlock(game, spriteBatch, textures[tempLayout[y][x] - 1],
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight),
                                Color.Red);
                            break;
                        case 4:
                            map[y, x] = new Matter(game, spriteBatch, null,
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight),
                                Color.Red);
                            map[y, x].Initialize();
                            break;
                        case 5:
                            map[y, x] = new Matter(game, spriteBatch, null,
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight),
                                Color.Blue);
                            map[y, x].Initialize();
                            break;
                    }
                }
            }
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

        public override void Draw(GameTime gameTime)
        {
            // Draw HUD
            HUD.Draw(gameTime);

            // Draw level
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != null)
                        map[y, x].Draw(gameTime);
                }
            }


            base.Draw(gameTime);
        }
    }
}
