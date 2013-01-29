using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RpgGame.Manager;

namespace RpgGame.GameComponents
{
    class AudioComponent : BaseGameComponent
    {
        private List<AudioObject> AudioList { get; set; }
        private AudioObject CurrentAudio { get; set; }
        public AudioComponent() : base("AudioComponent")
        {
            AudioList = new List<AudioObject>();
        }

        public AudioComponent(AudioObject audio) : this()
        {
            AudioList.Add(audio);
            CurrentAudio = audio;
        }

        public AudioObject getAudioPerName(string audioName)
        {
            return AudioList.Find(o => o.Name == audioName);
        }

        public bool addAudio(AudioObject audio)
        {
            if (AudioList.Find(o => o.Name == audio.Name) != null) return false;
            AudioList.Add(audio);
            return true;
        }

        public void Play()
        {
            CurrentAudio.Play();
        }

        public void Stop()
        {
            CurrentAudio.Stop();
        }

        public void Pause()
        {
            CurrentAudio.Pause();
        }

        public bool setCurrentAudio(string audioName)
        {
            AudioObject audio = AudioList.Find(o => o.Name == audioName);
            if (audio != null)
            {
                CurrentAudio = audio;
                return true;
            }
            return false;
        }

        public AudioObject getCurrentAudio()
        {
            return CurrentAudio;
        }

        public override void Update(GameTime gameTime)
        {

        }

    }
}
