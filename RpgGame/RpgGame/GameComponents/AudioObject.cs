using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace RpgGame.GameComponents
{
    public class AudioObject
    {
        public enum RepeatBehaviour
        {
            SinglePlay,
            LoopPlay
        }
        public string Name { get; set; }
        protected RepeatBehaviour Behaviour { get; set; }
        protected SoundEffectInstance Instance { get; set; }

        public AudioObject(String name, String audioPath, RepeatBehaviour behaviour)
        {
            Instance = RpgGame.ContentManager.Load<SoundEffect>(audioPath).CreateInstance();
            Name = name;
            Behaviour = behaviour;
            if (behaviour == RepeatBehaviour.LoopPlay) Instance.IsLooped = true;
        }

        public void Play()
        {
            Instance.Play();
        }

        public void Pause()
        {
            Instance.Pause();
        }

        public void Stop()
        {
            Instance.Stop();
        }
    }
}
