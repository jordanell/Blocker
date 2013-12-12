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
using Blocker.ParticleSystem;
using Blocker.Entities;


namespace Blocker
{
    /// <summary>
    /// The level object is the main gameplay scene used to actually play Block3r.
    /// </summary>
    public class Level : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // Meta information for level
        public int LevelNumber { get; private set; }
        public bool Complete { get; private set; }
        public bool Quit { get; private set; }

        // Level layout
        private const int blockWidth = 40;
        private const int blockHeight = 40;
        private const int hudBuffer = 80;

        // Heads up display
        private HeadsUpDisplay HUD;

        // The map or level layout
        private Block[,] map;
        public Block[,] Map
        {
            get { return map; }
            private set
            {
                map = value;
            }
        }

        // Game objects
        private Player player;
        private Exit exit;

        // Instructions
        private Instruction inst;

        // Lightning
        private LightningController lc;

        // The states a level can be in
        private enum LevelState { Idle, Moving };

        // The level state
        private LevelState state = LevelState.Idle;

        private Timer timer;

        public Level(Game game, SpriteBatch spriteBatch, int levelNumber)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.LevelNumber = levelNumber;

            Initialize(true);
        }

        /// <summary>
        /// Creates the lightning controller, heads up display, calls to load
        /// the current level, and calls to create instructions as needed.
        /// </summary>
        /// <param name="showInstructions">If true, instructions will be shown on level load if they exist.</param>
        public void Initialize(bool showInstructions)
        {
            // Create the lightning controller
            lc = new LightningController(game, spriteBatch);

            /// Create the HUD
            HUD = new HeadsUpDisplay(game, spriteBatch);
            HUD.Level = LevelNumber;

            // Initialize map and load level
            map = new Block[18, 12];
            LoadLevel();

            // Show instructions as needed
            inst = null;
            if(showInstructions)
                LoadInstruction();

            base.Initialize();
        }

        /// <summary>
        /// Loads the LevelNumber level into the map variable.
        /// </summary>
        private void LoadLevel()
        {
            // Get the file name
            String levelFile = "Levels\\level" + Convert.ToString(LevelNumber) + ".lvl";

            // Set up temporary variables
            List<string> textureNames = new List<string>();
            List<List<int>> tempLayout = new List<List<int>>();

            // Create an open stream to the level file and read one line at a time
            Stream stream = TitleContainer.OpenStream(levelFile);
            StreamReader sreader = new System.IO.StreamReader(stream);
            bool readingTextures = false, readingLayout = false, readingFuel = false;
            while (!sreader.EndOfStream)
            {
                string line = sreader.ReadLine().Trim();
                if (string.IsNullOrEmpty(line))
                    continue;
                // Reading textures
                if (line.Contains("[Textures]"))
                {
                    readingTextures = true;
                    readingLayout = false;
                    readingFuel = false;
                }
                // Reading layout
                else if (line.Contains("[Layout]"))
                {
                    readingLayout = true;
                    readingTextures = false;
                    readingFuel = false;
                }
                // Reading fuel
                else if (line.Contains("[Fuel]"))
                {
                    readingFuel = true;
                    readingLayout = false;
                    readingTextures = false;
                }
                // Read the fuel into the HUD
                else if (readingFuel)
                    HUD.Fuel = Convert.ToInt32(line);
                // Read a texture
                else if (readingTextures)
                    textureNames.Add(line);
                // Read a layout row
                else if (readingLayout)
                {
                    List<int> row = new List<int>();
                    string[] split = line.Split(' ');
                    // Parse each element in a row
                    foreach (string block in split)
                    {
                        if (!string.IsNullOrEmpty(block))
                            row.Add(int.Parse(block));
                    }
                    tempLayout.Add(row);
                }
            }
            stream.Close();

            // Generate the map from the temporary variables read from file
            GenerateMap(textureNames, tempLayout);
        }

        /// <summary>
        /// Generates the map from texture and layout input.
        /// </summary>
        /// <param name="textureNames">A list of textures that correspond to a layout.</param>
        /// <param name="tempLayout">The layout of the map.</param>
        private void GenerateMap(List<string> textureNames, List<List<int>> tempLayout)
        {
            // Load all the textures into the game that are needed by map
            List<Texture2D> textures = new List<Texture2D>();
            foreach (string tex in textureNames)
                textures.Add(game.Content.Load<Texture2D>(tex));

            // Add entities to the map as needed from reading the layout
            for(int y = 0; y < tempLayout.Count(); y++)
            {
                for(int x = 0; x < tempLayout[y].Count(); x++)
                {
                    switch(tempLayout[y][x])
                    {
                        case 0:
                            break;
                        case 1:
                            // Add a block
                            map[y,x] = new Block(game, spriteBatch, textures[tempLayout[y][x]-1], 
                                new Rectangle((x*blockWidth), (hudBuffer+(y*blockHeight)), blockWidth, blockHeight));
                            break;
                        case 2:
                            // Add a blue moveable block
                            map[y,x] = new MoveableBlock(game, spriteBatch, textures[tempLayout[y][x]-1], 
                                new Rectangle((x*blockWidth), (hudBuffer+(y*blockHeight)), blockWidth, blockHeight),
                                Color.Blue);
                            break;
                        case 3:
                            // Add a red moveable block
                            map[y, x] = new MoveableBlock(game, spriteBatch, textures[tempLayout[y][x] - 1],
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight),
                                Color.Red);
                            break;
                        case 4:
                            // Add red matter
                            map[y, x] = new Matter(game, spriteBatch, null,
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight),
                                Color.Red);
                            break;
                        case 5:
                            // Add blue matter
                            map[y, x] = new Matter(game, spriteBatch, null,
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight),
                                Color.Blue);
                            break;
                        case 8:
                            // Add the exit
                            exit = new Exit(game, spriteBatch, null,
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight));
                            break;
                        case 9:
                            // Add the player
                            player = new Player(game, spriteBatch,
                                new Rectangle((x * blockWidth), (hudBuffer + (y * blockHeight)), blockWidth, blockHeight),
                                this);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Loads an instruction in for the current level.
        /// </summary>
        private void LoadInstruction()
        {
            switch (LevelNumber)
            {
                case 1:
                case 2:
                case 4:
                case 5:
                case 7:
                    inst = new Instruction(game, spriteBatch, LevelNumber);
                    break;
                default:
                    inst = null;
                    break;
            }
        }

        /// <summary>
        /// Allows the level to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Monitor the back button to quit level
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Quit = true;
                return;
            }

            // Update entities
            exit.Update(gameTime);
            player.Update(gameTime);

            // Handles early input cancelling
            if (timer == null)
                timer = new Timer(game, 500);
            timer.Update(gameTime);
            if (!timer.IsDone())
            {
                InputHandler.Instance.Clear();
                return;
            }

            // Set state of level based on player
            state = LevelState.Idle;
            if (player.State == PlayerState.Moving)
                state = LevelState.Moving;

            // Check for win condition
            if (state == LevelState.Idle)
            {
                if (player.Position.Intersects(exit.Position))
                    Complete = true;
            }

            // Update the HUD
            HUD.Update(gameTime);

            // Check for reset
            if (HUD.Reset)
            {
                // Don't show instructions on reset
                Initialize(false);
                timer = null;
                return;
            }

            // Check for Exit()
            if (HUD.Exit)
            {
                Quit = true;
                return;
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

            // Update lightning effects
            lc.Update(gameTime);
            
            // Handle new gestures
            if (state == LevelState.Idle && inst == null) 
            {
                // Handle player movement
                Direction direction = InputHandler.Instance.DragDirection();
                if (direction != Direction.None)
                    ProcessPlayerMove(direction);

                // Handle block push
                Vector2 doubleTap = InputHandler.Instance.DoubleTap();
                if (doubleTap != Vector2.Zero)
                    ProcessPush(doubleTap);
            }
            // Handle inputs to instructions
            else if (state == LevelState.Idle && inst != null)
            {
                inst.Update(gameTime);
                if (inst.Complete)
                    inst = null;
            }

            // Clear the input this frame
            InputHandler.Instance.Clear();

            base.Update(gameTime);
        }

        /// <summary>
        /// Given a direction, this method applies a movement to the player if possible.
        /// </summary>
        /// <param name="direction">The direction to move the player</param>
        private void ProcessPlayerMove(Direction direction)
        {
            // Where the player started and can move to
            Vector2 origin = new Vector2(
                player.Position.X / blockWidth, (player.Position.Y / blockHeight) - 2);
            Vector2 destination = GetDestination(origin, direction);

            // Only move if the destination is not the origin and they player has fuel
            if (destination.X != -1 && destination != origin && HUD.Fuel > 0)
            {
                // Decrease the fuel
                HUD.DecreaseFuel();

                // Move the player
                Movement movement = new Movement(game,
                    new Rectangle(((int)origin.X * blockWidth), (hudBuffer + ((int)origin.Y * blockHeight)), blockWidth, blockHeight),
                    new Rectangle(((int)destination.X * blockWidth), (hudBuffer + ((int)destination.Y * blockHeight)), blockWidth, blockHeight));
                player.Move(movement);

                // Update the level state and play sound
                state = LevelState.Moving;
                SoundMixer.Instance(game).PlayMove(false);
            }
        }

        /// <summary>
        /// Returns a direction based on the delta vector given.
        /// </summary>
        /// <param name="delta">The vector to turn into a direction.</param>
        /// <returns>The direction being returned.</returns>
        private Direction GetDirection(Vector2 delta)
        {
            // Return a Y direction or X based on absolute value
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

        /// <summary>
        /// Returns a destination vecotr based on an origin and direction of movement.
        /// </summary>
        /// <param name="origin">The origin of a movement.</param>
        /// <param name="direction">The direction of the movement.</param>
        /// <returns>The destination of a movement</returns>
        private Vector2 GetDestination(Vector2 origin, Direction direction)
        {
            // Get the destination based on direction
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

            // Return a bad vector if no destination can be found
            return new Vector2(-1, -1);
        }

        /// <summary>
        /// Given a position of a double tap, this method processes a push attempt on
        /// a moveable block.
        /// </summary>
        /// <param name="position">The position of the double tap.</param>
        private void ProcessPush(Vector2 position)
        {
            // The delta vector between the double tap and the player
            Vector2 delta = new Vector2(position.X - player.Position.X, position.Y - player.Position.Y);

            // Get the direction of the tap, and the target in the direction
            Direction direction = GetDirection(delta);
            Vector2 origin = AcquireTarget(direction);
            Block target = map[(int)origin.Y, (int)origin.X];

            // Only attempt to move a moveable block
            if (target.GetType() == typeof(MoveableBlock))
            {
                // Handle block colors and move
                Color color = ((MoveableBlock)target).Color;
                Movement move = GetBlockMovement(origin, direction);

                // Move based on color if we can
                if (color == Color.Red)
                {
                    if (move != null && HUD.RedMatter > 0)
                        ApplyPush((MoveableBlock)target, move, origin, Color.Red);
                }
                else if (color == Color.Blue)
                {
                    if (move != null && HUD.BlueMatter > 0)
                        ApplyPush((MoveableBlock)target, move, origin, Color.SkyBlue);

                }
            }

        }

        /// <summary>
        /// Applies a movement to a moveable block.
        /// </summary>
        /// <param name="target">The moveable block to be moved.</param>
        /// <param name="move">The movement.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="color">The color of the block.</param>
        private void ApplyPush(MoveableBlock target, Movement move, Vector2 origin, Color color)
        {
            ((MoveableBlock)target).Move(move);
            HUD.DecreaseRedMatter();
            map[(int)origin.Y, (int)origin.X] = null;
            Vector2 dest = GridIndexOf(new Vector2(move.End.X, move.End.Y));
            map[(int)dest.Y, (int)dest.X] = target;
            GenerateLightning(move.Direction, new Vector2(move.Start.X, move.Start.Y), color);
            SoundMixer.Instance(game).PlayShoot(false);
        }

        /// <summary>
        /// Generates a lightning bolt based on the players position and block movement.
        /// </summary>
        /// <param name="direction">The direction of the lightning.</param>
        /// <param name="dest">The destination of the lightning bolt.</param>
        /// <param name="color">The color of the lightning bolt.</param>
        private void GenerateLightning(Direction direction, Vector2 dest, Color color)
        {
            Vector2 source = Vector2.Zero;

            // Get the source based on direction of the bolt. The source is
            // beside the player, on top, or below.
            switch (direction)
            {
                case Direction.Up:
                    source.X = player.Position.X + (blockWidth / 2);
                    source.Y = player.Position.Y;
                    dest.X += blockWidth / 2;
                    dest.Y += blockHeight; 
                    break;
                case Direction.Down:
                    source.X = player.Position.X + (blockWidth / 2);
                    source.Y = player.Position.Y + (blockHeight);
                    dest.X += blockWidth / 2;
                    break;
                case Direction.Left:
                    source.X = player.Position.X;
                    source.Y = player.Position.Y + (blockHeight / 2);
                    dest.X += blockWidth;
                    dest.Y += blockHeight / 2; 
                    break;
                case Direction.Right:
                    source.X = player.Position.X + (blockWidth);
                    source.Y = player.Position.Y + (blockHeight / 2);
                    dest.Y += blockHeight / 2; 
                    break;
            }

            // Create the lightning as long as the source exists
            if (source != Vector2.Zero)
                lc.CreateLightning(source, dest, color);
        }

        /// <summary>
        /// Given a screen position, a location of (x,y) is returned for the grid.
        /// </summary>
        /// <param name="vector">The screen location.</param>
        /// <returns>The (x,y) vector of grid location.</returns>
        private Vector2 GridIndexOf(Vector2 vector)
        {
            return new Vector2(
                vector.X / blockWidth, (vector.Y / blockHeight) - 2);
        }

        /// <summary>
        /// Given a direction, this returns a target location from the player
        /// for a block move.
        /// </summary>
        /// <param name="direction">The direction away from the player.</param>
        /// <returns>The location of the target</returns>
        private Vector2 AcquireTarget(Direction direction)
        {
            // Get origin of player
            Vector2 origin = new Vector2(
                player.Position.X / blockWidth, (player.Position.Y / blockHeight) - 2);

            Vector2 target = new Vector2(-1, -1);

            // Get target based on direction
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

        /// <summary>
        /// Returns a movement for a moveable block based on origin and direction.
        /// </summary>
        /// <param name="origin">The origin of the movement.</param>
        /// <param name="direction">The direction of the movement.</param>
        /// <returns></returns>
        private Movement GetBlockMovement(Vector2 origin, Direction direction)
        {
            // Get the destination of the movement
            Vector2 destination = GetDestination(origin, direction);

            // Create the movement if destination is not origin
            if (destination.X != -1 && destination != origin)
            {
                Movement movement = new Movement(game,
                    new Rectangle(((int)origin.X * blockWidth), (hudBuffer + ((int)origin.Y * blockHeight)), blockWidth, blockHeight),
                    new Rectangle(((int)destination.X * blockWidth), (hudBuffer + ((int)destination.Y * blockHeight)), blockWidth, blockHeight));
                return movement;
            }

            // Return null if movement is bad
            return null;
        }

        /// <summary>
        /// Returns the destination of a movement with direction left
        /// </summary>
        /// <param name="origin">The origin of the movement.</param>
        /// <returns></returns>
        private Vector2 LeftDestination(Vector2 origin)
        {
            for (int x = (int)origin.X - 1; x >= 0; x--)
            {
                if (CellIsOccupied(new Vector2(x, origin.Y)))
                    return new Vector2(x+1, origin.Y);
            }
            return origin;
        }

        /// <summary>
        /// Returns the destination of a movement with direction right
        /// </summary>
        /// <param name="origin">The origin of the movement.</param>
        /// <returns></returns>
        private Vector2 RightDestination(Vector2 origin)
        {
            for (int x = (int)origin.X + 1; x < map.GetLength(1); x++)
            {
                if (CellIsOccupied(new Vector2(x, origin.Y)))
                    return new Vector2(x-1, origin.Y);
            }
            return origin;
        }

        /// <summary>
        /// Returns the destination of a movement with direction up
        /// </summary>
        /// <param name="origin">The origin of the movement.</param>
        /// <returns></returns>
        private Vector2 UpDestination(Vector2 origin)
        {
            for (int y = (int)origin.Y - 1; y >= 0; y--)
            {
                if (CellIsOccupied(new Vector2(origin.X, y)))
                    return new Vector2(origin.X, y+1);
            }
            return origin;
        }

        /// <summary>
        /// Returns the destination of a movement with direction down
        /// </summary>
        /// <param name="origin">The origin of the movement.</param>
        /// <returns></returns>
        private Vector2 DownDestination(Vector2 origin)
        {
            for (int y = (int)origin.Y + 1; y < map.GetLength(0); y++)
            {
                if (CellIsOccupied(new Vector2(origin.X, y)))
                    return new Vector2(origin.X, y-1);
            }
            return origin;
        }

        /// <summary>
        /// Returns true if a cell is occupied by a entitiy.
        /// </summary>
        /// <param name="cell">The cell to check.</param>
        /// <returns>The boolean result.</returns>
        private bool CellIsOccupied(Vector2 cell)
        {
            return (map[(int)cell.Y, (int)cell.X] != null && 
                (map[(int)cell.Y, (int)cell.X].GetType() != typeof(Matter)) &&
                (map[(int)cell.Y, (int)cell.X].GetType() != typeof(Exit)));
        }

        /// <summary>
        /// Add matter to the HUD at the given grid location.
        /// </summary>
        /// <param name="x">The x grid coordinate.</param>
        /// <param name="y">The y grid coordinate.</param>
        public void AddMatterAt(int x, int y)
        {
            // The matter at the coordinate
            Matter matter = (Matter)map[y, x];

            // Add to HUD
            if (matter.Color == Color.Red)
                HUD.AddRedMatter();
            else if (matter.Color == Color.Blue)
                HUD.AddBlueMatter();

            // Delete matter from map
            map[y, x] = null;

            // Play matter sound
            SoundMixer.Instance(game).PlayMatter(false);
        }

        /// <summary>
        /// Allows the level to draw itself.
        /// </summary>
        /// <param name="gameTime">A representation of game time.</param>
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

            // Draw lightning
            lc.Draw(gameTime);

            // Draw the exit
            exit.Draw(gameTime);

            // Draw player
            player.Draw(gameTime);

            // Draw instructions
            if (inst != null)
                inst.Draw(gameTime);


            base.Draw(gameTime);
        }
    }
}
