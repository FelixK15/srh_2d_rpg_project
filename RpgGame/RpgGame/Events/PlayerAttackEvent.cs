using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RpgGame.GameComponents;

namespace RpgGame.Events
{
    class PlayerAttackEvent : Event
    {
        public Rectangle AttackArea     { get; private set; }
        public Weapon    AttackedWeapon { get; private set; }
        public int       AttackLevel    { get; private set; }

        public PlayerAttackEvent(Rectangle area,Weapon weapon,int attackLevel) : base(Event.Types.ON_PLAYER_ATTACK)
        {
            AttackArea      = area;
            AttackedWeapon  = weapon;
            AttackLevel     = attackLevel;
        }
    }
}
