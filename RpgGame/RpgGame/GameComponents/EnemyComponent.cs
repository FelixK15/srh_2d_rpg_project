using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RpgGame.Events;
using RpgGame.Manager;
using Microsoft.Xna.Framework;

namespace RpgGame.GameComponents
{
    class EnemyComponent : BaseGameComponent, IEventListener
    {
        private static Random DamageRandom = new Random();

        public int Health       { get; set; }
        public int Experience   { get; set; }
        public int AttackDamage { get; set; }
        public int Defense      { get; set; }

        public bool IsDead {
            get 
            {
                return Health <= 0;
            }

            set
            {
                if(value == true){
                    Health = 0;
                }
            }
        }

        public EnemyComponent() : base("EnemyComponent")
        {
            EventManager.AddListener(Event.Types.ON_PLAYER_ATTACK,this);
        }

        ~EnemyComponent()
        {
            EventManager.RemoveListener(Event.Types.ON_PLAYER_ATTACK,this);
        }

        public void HandleEvent(Event GameEvent)
        {
            if(GameEvent is PlayerAttackEvent){
                PlayerAttackEvent attackEvent = (PlayerAttackEvent)GameEvent;

                //get the collision component of the enemy and check for intersections with the attack area
                CollisionComponent collComponent = Parent.GetComponent<CollisionComponent>();
                if(collComponent != null && collComponent.Active){
                    Rectangle collArea = new Rectangle((int)(Parent.Position.X + collComponent.Offset.X),(int)(Parent.Position.Y + collComponent.Offset.Y),
                                                       collComponent.Width,collComponent.Height);

                    if(attackEvent.AttackArea.X < collArea.X + collArea.Width && attackEvent.AttackArea.X + attackEvent.AttackArea.Width > collArea.X){
                        if(attackEvent.AttackArea.Y < collArea.Y + collArea.Height && attackEvent.AttackArea.Y + attackEvent.AttackArea.Height > collArea.Y){
                            //the enemy has been hit by the weapon
                            //call function in weapon script (Parameter = GameObject that holds the weapon that hit the enemy, attack level of the attack)
                            attackEvent.AttackedWeapon.Script.CallFunction("OnHit",new object[]{Parent,attackEvent.AttackLevel});

                            int damage = attackEvent.AttackedWeapon.AttackDamagePerLevel[attackEvent.AttackLevel];

                            if(Defense > 100){
                                Defense = 100;
                            }

                            //calculate the damage based on the armor of the enemy
                            damage = (Defense / 100) * damage;

                            //0 - 10% of the armor will get ignored by the attack
                            damage += (int)((DamageRandom.NextDouble() * 10) * attackEvent.AttackedWeapon.AttackDamagePerLevel[attackEvent.AttackLevel]);

                            //call function in enemy script (Parameter = Weapon that hit the enemy, damage of that attack)
                            ScriptComponent scriptComponent = Parent.GetComponent<ScriptComponent>();
                            if(scriptComponent != null){
                                scriptComponent.CallFunction("OnAttack",new object[]{attackEvent.AttackedWeapon,damage});
                            }

                            //increment health based on damage and check if the enemy is dead.
                            Health -= damage;

                            if(Health <= 0){
                                //Call onDeath function (Parameter = player that killed the enemy
                                if(scriptComponent != null){
                                    scriptComponent.CallFunction("OnDeath",new object[]{attackEvent.AttackedWeapon.Parent});
                                }

                                ++attackEvent.AttackedWeapon.Kills;
                            }

                            //add experience to the players experience
                            PlayerComponent playerComponent = attackEvent.AttackedWeapon.Parent.GetComponent<PlayerComponent>();
                            if(playerComponent != null){
                                playerComponent.Experience += Experience;
                            }
                        }
                    }
                }
            }
        }
    }
}
