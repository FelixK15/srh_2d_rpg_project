using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.GameComponents
{
    class EnemyComponent : BaseGameComponent
    {
        public int Health       { get; set; }
        public int Experience   { get; set; }
        public int AttackDamage { get; set; }
        public int Defense      { get; set; }

        public bool IsDead {
            get 
            {
                return Health <= 0;
            }
        }

        public EnemyComponent() : base("EnemyComponent")
        {

        }
    }
}
