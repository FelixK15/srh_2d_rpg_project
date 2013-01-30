using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using RpgGame;
using RpgGame.GameComponents;

public class test
{
    public void Init()
    {
        Parent.AddComponent(new CollisionComponent(CollisionComponent.CollisionType.DYNAMIC,
                            Vector2.Zero, Parent.Width, Parent.Height,
                            new List<String>() { "Player" }));

        Parent.Name = "Test";
    }

    public void Update(int deltaTime)
    {
        //Console.Out.WriteLine("Test " + deltaTime);
    }

    public void OnCollision(GameObject gameObject)
    {
        //Console.Out.WriteLine("Collision with " + gameObject.Name);
    }

    public void OnTriggerEnter(GameObject gameObject)
    {
        Console.Out.WriteLine("OnTriggerEnter" + gameObject.Name);
    }

    public void OnTriggerLeave(GameObject gameObject)
    {
        Console.Out.WriteLine("OnTriggerLeave" + gameObject.Name);
    }

    public void OnInteraction(GameObject gameObject)
    {

    }
}