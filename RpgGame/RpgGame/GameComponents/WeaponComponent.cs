using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RpgGame.Manager;
using RpgGame.Events;

namespace RpgGame.GameComponents
{
    class WeaponComponent : BaseGameComponent
    {
        public  Weapon  CurrentWeapon       { get; set; }
        private int     LastAttackLevel     { get; set; }
        private int     AttackPattern       { get; set; }
        private int     AttackOrientation   { get; set; } //AttackOrientation will get set based on the orientation of the character

        private const int LEFT   = 0;
        private const int RIGHT  = 1;
        private const int TOP    = 2;
        private const int BOTTOM = 3;

        public WeaponComponent(Weapon weapon) : base("WeaponComponent")
        {
            CurrentWeapon = weapon;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void StartAttack(int level)
        {
            LastAttackLevel = level;

            CurrentWeapon.Parent = Parent;

            //Call function on the start of the attack (Parameter = player that started the attack and weapon level)
            if(CurrentWeapon != null){
                CurrentWeapon.Script.CallFunction("OnAttack",new object[]{Parent,level});
            }

            if(Parent.Orientation.X != 0){
                if(Parent.Orientation.X == 1){
                    AttackOrientation = RIGHT;
                }else{
                    AttackOrientation = LEFT;
                }
            }else{
                if(Parent.Orientation.Y == 1){
                    AttackOrientation = BOTTOM;
                }else{
                    AttackOrientation = TOP;
                }
            }
        }
    }
}
