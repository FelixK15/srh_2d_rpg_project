using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RpgGame.Events;
using RpgGame.Manager;
using Microsoft.Xna.Framework;
using RpgGame.Tools;

namespace RpgGame.GameComponents
{
    class EnemyComponent : BaseGameComponent, IEventListener
    {
        private static Random DamageRandom = new Random();

        public  int Health       { get; set; }
        public  int Experience   { get; set; }
        public  int AttackDamage { get; set; }
        public  int Defense      { get; set; }
        private int HitCoolDown  { get; set; }

        private const int HIT_COOLDOWN = 100;

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

        public EnemyComponent(int health,int experience,int damage,int defense) : this()
        {
            Health          = health;
            Experience      = experience;
            AttackDamage    = damage;
            Defense         = damage;
        }

        ~EnemyComponent()
        {
            EventManager.RemoveListener(Event.Types.ON_PLAYER_ATTACK,this);
        }

        public override void Update(GameTime gameTime)
        {
            if(HitCoolDown > 0){
                HitCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        public void HandleEvent(Event GameEvent)
        {
            if(GameEvent is PlayerAttackEvent){
                if(HitCoolDown > 0){
                    return;
                }

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
                            damage = (int)(((float)Defense / 100.0f) * damage);

                            //0 - 10% of the armor will get ignored by the attack
                            damage += (int)((DamageRandom.Next(0,10) / 100.0f) * attackEvent.AttackedWeapon.AttackDamagePerLevel[attackEvent.AttackLevel]);

                            DeveloperConsole.AddMessage(Tools.DeveloperConsole.MessageType.DEBUG,String.Format("Hit '{0}' with {1} damage",Parent.Name,damage));

                            //call function in enemy script (Parameter = Weapon that hit the enemy, damage of that attack)
                            ScriptComponent scriptComponent = Parent.GetComponent<ScriptComponent>();
                            if(scriptComponent != null){
                                scriptComponent.CallFunction("OnAttack",new object[]{attackEvent.AttackedWeapon,damage});
                            }

                            //decrement health based on damage and check if the enemy is dead.
                            Health -= damage;

                            HitCoolDown = HIT_COOLDOWN;

                            if(Health <= 0){
                                //Call onDeath function (Parameter = player that killed the enemy)
                                if(scriptComponent != null){
                                    scriptComponent.CallFunction("OnDeath",new object[]{attackEvent.AttackedWeapon.Parent});
                                }

                                ++attackEvent.AttackedWeapon.Kills;
                                
                                foreach(BaseGameComponent component in Parent.Components){
                                    component.Active = false;
                                }
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
