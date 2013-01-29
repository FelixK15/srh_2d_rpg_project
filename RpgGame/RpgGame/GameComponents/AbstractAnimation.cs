using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RpgGame.GameComponents
{
    public abstract class AbstractAnimation
    {
        public enum RepeatBehaviour 
        { 
            SingleAnimation, 
            LoopAnimation 
        }
        
        public string Name { get; set; }
        public List<AnimationFrame> FrameList { get; protected set; }

        public AnimationFrame CurrentFrame { get; set; }
        protected int CurrentAnimationIndex { get; set; }
        
        protected double TimeSinceLastFrame { get; set; }
        protected RepeatBehaviour Behaviour { get; set; }

        protected AbstractAnimation(String name, RepeatBehaviour behaviour)
        {
            Name = name;
            Behaviour = behaviour;

            FrameList = new List<AnimationFrame>();
        }

        protected void NextFrame()
        {
            if ((CurrentAnimationIndex + 1) < FrameList.Count)
            {
                CurrentAnimationIndex++;
            }
            else
            {
                if (Behaviour == RepeatBehaviour.LoopAnimation)
                {
                    CurrentAnimationIndex = 0;
                }
            }
            CurrentFrame = FrameList.ElementAt(CurrentAnimationIndex);
        }

        public void Update(GameTime gameTime)
        {
            TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
          
            if (TimeSinceLastFrame > FrameList.ElementAt(CurrentAnimationIndex).duration)
            {
                TimeSinceLastFrame = 0;
                NextFrame();
            }
        }
    }
}
