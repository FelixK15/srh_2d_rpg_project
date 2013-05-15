using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.GameScripts.Enemies
{
    class Default
    {
        public void OnAttack(Weapon weapon,int damage)
        {
            //Attribute parent will be set automatically when used with script component
            //weapon = weapon that hit the enemy
            //damage = damage that the weapon did to that enemy (already calculated by the enemy component
            //..
        }

        public void OnDeath(GameObject player)
        {
            //player = player that killed the enemy
            //..
        }
    }
}
