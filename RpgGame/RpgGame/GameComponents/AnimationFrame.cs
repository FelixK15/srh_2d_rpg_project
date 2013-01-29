using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.GameComponents
{
    public class AnimationFrame
    {
        public Texture2D texture;
        public double duration;
        public int x;
        public int y;

        public AnimationFrame()
        {
            texture = null;
            duration = 0;
            x = y = 0;
        }

        public AnimationFrame(Texture2D texture, double duration, int x, int y)
        {
            this.texture = texture;
            this.duration = duration;
            this.x = x;
            this.y = y;
        }
    }
}
