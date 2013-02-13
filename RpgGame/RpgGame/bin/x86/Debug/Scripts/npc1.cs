using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using RpgGame;
using RpgGame.GameComponents;

public class npc1
{
    private Random r = new Random();

    private int velocityCounter = 0;
    private int velocityCounter_max = 3000;

    public void Init()
    {
        Parent.AddComponent(new CollisionComponent(CollisionComponent.CollisionType.DYNAMIC,
                            Vector2.Zero, Parent.Width, Parent.Height,null));

        Parent.AddComponent(new CollisionComponent(CollisionComponent.CollisionType.STATIC,
                            new Vector2(0.0f,20.0f),16,16,null));
    }

    public void Update(int deltaTime)
    {
        if(velocityCounter <= 0)
        {
            _moveRandom();

            velocityCounter = velocityCounter_max;        
        }else{
            velocityCounter -= deltaTime;
        }
    }

    public void OnInteraction(GameObject gameObject)
    {
	   Console.Out.WriteLine("OnInteraction" + gameObject.Name);
    }

    private void _moveRandom()
    {
        float x = (float)r.Next(-1,1);
        float y = (float)r.Next(-1,1);

        Parent.Velocity = new Vector2(x,y);

        AnimationComponent animComponent = Parent.GetComponent<AnimationComponent>();
        
        if (Parent.Orientation.X == 1)
        {
            if (Parent.Velocity != Vector2.Zero)
            {
                animComponent.setCurrentAnimation("WalkRight");
            }
            else
            {
                animComponent.setCurrentAnimation("LookRight");
            }
        }
        else if (Parent.Orientation.X == -1)
        {
            if (Parent.Velocity != Vector2.Zero)
            {
                animComponent.setCurrentAnimation("WalkLeft");
            }
            else
            {
                animComponent.setCurrentAnimation("LookLeft");
            }
        }
        else if (Parent.Orientation.Y == 1)
        {
            if (Parent.Velocity != Vector2.Zero)
            {
                animComponent.setCurrentAnimation("WalkDown");
            }else{
                animComponent.setCurrentAnimation("LookDown");
            }
        }
        else if (Parent.Orientation.Y == -1)
        {
            if (Parent.Velocity != Vector2.Zero)
            {
                animComponent.setCurrentAnimation("WalkUp");
            }
            else
            {
                animComponent.setCurrentAnimation("LookUp");
            }
        }
    }
}