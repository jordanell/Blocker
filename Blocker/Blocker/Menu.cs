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
using Microsoft.Xna.Framework.Input.Touch;


namespace Blocker
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private enum MenuState { Main, LevelSelect, Settings };
        private MenuState state = MenuState.Main;

        private Texture2D title;

        // Buttons
        private Button startButton;
        private Button levelSelectButton;
        private Button settingsButton;
        private Button exitButton;

        public Menu(Game game, SpriteBatch spriteBatch)
            : base(game)
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
            title = game.Content.Load<Texture2D>("Logo");

            LoadMainMenu();

            base.Initialize();
        }

        private void LoadMainMenu()
        {
            Texture2D yellowButton = game.Content.Load<Texture2D>("YellowButton");
            Texture2D blueButton = game.Content.Load<Texture2D>("BlueButton");

            SpriteFont menuFont = game.Content.Load<SpriteFont>("Fonts\\Pericles36");
            SpriteFont menuFont2 = game.Content.Load<SpriteFont>("Fonts\\Pericles28");

            startButton = new Button(game, spriteBatch, new Rectangle(103, 375, 275, 70), blueButton, menuFont, "Play");
            startButton.Initialize();

            levelSelectButton = new Button(game, spriteBatch, new Rectangle(103, 475, 275, 70), blueButton, menuFont2, "Level Select");
            levelSelectButton.Initialize();

            settingsButton = new Button(game, spriteBatch, new Rectangle(103, 575, 275, 70), yellowButton, menuFont, "Settings");
            settingsButton.Initialize();

            exitButton = new Button(game, spriteBatch, new Rectangle(103, 675, 275, 70), yellowButton, menuFont, "Exit");
            exitButton.Initialize();
        }

        private void UnloadMainMenu()
        {
            startButton = null;
            levelSelectButton = null;
            settingsButton = null;
            exitButton = null;
        }

        private void LoadLevelSelect()
        {

        }

        private void LoadSettings()
        {

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (state == MenuState.Main)
            {
                startButton.Update(gameTime);
                levelSelectButton.Update(gameTime);
                settingsButton.Update(gameTime);
                exitButton.Update(gameTime);

                if (startButton.state == TouchButtonState.Clicked)
                {

                }

                else if (levelSelectButton.state == TouchButtonState.Clicked)
                {
                    state = MenuState.LevelSelect;
                    UnloadMainMenu();
                    LoadLevelSelect();
                }

                else if (settingsButton.state == TouchButtonState.Clicked)
                {
                    state = MenuState.Settings;
                    UnloadMainMenu();
                    LoadSettings();
                }

                else if (exitButton.state == TouchButtonState.Clicked)
                    game.Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the static title
            spriteBatch.Begin();
            spriteBatch.Draw(title, new Vector2(88, 190), Color.White);
            spriteBatch.End();

            if (state == MenuState.Main)
            {
                startButton.Draw(gameTime);
                levelSelectButton.Draw(gameTime);
                settingsButton.Draw(gameTime);
                exitButton.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
