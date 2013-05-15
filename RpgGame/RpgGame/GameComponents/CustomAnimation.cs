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
        public CustomAnimation(string name,string[] textureNames,int[] frameDuration, RepeatBehaviour repeatBehaviour)
            : base(name,repeatBehaviour)
        {
            for (int i = 0; i < textureNames.Length; ++i)
            {
                AnimationFrame NewFrame = new AnimationFrame();
                NewFrame.Sprite = RpgGame.ContentManager.Load<Texture2D>(textureNames[i]);
                if (i > frameDuration.Length){
                    NewFrame.Duration = frameDuration.Last();
                }
                else{
                    NewFrame.Duration = frameDuration[i];
                }

                FrameList.Add(NewFrame);
            }
        }
    }
}
