using System;
using System.Collections.Generic;
using System.Text;

using RpgGame.GameComponents;
using RpgGame;
using RpgGame.Tools;


public class TestSword
{
    public void OnAttack(GameObject parent, int attackLevel)
    {
        AnimationComponent animComponent = parent.GetComponent<AnimationComponent>();
        if (animComponent != null)
        {
            if (parent.Orientation.Y == 1)
            {
                animComponent.SetCurrentAnimation("AttackDown_Sword");
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.TRACE,attackLevel.ToString());
            }
        }
    }
}

