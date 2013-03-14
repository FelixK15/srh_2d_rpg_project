using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.Tools
{
    class Texture2DFiller
    {
        public static void Fill(ref Texture2D Texture,Color Pixel)
        {
            Color[] Pixels = new Color[Texture.Width * Texture.Height];

            for(int i = 0;i < Pixels.Length;++i){
                Pixels[i] = Pixel;
            }

            Texture.SetData<Color>(Pixels);
        }
    }
}
