using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RpgGame.GameComponents;
using Microsoft.Xna.Framework;
using RpgGame.Manager;
using RpgGame.Events;

namespace RpgGame.Processes
{
    //This process raises events for a specific amount of time
    public class PlayerAttackProcess : Process
    {
        private Weapon      UsedWeapon      { get; set; }
        private Rectangle   AttackArea      { get; set; }
        private int         AttackLevel     { get; set; }
        private int         Duration        { get; set; }
        private int         SleepDuration   { get; set; }

        public PlayerAttackProcess(Weapon weapon,int attackLevel,Rectangle attackArea,int duration,int sleep)
        {
            UsedWeapon      = weapon;
            AttackArea      = attackArea;
            AttackLevel     = attackLevel;
            Duration        = duration;
            SleepDuration   = sleep;

            Started = true;
        }

        public override void Start()
        {

        }

        public override void End()
        {

        }

        public override void Update(GameTime gameTime)
        {
            int deltaMS = gameTime.ElapsedGameTime.Milliseconds;

            if(SleepDuration <= 0){
                if(Duration <= 0){
                    Finished = true;
                }else{
                    Duration -= deltaMS;

                    EventManager.AddEventToQuery(new PlayerAttackEvent(AttackArea, UsedWeapon, AttackLevel));
                }
            }else{
                SleepDuration -= deltaMS;
            }
        }
    }
}
