using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using RpgGame;
using RpgGame.GameComponents;

public class Default
{
    public void Init()
    {
        //Parent will get add automaticly as a class member when used via script component
        //..
    }

    public void Update(int deltaTime)
    {
        //Deltatime in milliseconds
        //...
    }

    public void OnCollision(GameObject gameObject)
    {
        //gameobject = object that has been collided with
        //..
    }

    public void OnTriggerEnter(GameObject gameObject)
    {
        //gameobject =  object that has been collided with
        //..
    }

    public void OnTriggerLeave(GameObject gameObject)
    {
        //gameobject =  object that has been collided with
        //..
    }

    public void OnInteraction(GameObject gameObject)
    {
        //gameobject = player
        //..
    }
}