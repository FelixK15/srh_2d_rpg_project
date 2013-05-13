using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RpgGame.Tools;
using RpgGame.Manager;
using Microsoft.Xna.Framework;

namespace RpgGame.GameComponents
{
    public class Weapon
    {
        private GameObject  _Parent;

        public GameObject   Parent                  
        { 
            get
            {
                return _Parent;
            }
            set
            {
                PlayerComponent playerComponent = value.GetComponent<PlayerComponent>();
                playerComponent.IdleAnimations = IdleAnimations;
                playerComponent.WalkAnimations = WalkAnimations;
                _Parent = value;
            }
        }
        public int          WeaponLevel             { get; set; } 
        public int          Kills                   { get; set; }  //Amount of kills that has been done with this weapon
        public string       Name                    { get; set; }
        public int[]        AttackDamagePerLevel    { get; set; }

        public int[]        AttackSleep             { get; private set; }   //A timespan in ms that determines how long to wait from the start of the attack to the
                                                                            //intersection test with enemies (for each attack level)
        public int[]        AttackDuration          { get; private set; }   //A timespan in ms that determines how long to check for intersection in the attack area. (for each attack level)
       
        public CompiledScript  Script               { get; set; } //A script that will get called if the weapon hit an enemy or if the attack started / ended.
        public String[]        WalkAnimations       { get; private set; }   //an array of string holding the animations for walking (4 max - one animation for each direction)
        public String[]        IdleAnimations       { get; private set; }   //an array of string holding the animations for walking (4 max - one animation for each direction)
        
        private const int MAX_LEVELS = 4;

        public Weapon(string scriptFile)
        {
            Script = ScriptManager.CompileScript(scriptFile);

            WeaponLevel             = 1;
            AttackDamagePerLevel    = new int[MAX_LEVELS];
            AttackSleep             = new int[MAX_LEVELS];
            AttackDuration          = new int[MAX_LEVELS];
            WalkAnimations          = new String[PlayerComponent.MAX_DIRECTIONS];
            IdleAnimations          = new String[PlayerComponent.MAX_DIRECTIONS];
        }
    }
}
