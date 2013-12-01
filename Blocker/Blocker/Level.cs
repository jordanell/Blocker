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

        // Meta information for level
        private int levelNumber;
        public int LevelNumber { get { return levelNumber; } private set { levelNumber = value; } }
        private bool complete;
        public bool Complete { get { return complete; } private set { complete = value; } }
        private bool quit;
        public bool Quit { get { return quit; } private set { quit = value; } }

        // Level layout
        private int blockWidth = 40;
        private int blockHeight = 40;
        private int hudBuffer = 80;

        // Level elements
        private HeadsUpDisplay HUD;
        private Block[,] map;
        public Block[,] Map { get { return map; } private set { map = value; } }

        // Game objects
        private Player player;
        private Exit exit;

        private enum LevelState { Idle, Moving };
        private LevelState state = LevelState.Idle;

        private Timer timer;

        public Level(Game game, SpriteBatch spriteBatch, int levelNumber)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.levelNumber = levelNumber;

            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            HUD = new HeadsUpDisplay(game, spriteBatch);
            HUD.Level = levelNumber;

            map = new Block[18, 12];
            LoadLevel();

            base.Initialize();
        }

        private void LoadLevel()
        {
            String levelFile = "Levels\\level" + Convert.ToString(levelNumber) + ".lvl";

            List<string> textureNames = new List<string>();
            List<List<int>> tempLayout = new List<List<int>>();

            Stream stream = TitleContainer.OpenStream(levelFile);
            StreamReader sreader = new System.IO.StreamReader(stream);
            bool readingTextures = false, readingLayout = false, readingFuel = false;
            while (!sreader.EndOfStream)
            {
                string line = sreader.ReadLine().Trim();
                if (string.IsNullOrEmpty(line))
                    continue;
                if (line.Contains("[Textures]"))
                {
                    readingTextures = true;
                    readingLayout = false;
                    readingFuel = false;
                }
                else if (line.Contains("[Layout]"))
                {
                    readingLayout = true;
                    readingTextures = false;
                    readingFuel = false;
                }
                else if (line.Contains("[Fuel]"))
                {
                    readingFuel = true;
                    readingLayout = false;
                    readingTextures = false;
                }
                else if (readingFuel)
                    HUD.Fuel = Convert.ToInt32(line);
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
                        case 8:
                            exit = new Exit(game, spriteBatch, null,
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight));
                            exit.Initialize();
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
            // Handles early input cancelling
            if (timer == null)
                timer = new Timer(game, 1000);
            timer.Update(gameTime);
            if (!timer.IsDone())
                return; 

            state = LevelState.Idle;
            if (player.Getstate() == PlayerState.Moving)
                state = LevelState.Moving;

            // Check for win condition
            if (state == LevelState.Idle)
            {
                if (player.GetPosition().Intersects(exit.GetPosition()))
                    complete = true;
            }

            // Update blocks
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] != null)
                        map[y, x].Update(gameTime);
                }
            }
            
            // Handle new gestures
            if (state == LevelState.Idle) 
            {
                while (TouchPanel.IsGestureAvailable)
                {
                    if (state != LevelState.Idle)
                        break;

                    GestureSample gs = TouchPanel.ReadGesture();
                    switch (gs.GestureType)
                    {
                        case GestureType.VerticalDrag:
                        case GestureType.HorizontalDrag:
                        case GestureType.FreeDrag:
                            if (SignificantDrag(gs))
                                ProcessPlayerMove(gs.Delta);
                            break;
                        case GestureType.Flick:
                            ProcessPlayerMove(gs.Delta);
                            break;
                        case GestureType.DoubleTap:
                            ProcessPush(gs.Position);
                            break;
                    }
                }
            }

            HUD.Update(gameTime);

            // Check for reset
            if (HUD.Reset)
            {
                Initialize();
                timer = null;
            }

            // Check for Exit()
            if (HUD.Exit)
                Quit = true;

            exit.Update(gameTime);
            player.Update(gameTime);

            base.Update(gameTime);
        }

        private bool SignificantDrag(GestureSample gs)
        {
            return (Math.Abs(gs.Delta.X) > 5 || Math.Abs(gs.Delta.Y) > 5);
        }

        private void ProcessPlayerMove(Vector2 delta)
        {
            Vector2 origin = new Vector2(
                player.GetPosition().X / blockWidth, (player.GetPosition().Y / blockHeight) - 2);

            // Get direction and destination of move
            Direction direction = GetDirection(delta);
            Vector2 destination = GetDestination(origin, direction);

            if (destination.X != -1 && destination != origin && HUD.Fuel > 0)
            {
                HUD.DecreaseFuel();
                Movement movement = new Movement(game,
                    new Rectangle(((int)origin.X * blockWidth), (hudBuffer + ((int)origin.Y * blockHeight)), blockWidth, blockHeight),
                    new Rectangle(((int)destination.X * blockWidth), (hudBuffer + ((int)destination.Y * blockHeight)), blockWidth, blockHeight));
                movement.Initialize();

                player.Move(movement);
                state = LevelState.Moving;
            }
        }

        private Direction GetDirection(Vector2 delta)
        {
            if (Math.Abs(delta.Y) >= Math.Abs(delta.X))
            {
                if (delta.Y < 0)
                    return Direction.Up;
                else
                    return Direction.Down;
            }
            else
            {
                if (delta.X < 0)
                    return Direction.Left;
                else
                    return Direction.Right;
            }
        }

        private Vector2 GetDestination(Vector2 origin, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return LeftDestination(origin);
                case Direction.Right:
                    return RightDestination(origin);
                case Direction.Up:
                    return UpDestination(origin);
                case Direction.Down:
                    return DownDestination(origin);
            }

            return new Vector2(-1, -1);
        }

        private void ProcessPush(Vector2 position)
        {
            Vector2 delta = new Vector2(position.X - player.GetPosition().X, position.Y - player.GetPosition().Y);

            Direction direction = GetDirection(delta);
            Vector2 origin = AcquireTarget(delta, direction);
            Block target = map[(int)origin.Y, (int)origin.X];

            if (target.GetType() == typeof(MoveableBlock))
            {
                Color color = ((MoveableBlock)target).color;
                if (color == Color.Red)
                {
                    Movement move = GetBlockMovement(origin, direction);
                    if (move != null && HUD.RedMatter > 0)
                    {
                        ((MoveableBlock)target).Move(move);
                        HUD.DecreaseRedMatter();
                        map[(int)origin.Y, (int)origin.X] = null;
                        Vector2 dest = GridIndexOf(new Vector2(move.end.X, move.end.Y));
                        map[(int)dest.Y, (int)dest.X] = target;
                    }
                }
                else if (color == Color.Blue)
                {
                    Movement move = GetBlockMovement(origin, direction);
                    if (move != null && HUD.BlueMatter > 0)
                    {
                        ((MoveableBlock)target).Move(move);
                        HUD.DecreaseBlueMatter();
                        map[(int)origin.Y, (int)origin.X] = null;
                        Vector2 dest = GridIndexOf(new Vector2(move.end.X, move.end.Y));
                        map[(int)dest.Y, (int)dest.X] = target;
                    }

                }
            }

        }

        private Vector2 GridIndexOf(Vector2 vector)
        {
            return new Vector2(
                vector.X / blockWidth, (vector.Y / blockHeight) - 2);
        }

        private Vector2 AcquireTarget(Vector2 delta, Direction direction)
        {
            Vector2 origin = new Vector2(
                player.GetPosition().X / blockWidth, (player.GetPosition().Y / blockHeight) - 2);

            Vector2 target = new Vector2(-1, -1);
            switch (direction)
            {
                case Direction.Left:
                    target = LeftDestination(origin);
                    target.X -= 1;
                    break;
                case Direction.Right:
                    target = RightDestination(origin);
                    target.X += 1;
                    break;
                case Direction.Up:
                    target = UpDestination(origin);
                    target.Y -= 1;
                    break;
                case Direction.Down:
                    target = DownDestination(origin);
                    target.Y += 1;
                    break;
                default:
                    break;
            }
            return target;
        }

        private Movement GetBlockMovement(Vector2 origin, Direction direction)
        {
            Vector2 destination = GetDestination(origin, direction);
            if (destination.X != -1 && destination != origin)
            {
                Movement movement = new Movement(game,
                    new Rectangle(((int)origin.X * blockWidth), (hudBuffer + ((int)origin.Y * blockHeight)), blockWidth, blockHeight),
                    new Rectangle(((int)destination.X * blockWidth), (hudBuffer + ((int)destination.Y * blockHeight)), blockWidth, blockHeight));
                movement.Initialize();

                return movement;
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
                (map[(int)cell.Y, (int)cell.X].GetType() != typeof(Matter)) &&
                (map[(int)cell.Y, (int)cell.X].GetType() != typeof(Exit)));
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

            // Draw the exit
            exit.Draw(gameTime);

            // Draw player
            player.Draw(gameTime);


            base.Draw(gameTime);
        }
    }
}
