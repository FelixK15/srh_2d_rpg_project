using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RpgGame.Events
{
    class PlayerMoveEvent : Event
    {
        public PlayerIndex Player { get; private set; }
        public Vector2 Position { get; private set; }

        public PlayerMoveEvent(PlayerIndex index,Vector2 position)
            : base(Types.PLAYER_MOVED)
        {
            Player = index;
            Position = position;
        }
    }
}
