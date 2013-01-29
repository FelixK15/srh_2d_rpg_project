using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.GameComponents
{
    public interface IGameObjectComponent
    {
        string Name { get; set; }
        GameObject Parent { get; set; }
        Vector2 Position { get; set; }

        Boolean Active { get; set; }

        void Update(GameTime gameTime);
        void Draw(ref SpriteBatch batch);
    
        void Init();
    }
}
