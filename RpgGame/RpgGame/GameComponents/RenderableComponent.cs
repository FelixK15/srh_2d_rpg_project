using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.Manager;
using RpgGame.Tools;


namespace RpgGame.GameComponents
{
    class RenderableComponent : BaseGameComponent
    {
        public Texture2D Texture
        {
            get;
            set;
        }

        public RenderableComponent(string texturePath) : base("RenderableComponent")
        {
            Texture = RpgGame.ContentManager.Load<Texture2D>(texturePath);
        }

        public RenderableComponent(Texture2D texture) : base("RenderableComponent")
        {
            Texture = texture;
        }

        public override void Draw(ref SpriteBatch batch)
        {
            batch.Draw(Texture, Position, Color.White);
            //GraphicManager.Graphics.Add(new GraphicHelper((int)Position.X, (int)Position.Y, Texture));
        }

        public override BaseGameComponent Copy()
        {
            Texture2D copyTexture = null;
            if(Texture != null){
                copyTexture = Texture2DCropper.Crop(Texture,new Rectangle(0,0,Texture.Width,Texture.Height));
            }
            RenderableComponent copyComponent = new RenderableComponent(copyTexture);
            copyComponent.Active = Active;
            return copyComponent;
        }
    }
}
