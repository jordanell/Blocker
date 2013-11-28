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
        private Button playButton;
        private Button levelSelectButton;
        private Button settingsButton;
        private Button exitButton;

        // Level selector
        private LevelSelector selector;

        // Settings
        private Label sound;
        private Button soundYes;
        private Button soundNo;
        private Button reset;

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
            Texture2D yellowButton = game.Content.Load<Texture2D>("Buttons\\YellowButton");
            Texture2D blueButton = game.Content.Load<Texture2D>("Buttons\\BlueButton");
            Texture2D redButton = game.Content.Load<Texture2D>("Buttons\\RedButton");

            SpriteFont menuFont = game.Content.Load<SpriteFont>("Fonts\\Pericles36");
            SpriteFont menuFont2 = game.Content.Load<SpriteFont>("Fonts\\Pericles28");

            playButton = new Button(game, spriteBatch, new Rectangle(103, 375, 275, 70), blueButton, menuFont, "Play");
            playButton.Initialize();

            levelSelectButton = new Button(game, spriteBatch, new Rectangle(103, 475, 275, 70), blueButton, menuFont2, "Level Select");
            levelSelectButton.Initialize();

            settingsButton = new Button(game, spriteBatch, new Rectangle(103, 575, 275, 70), yellowButton, menuFont, "Settings");
            settingsButton.Initialize();

            exitButton = new Button(game, spriteBatch, new Rectangle(103, 675, 275, 70), redButton, menuFont, "Exit");
            exitButton.Initialize();
        }

        private void UnloadMainMenu()
        {
            playButton = null;
            levelSelectButton = null;
            settingsButton = null;
            exitButton = null;
        }

        private void LoadLevelSelect()
        {
            selector = new LevelSelector(game, spriteBatch);
            selector.Initialize();
        }

        private void UnloadLevelSelect()
        {
            selector = null;
        }

        private void LoadSettings()
        {
            Texture2D yellowButton = game.Content.Load<Texture2D>("Buttons\\YellowButton");
            Texture2D blueButton = game.Content.Load<Texture2D>("Buttons\\BlueButton");
            Texture2D redButton = game.Content.Load<Texture2D>("Buttons\\RedButton");

            SpriteFont menuFont = game.Content.Load<SpriteFont>("Fonts\\Pericles36");
            SpriteFont menuFont2 = game.Content.Load<SpriteFont>("Fonts\\Pericles28");

            sound = new Label(game, spriteBatch, new Rectangle(103, 375, 275, 70), menuFont2, "Play with sound?");
            sound.Initialize();

            soundYes = new Button(game, spriteBatch, new Rectangle(103, 475, 122, 70), yellowButton, menuFont, "Yes");
            soundYes.Initialize();

            soundNo = new Button(game, spriteBatch, new Rectangle(255, 475, 122, 70), blueButton, menuFont, "No");
            soundNo.Initialize();

            reset = new Button(game, spriteBatch, new Rectangle(103, 675, 275, 70), redButton, menuFont2, "Reset Levels");
            reset.Initialize();
        }

        private void UnloadSettings()
        {
            soundYes = null;
            soundNo = null;
            reset = null;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case MenuState.Main:
                    playButton.Update(gameTime);
                    levelSelectButton.Update(gameTime);
                    settingsButton.Update(gameTime);
                    exitButton.Update(gameTime);

                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        game.Exit();

                    if (playButton.state == TouchButtonState.Clicked)
                    {
                        UnloadMainMenu();
                        LoadMainMenu();
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
                    break;

                case MenuState.LevelSelect:
                    selector.Update(gameTime);

                    // Monitor the back button
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    {
                        state = MenuState.Main;
                        UnloadLevelSelect();
                        LoadMainMenu();
                    }
                    break;

                case MenuState.Settings:
                    soundYes.Update(gameTime);
                    soundNo.Update(gameTime);
                    reset.Update(gameTime);

                    // Monitor the back button
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    {
                        state = MenuState.Main;
                        UnloadSettings();
                        LoadMainMenu();
                    }

                    break;
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

            switch (state)
            {
                case MenuState.Main:
                    playButton.Draw(gameTime);
                    levelSelectButton.Draw(gameTime);
                    settingsButton.Draw(gameTime);
                    exitButton.Draw(gameTime);
                    break;
                
                case MenuState.LevelSelect:
                    selector.Draw(gameTime);
                    break;

                case MenuState.Settings:
                    sound.Draw(gameTime);
                    soundYes.Draw(gameTime);
                    soundNo.Draw(gameTime);
                    reset.Draw(gameTime);

                    break;
            }

            base.Draw(gameTime);
        }
    }
}
