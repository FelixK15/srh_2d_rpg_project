using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.Events;
using RpgGame.Manager;

namespace RpgGame.GameComponents
{
    class PlayerComponent : BaseGameComponent
    {
        private PlayerIndex Player { get; set; }

        public PlayerComponent(PlayerIndex player)
            : base("PlayerComponent")
        {
            Player = player;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(ref SpriteBatch batch)
        {
            
        }

        public override void Init()
        {
            
        }

        public override BaseGameComponent Copy()
        {
            return null;
        }
    }
}
