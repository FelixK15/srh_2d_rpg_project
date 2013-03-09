using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RpgGame.Tools
{
    class Texture2DCropper
    {
        public static Texture2D Crop(Texture2D source, Rectangle rect)
        {
            GraphicsDevice tempDevice = source.GraphicsDevice;
            tempDevice.BlendState = BlendState.AlphaBlend;
            RenderTarget2D tempRenderTarget = new RenderTarget2D(tempDevice, rect.Width, rect.Height,false,SurfaceFormat.Color,DepthFormat.None);

            SpriteBatch tempSpriteBatch = new SpriteBatch(tempDevice);

            tempDevice.SetRenderTarget(tempRenderTarget);
            tempRenderTarget.GraphicsDevice.Clear(Color.Transparent);

            tempSpriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullCounterClockwise);
            tempSpriteBatch.Draw(source, Vector2.Zero, rect, Color.White);
            tempSpriteBatch.End();

            tempDevice.SetRenderTarget(null);

            return (Texture2D)tempRenderTarget;
        }
    }
}
