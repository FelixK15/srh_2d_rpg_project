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
        private Weapon  _CurrentWeapon;
        public  Weapon  CurrentWeapon       
        { 
            get
            {
                return _CurrentWeapon;
            }
            set
            {
                value.Parent = Parent;
                _CurrentWeapon = value;
            }
        }
        private int     LastAttackLevel     { get; set; }
        private int     AttackPattern       { get; set; }
        private int     AttackOrientation   { get; set; } //AttackOrientation will get set based on the orientation of the character

        public WeaponComponent(Weapon weapon) : base("WeaponComponent")
        {
            _CurrentWeapon = weapon;
        }

        public override void Init()
        {
            _CurrentWeapon.Parent = Parent;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void StartAttack(int level)
        {
            LastAttackLevel = level;

            //Call function on the start of the attack (Parameter = player that started the attack and weapon level)
            if(CurrentWeapon != null){
                CurrentWeapon.Script.CallFunction("OnAttack",new object[]{Parent,level});
            }

            if(Parent.Orientation.X != 0){
                if(Parent.Orientation.X == 1){
                    AttackOrientation = PlayerComponent.RIGHT;
                }else{
                    AttackOrientation = PlayerComponent.LEFT;
                }
            }else{
                if(Parent.Orientation.Y == 1){
                    AttackOrientation = PlayerComponent.DOWN;
                }else{
                    AttackOrientation = PlayerComponent.UP;
                }
            }
        }
    }
}
