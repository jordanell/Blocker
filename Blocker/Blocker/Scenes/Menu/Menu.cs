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
using System.IO.IsolatedStorage;
using Blocker.Handlers;


namespace Blocker
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private enum MenuState { Main, LevelSelect, Instructions, Settings };
        private MenuState state = MenuState.Main;

        // Title logo
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

        // Level loading
        public bool LoadLevel { get; private set; }
        public int LevelNumber { get; private set; }

        public Menu(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            Initialize();
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

            SpriteFont menuFontBig = game.Content.Load<SpriteFont>("Fonts\\Pericles36");
            SpriteFont menuFontSmall = game.Content.Load<SpriteFont>("Fonts\\Pericles28");

            playButton = new Button(game, spriteBatch, new Rectangle(103, 275, 275, 70), blueButton, menuFontBig, "Play");
            levelSelectButton = new Button(game, spriteBatch, new Rectangle(103, 375, 275, 70), blueButton, menuFontSmall, "Level Select");
            settingsButton = new Button(game, spriteBatch, new Rectangle(103, 475, 275, 70), yellowButton, menuFontBig, "Settings");
            exitButton = new Button(game, spriteBatch, new Rectangle(103, 575, 275, 70), redButton, menuFontBig, "Exit");
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

            SpriteFont menuFontBig = game.Content.Load<SpriteFont>("Fonts\\Pericles36");
            SpriteFont menuFontSmall = game.Content.Load<SpriteFont>("Fonts\\Pericles28");

            sound = new Label(game, spriteBatch, new Rectangle(103, 275, 275, 70), menuFontSmall, "Play with sound?");

            soundYes = new Button(game, spriteBatch, new Rectangle(103, 375, 122, 70), yellowButton, menuFontBig, "Yes");

            soundNo = new Button(game, spriteBatch, new Rectangle(255, 375, 122, 70), blueButton, menuFontBig, "No");

            reset = new Button(game, spriteBatch, new Rectangle(103, 575, 275, 70), redButton, menuFontSmall, "Reset Levels");
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
                    UpdateMainMenu(gameTime);
                    break;

                case MenuState.LevelSelect:
                    UpdateLevelSelection(gameTime);
                    break;

                case MenuState.Instructions:
                    UpdateInstructions();
                    break;

                case MenuState.Settings:
                    UpdateSettings(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateMainMenu(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                game.Exit();
            playButton.Update(gameTime);
            levelSelectButton.Update(gameTime);
            settingsButton.Update(gameTime);
            exitButton.Update(gameTime);

            if (playButton.State == TouchButtonState.Clicked)
            {
                UnloadMainMenu();
                LoadLevel = true;
                LevelNumber = FileHandler.TopLevel() + 1;
                if (LevelNumber == 26)
                    LevelNumber = 25;
            }

            else if (levelSelectButton.State == TouchButtonState.Clicked)
            {
                state = MenuState.LevelSelect;
                UnloadMainMenu();
                LoadLevelSelect();
            }

            else if (settingsButton.State == TouchButtonState.Clicked)
            {
                state = MenuState.Settings;
                UnloadMainMenu();
                LoadSettings();
            }

            else if (exitButton.State == TouchButtonState.Clicked)
                game.Exit();
        }

        private void UpdateLevelSelection(GameTime gameTime)
        {
            selector.Update(gameTime);

            if (selector.LoadLevel)
            {
                LoadLevel = true;
                LevelNumber = selector.LevelNumber;
            }

            // Monitor the back button
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                state = MenuState.Main;
                UnloadLevelSelect();
                LoadMainMenu();
            }
        }

        private void UpdateInstructions()
        {
            // Monitor the back button
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                state = MenuState.Main;
                UnloadLevelSelect();
                LoadMainMenu();
            }
        }

        private void UpdateSettings(GameTime gameTime)
        {
            soundYes.Update(gameTime);
            soundNo.Update(gameTime);
            reset.Update(gameTime);

            if (reset.State == TouchButtonState.Clicked)
                FileHandler.ResetLevels();

            if (soundYes.State == TouchButtonState.Clicked)
            {
                if (SoundMixer.Instance(game).Muted)
                    SoundMixer.Instance(game).PlayEffectForce("Audio\\Button");
                SoundMixer.Instance(game).Muted = false;
            }

            if (soundNo.State == TouchButtonState.Clicked)
                SoundMixer.Instance(game).Muted = true;

            // Monitor the back button
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                state = MenuState.Main;
                UnloadSettings();
                LoadMainMenu();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the static title
            spriteBatch.Begin();
            spriteBatch.Draw(title, new Vector2(0, 90), Color.White);
            spriteBatch.End();

            // Draw based on state
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
