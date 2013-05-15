using System;
using System.Collections.Generic;
using System.Text;

using RpgGame.GameComponents;
using RpgGame;
using RpgGame.Tools;
using RpgGame.Manager;
using RpgGame.Processes;

using Microsoft.Xna.Framework;

public class TestSword
{
    public void OnAttack(GameObject parent, int attackLevel)
    {
        AnimationComponent animComponent = parent.GetComponent<AnimationComponent>();
        WeaponComponent    weapComponent = parent.GetComponent<WeaponComponent>();
        if (animComponent != null)
        {
            if (parent.Orientation.Y == 1)
            {
                animComponent.SetCurrentAnimation("AttackDown_Sword");
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.TRACE,attackLevel.ToString());

                Rectangle attackArea = new Rectangle((int)parent.Position.X,(int)parent.Position.Y + parent.Height + 10,10,10);
                ProcessManager.Processes.Add(new PlayerAttackProcess(weapComponent.CurrentWeapon,attackLevel,attackArea,50,50));
            }
        }
    }
}

