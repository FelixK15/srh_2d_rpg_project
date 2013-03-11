using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using RpgGame.Manager;

namespace RpgGame.Tools
{
    class Texture2DCropper
    {
        public static Texture2D Crop(Texture2D source, Rectangle rect)
        {
            RenderTarget2D tempRenderTarget = new RenderTarget2D(GraphicSettings.GraphicDevice, rect.Width, rect.Height);

            SpriteBatch tempSpriteBatch = new SpriteBatch(GraphicSettings.GraphicDevice);

            GraphicSettings.GraphicDevice.SetRenderTarget(tempRenderTarget);
            tempRenderTarget.GraphicsDevice.Clear(Color.Transparent);

            tempSpriteBatch.Begin();
            tempSpriteBatch.Draw(source, Vector2.Zero, rect, Color.White);
            tempSpriteBatch.End();

            GraphicSettings.GraphicDevice.SetRenderTarget(null);

            return (Texture2D)tempRenderTarget;
        }
    }
}
