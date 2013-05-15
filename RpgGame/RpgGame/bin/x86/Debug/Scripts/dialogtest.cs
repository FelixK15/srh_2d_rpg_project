using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using RpgGame;
using RpgGame.GameComponents;
using RpgGame.Tools;
using RpgGame.Dialogs;
using RpgGame.GameStates;
using RpgGame.Manager;


public class dialogtest
{
    //private Random r = new Random();

    public void Init()
    {
        Parent.AddComponent(new InteractionComponent());
    }

    public void Update(int deltaTime)
    {
        
    }

    public void OnInteraction(GameObject gameObject)
    {
	   DeveloperConsole.AddMessage(DeveloperConsole.MessageType.NORMAL, "OnInteraction");
	   DialogBox.start(1);
    }

}