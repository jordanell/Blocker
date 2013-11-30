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
using Microsoft.Xna.Framework.Input.Touch;


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
        public bool complete;

        private int blockWidth = 40;
        private int blockHeight = 40;

        private int hudBuffer = 80;

        private HeadsUpDisplay HUD;

        public Block[,] map = new Block[18,12];

        private Player player;

        private enum LevelState { Idle, Moving };

        private LevelState state = LevelState.Idle;

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
                        case 9:
                            player = new Player(game, spriteBatch,
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight),
                                this);
                            player.Initialize();
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
            state = LevelState.Idle;
            if (player.Getstate() == PlayerState.Moving)
                state = LevelState.Moving;
            
            // Handle new gestures
            while (TouchPanel.IsGestureAvailable)
            {
                if (state != LevelState.Idle)
                    break;

                GestureSample gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    case GestureType.VerticalDrag:
                    case GestureType.HorizontalDrag:
                        //ProcessGesture(gs.Delta * 10);
                        break;
                    case GestureType.Flick:
                        ProcessGesture(gs.Delta);
                        break;
                }

            }

            player.Update(gameTime);

            base.Update(gameTime);
        }

        private Movement ProcessGesture(Vector2 delta)
        {
            Vector2 origin = new Vector2(
                player.GetPosition().X / blockWidth, (player.GetPosition().Y / blockHeight) - 2);

            // Get direction to move
            MovementDirection direction;
            if (Math.Abs(delta.Y) >= Math.Abs(delta.X))
            {
                if (delta.Y < 0)
                    direction = MovementDirection.Up;
                else
                    direction = MovementDirection.Down;
            }
            else
            {
                if (delta.X < 0)
                    direction = MovementDirection.Left;
                else
                    direction = MovementDirection.Right;
            }

            Vector2 destination = new Vector2(-1, -1);
            switch (direction)
            {
                case MovementDirection.Left:
                    destination = LeftDestination(origin);
                    break;
                case MovementDirection.Right:
                    destination = RightDestination(origin);
                    break;
                case MovementDirection.Up:
                    destination = UpDestination(origin);
                    break;
                case MovementDirection.Down:
                    destination = DownDestination(origin);
                    break;
                default:
                    break;
            }

            if (destination.X != -1 && destination != origin)
            {
                Movement movement = new Movement(game,
                    new Rectangle(((int)origin.X * blockWidth), (hudBuffer + ((int)origin.Y * blockHeight)), blockWidth, blockHeight),
                    new Rectangle(((int)destination.X * blockWidth), (hudBuffer + ((int)destination.Y * blockHeight)), blockWidth, blockHeight));
                movement.Initialize();

                player.Move(movement);
                state = LevelState.Moving;
            }


            return null;
        }

        private Vector2 LeftDestination(Vector2 origin)
        {
            for (int x = (int)origin.X - 1; x >= 0; x--)
            {
                if (CellIsOccupied(new Vector2(x, origin.Y)))
                    return new Vector2(x+1, origin.Y);
            }
            return origin;
        }

        private Vector2 RightDestination(Vector2 origin)
        {
            for (int x = (int)origin.X + 1; x < map.GetLength(1); x++)
            {
                if (CellIsOccupied(new Vector2(x, origin.Y)))
                    return new Vector2(x-1, origin.Y);
            }
            return origin;
        }

        private Vector2 UpDestination(Vector2 origin)
        {
            for (int y = (int)origin.Y - 1; y >= 0; y--)
            {
                if (CellIsOccupied(new Vector2(origin.X, y)))
                    return new Vector2(origin.X, y+1);
            }
            return origin;
        }

        private Vector2 DownDestination(Vector2 origin)
        {
            for (int y = (int)origin.Y + 1; y < map.GetLength(0); y++)
            {
                if (CellIsOccupied(new Vector2(origin.X, y)))
                    return new Vector2(origin.X, y-1);
            }
            return origin;
        }

        private bool CellIsOccupied(Vector2 cell)
        {
            return (map[(int)cell.Y, (int)cell.X] != null && 
                (map[(int)cell.Y, (int)cell.X].GetType() != typeof(Matter)));
        }

        private void ProcessTouch()
        {
            // TODO for moving blocks
        }

        public void AddMatterAt(int x, int y)
        {
            Matter matter = (Matter)map[y, x];
            if (matter.color == Color.Red)
                HUD.AddRedMatter();
            else if (matter.color == Color.Blue)
                HUD.AddBlueMatter();

            // Delete matter from map
            map[y, x] = null;
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

            // Draw player
            player.Draw(gameTime);


            base.Draw(gameTime);
        }
    }
}
