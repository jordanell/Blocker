using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Blocker
{
    class SoundMixer
    {
        private Game game;
        private static SoundMixer instance;

        private Dictionary<string, SoundEffect> effects;

        private bool muted;
        public bool Muted
        {
            get { return muted; }
            set 
            {
                muted = value;
                SaveSettings(value);
            }
        }

        public static SoundMixer Instance(Game game)
        {
            if (instance == null)
                instance = new SoundMixer(game);
            return instance;
        }

        private SoundMixer(Game game)
        {
            int soundSetting = SoundEnabled();
            if (soundSetting == 0)
                muted = false;
            if (soundSetting == 1)
                muted = true;

            this.game = game;

            effects = new Dictionary<string, SoundEffect>();
        }

        public void PlayEffect(string file)
        {
            if (muted)
                return;

            // Load sound
            SoundEffect effect;
            effects.TryGetValue(file, out effect);
            if (effect == null)
            {
                effects.Add(file, game.Content.Load<SoundEffect>(file));
                effect = effects[file];
            }

            SoundEffectInstance effectInstance = effect.CreateInstance();
            effectInstance.Volume = 1.0f;
            effectInstance.Play();
        }

        public void PlayEffectForce(string file)
        {
            // Load sound
            SoundEffect effect;
            effects.TryGetValue(file, out effect);
            if (effect == null)
            {
                effects.Add(file, game.Content.Load<SoundEffect>(file));
                effect = effects[file];
            }

            SoundEffectInstance effectInstance = effect.CreateInstance();
            effectInstance.Volume = 1.0f;
            effectInstance.Play();
        }

        private int SoundEnabled()
        {
            using (IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (gameStorage.FileExists("settings"))
                {
                    using (IsolatedStorageFileStream fs = gameStorage.OpenFile("settings", System.IO.FileMode.Open))
                    {
                        if (fs != null)
                        {
                            byte[] bytes = new byte[10];
                            int x = fs.Read(bytes, 0, 10);
                            return System.BitConverter.ToInt32(bytes, 0);
                        }
                    }
                }
            }
            return -1;
        }

        private void SaveSettings(bool muted)
        {
            IsolatedStorageFile gameStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs = null;

            using (fs = gameStorage.CreateFile("settings"))
            {
                if (fs != null)
                {
                    byte[] bytes;
                    if (muted)
                        bytes = System.BitConverter.GetBytes(1);
                    else
                        bytes = System.BitConverter.GetBytes(0);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }


    }
}
