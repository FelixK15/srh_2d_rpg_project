using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using RpgGame.Manager;
using RpgGame.Tools;

namespace RpgGame.GameComponents
{
    class SpritesheetAnimation : AbstractAnimation
    {
        private int Frames { get; set; }

        private int FrameHeight { get; set; }
        private int FrameWidth { get; set; }

        private int Duration { get; set; }

        public SpritesheetAnimation(string name,string texturename,int frameWidth,int frameHeight,
                                    int duration, int frames, RepeatBehaviour repeatBehaviour)
            : this(name,RpgGame.ContentManager.Load<Texture2D>(texturename),frameWidth,frameHeight,duration,frames,repeatBehaviour)
        {

        }

        public SpritesheetAnimation(string animationName, Texture2D texture, int frameWidth, int frameHeight, 
                                    int duration, int frames, RepeatBehaviour repeatBehaviour)
            : base(animationName, repeatBehaviour)
        {
            FrameList = new List<AnimationFrame>();
            Name = animationName;
            CurrentAnimationIndex = 0;
            FrameHeight = frameHeight;
            FrameWidth = frameWidth;
            Frames = frames;
            Behaviour = repeatBehaviour;
            Duration = duration;
            Init(texture);
            NextFrame();
        }

        private void Init(Texture2D spriteSheet)
        {
            int X = 0;
            int Y = 0;

            for (int i = 0; i < Frames; ++i)
            {
                FrameList.Add(new AnimationFrame(Texture2DCropper.Crop(spriteSheet, new Rectangle(X,Y, FrameWidth, FrameHeight)),
                              Duration, 0, 0));

                if (X > spriteSheet.Width)
                {
                    X = 0;
                    Y += FrameHeight;
                }

                X += FrameWidth;
            }
        }                     
    }
}
