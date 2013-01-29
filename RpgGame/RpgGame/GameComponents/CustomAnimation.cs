using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.GameComponents
{
    class CustomAnimation : AbstractAnimation
    {
        public CustomAnimation(string name,string[] textureNames,double[] frameDuration, RepeatBehaviour repeatBehaviour)
            : base(name,repeatBehaviour)
        {
            for (int i = 0; i < textureNames.Length; ++i)
            {
                AnimationFrame NewFrame = new AnimationFrame();
                NewFrame.texture = RpgGame.ContentManager.Load<Texture2D>(textureNames[i]);
                NewFrame.x = 0;
                NewFrame.y = 0;
                if (i > frameDuration.Length)
                {
                    NewFrame.duration = frameDuration.Last();
                }
                else
                {
                    NewFrame.duration = frameDuration[i];
                }

                FrameList.Add(NewFrame);
            }
        }
    }
}
