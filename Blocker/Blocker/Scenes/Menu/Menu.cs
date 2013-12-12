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
    /// This is a game component that implements IUpdateable. The menu object is the 
    /// main menu as part of the Block3r game.
    /// </summary>
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // Xna components
        private Game game;
        private SpriteBatch spriteBatch;

        // The possible states the menu can be in
        private enum MenuState { Main, LevelSelect, Settings };

        // The menu state
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
        /// Load the logo texture and initialize the main menu.
        /// </summary>
        public override void Initialize()
        {
            title = game.Content.Load<Texture2D>("Logo");

            LoadMainMenu();

            base.Initialize();
        }

        /// <summary>
        /// Loads all components needed to display the main menu
        /// </summary>
        private void LoadMainMenu()
        {
            // Load textures needed
            Texture2D yellowButton = game.Content.Load<Texture2D>("Buttons\\YellowButton");
            Texture2D blueButton = game.Content.Load<Texture2D>("Buttons\\BlueButton");
            Texture2D redButton = game.Content.Load<Texture2D>("Buttons\\RedButton");

            // Load fonts needed
            SpriteFont menuFontBig = game.Content.Load<SpriteFont>("Fonts\\Pericles36");
            SpriteFont menuFontSmall = game.Content.Load<SpriteFont>("Fonts\\Pericles28");

            // Create main menu buttons
            playButton = new Button(game, spriteBatch, new Rectangle(103, 275, 275, 70), blueButton, menuFontBig, "Play");
            levelSelectButton = new Button(game, spriteBatch, new Rectangle(103, 375, 275, 70), blueButton, menuFontSmall, "Level Select");
            settingsButton = new Button(game, spriteBatch, new Rectangle(103, 475, 275, 70), yellowButton, menuFontBig, "Settings");
            exitButton = new Button(game, spriteBatch, new Rectangle(103, 575, 275, 70), redButton, menuFontBig, "Exit");
        }

        /// <summary>
        /// Unloads all components of the main menu.
        /// </summary>
        private void UnloadMainMenu()
        {
            playButton = null;
            levelSelectButton = null;
            settingsButton = null;
            exitButton = null;
        }

        /// <summary>
        /// Load all components needed for the level select view.
        /// </summary>
        private void LoadLevelSelect()
        {
            selector = new LevelSelector(game, spriteBatch);
        }

        /// <summary>
        /// Unload all components of the level select view.
        /// </summary>
        private void UnloadLevelSelect()
        {
            selector = null;
        }

        /// <summary>
        /// Loads all components needed for the settings view.
        /// </summary>
        private void LoadSettings()
        {
            // Load textures
            Texture2D yellowButton = game.Content.Load<Texture2D>("Buttons\\YellowButton");
            Texture2D blueButton = game.Content.Load<Texture2D>("Buttons\\BlueButton");
            Texture2D redButton = game.Content.Load<Texture2D>("Buttons\\RedButton");

            // Load fonts
            SpriteFont menuFontBig = game.Content.Load<SpriteFont>("Fonts\\Pericles36");
            SpriteFont menuFontSmall = game.Content.Load<SpriteFont>("Fonts\\Pericles28");

            // Create sound label
            sound = new Label(game, spriteBatch, new Rectangle(103, 275, 275, 70), menuFontSmall, "Play with sound?");

            // Create buttons
            soundYes = new Button(game, spriteBatch, new Rectangle(103, 375, 122, 70), yellowButton, menuFontBig, "Yes");
            soundNo = new Button(game, spriteBatch, new Rectangle(255, 375, 122, 70), blueButton, menuFontBig, "No");
            reset = new Button(game, spriteBatch, new Rectangle(103, 575, 275, 70), redButton, menuFontSmall, "Reset Levels");
        }

        /// <summary>
        /// Unloads all components needed for settings view.
        /// </summary>
        private void UnloadSettings()
        {
            soundYes = null;
            soundNo = null;
            reset = null;
        }

        /// <summary>
        /// Allows the menu to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update based on view
            switch (state)
            {
                case MenuState.Main:
                    UpdateMainMenu(gameTime);
                    break;

                case MenuState.LevelSelect:
                    UpdateLevelSelection(gameTime);
                    break;

                case MenuState.Settings:
                    UpdateSettings(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates all components in the main menu view.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateMainMenu(GameTime gameTime)
        {
            // Allows the game to exit on back button
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                game.Exit();

            // Update all the buttons
            playButton.Update(gameTime);
            levelSelectButton.Update(gameTime);
            settingsButton.Update(gameTime);
            exitButton.Update(gameTime);

            // Handle the play button
            if (playButton.State == TouchButtonState.Clicked)
            {
                UnloadMainMenu();
                LoadLevel = true;
                LevelNumber = FileHandler.TopLevel() + 1;
                // Don't go over max levels
                if (LevelNumber == 26)
                    LevelNumber = 25;
            }

            // Handle the level select button
            else if (levelSelectButton.State == TouchButtonState.Clicked)
            {
                state = MenuState.LevelSelect;
                UnloadMainMenu();
                LoadLevelSelect();
            }

            // Handle the settings button
            else if (settingsButton.State == TouchButtonState.Clicked)
            {
                state = MenuState.Settings;
                UnloadMainMenu();
                LoadSettings();
            }

            // Handle the exit button
            else if (exitButton.State == TouchButtonState.Clicked)
                game.Exit();
        }

        /// <summary>
        /// Updates all components in the level select view.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateLevelSelection(GameTime gameTime)
        {
            selector.Update(gameTime);

            // Handle a level load
            if (selector.LoadLevel)
            {
                LoadLevel = true;
                LevelNumber = selector.LevelNumber;
            }

            // Monitor the back button to go back to main menu
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                state = MenuState.Main;
                UnloadLevelSelect();
                LoadMainMenu();
            }
        }

        /// <summary>
        /// Updates all components in the settings view.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateSettings(GameTime gameTime)
        {
            // Update the settings buttons
            soundYes.Update(gameTime);
            soundNo.Update(gameTime);
            reset.Update(gameTime);

            // Handle the reset button to reset levels back to 1
            if (reset.State == TouchButtonState.Clicked)
                FileHandler.ResetLevels();

            // Handle the enable sound button
            if (soundYes.State == TouchButtonState.Clicked)
            {
                // Force a sound to play when sound gets turned on intially
                if (SoundMixer.Instance(game).Muted)
                    SoundMixer.Instance(game).PlayButton(true);
                SoundMixer.Instance(game).Muted = false;
            }

            // Handle sound off button
            if (soundNo.State == TouchButtonState.Clicked)
                SoundMixer.Instance(game).Muted = true;

            // Monitor the back button to go back to main menu
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                state = MenuState.Main;
                UnloadSettings();
                LoadMainMenu();
            }
        }

        /// <summary>
        /// Allows the menu to draw itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw the static title (Block3r)
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
