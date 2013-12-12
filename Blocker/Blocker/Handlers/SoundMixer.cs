using Blocker.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Blocker
{
    /// <summary>
    /// SoundMixer is a singleton class which is used to play any type of
    /// audio in Block3r.
    /// </summary>
    class SoundMixer
    {
        // XNA componenets
        private Game game;

        // SoundMixer is a singleton
        private static SoundMixer instance;
        public static SoundMixer Instance(Game game)
        {
            if (instance == null)
                instance = new SoundMixer(game);
            return instance;
        }

        // Current audio loaded
        private Dictionary<string, SoundEffect> effects;

        // Muted flag
        private bool muted;
        public bool Muted
        {
            get { return muted; }
            set 
            {
                muted = value;
                FileHandler.SaveSettings(value);
            }
        }

        /// <summary>
        /// Since SoundMixer is a singleton, this constructor is private. This constructor
        /// checks for audio settings for muted options.
        /// </summary>
        /// <param name="game"></param>
        private SoundMixer(Game game)
        {
            // Check for muted
            int soundSetting = FileHandler.SoundEnabled();
            if (soundSetting == 0)
                muted = false;
            if (soundSetting == 1)
                muted = true;

            this.game = game;

            // Initialize the audio hash
            effects = new Dictionary<string, SoundEffect>();
        }

        /// <summary>
        /// Play an audio sound based on the given file name.
        /// </summary>
        /// <param name="file">The given audio file to play.</param>
        public void PlayEffect(string file)
        {
            // Don't do anything when muted
            if (muted)
                return;

            // Load sound into audio hash if not already present
            SoundEffect effect;
            effects.TryGetValue(file, out effect);
            if (effect == null)
            {
                effects.Add(file, game.Content.Load<SoundEffect>(file));
                effect = effects[file];
            }

            // Play sound
            SoundEffectInstance effectInstance = effect.CreateInstance();
            effectInstance.Volume = 1.0f;
            effectInstance.Play();
        }

        /// <summary>
        /// Play an audio sound regardless of mute setting.
        /// </summary>
        /// <param name="file">The given audio file to play.</param>
        public void PlayEffectForce(string file)
        {
            // Load sound into audio hash if not already present
            SoundEffect effect;
            effects.TryGetValue(file, out effect);
            if (effect == null)
            {
                effects.Add(file, game.Content.Load<SoundEffect>(file));
                effect = effects[file];
            }

            // Play sound
            SoundEffectInstance effectInstance = effect.CreateInstance();
            effectInstance.Volume = 1.0f;
            effectInstance.Play();
        }
    }
}
