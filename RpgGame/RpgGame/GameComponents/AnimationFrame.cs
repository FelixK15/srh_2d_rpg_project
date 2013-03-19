using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RpgGame.GameComponents
{
    public class AnimationFrame
    {
        public Texture2D Sprite     { get; set; }
        public int       Duration   { get; set; }
        public Vector2   Offset     { get; set; }

        public AnimationFrame()
        {
            Sprite      = null;
            Duration    = 0;
            Offset      = Vector2.Zero;
        }

        public AnimationFrame(Texture2D sprite, int duration, int x, int y)
        {
            Sprite      = sprite;
            Duration    = duration;
            Offset      = new Vector2(x,y);
        }

        public AnimationFrame(Texture2D sprite, int duration, Vector2 offset)
        {
            Sprite      = sprite;
            Duration    = duration;
            Offset      = offset;
        }
    }
}
