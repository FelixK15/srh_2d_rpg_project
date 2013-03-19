using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RpgGame.GameComponents
{
    class WeaponComponent : BaseGameComponent
    {
        public int         AttackDamage { get; set; }
        public Rectangle[] AttackAreas  { get; set; }
        
        public WeaponComponent() : base("WeaponComponent")
        {
            AttackDamage = 0;
            AttackAreas  = null;
        }
    }
}
