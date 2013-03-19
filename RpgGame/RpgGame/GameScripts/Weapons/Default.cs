using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.GameScripts.Weapons
{
    class Default
    {
        public void OnAttack(GameObject parent,int attackLevel)
        {
            //Parent = player who hold the weapon when it hit an enemy
            //attacklevel = level of the attack (0 - 4)
        }

        public void OnHit(GameObject parent,int attackLevel)
        {
            //Parent = player who hold the weapon when it hit an enemy
            //attacklevel = level of the attack (0 - 4)
        }
    }
}
