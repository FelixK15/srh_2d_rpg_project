using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.GameComponents
{
    public class BaseGameComponent : IGameObjectComponent
    {
        public Vector2 Position { get; set; }

        public GameObject Parent { get; set; }

        public string Name { get; set; }

        public Boolean Active { get; set; }

        public BaseGameComponent(string name)
        {
            Position = Vector2.Zero;
            Name = name;
            Active = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(ref SpriteBatch batch)
        {

        }

        public virtual void Init()
        {

        }

        public virtual BaseGameComponent Copy()
        {
            return null;
        }
    }
}
