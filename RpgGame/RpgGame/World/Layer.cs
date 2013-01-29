using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.Manager;
using RpgGame.GameComponents;
using RpgGame.Tools;

namespace RpgGame.World
{
    class Layer
    {
        public string Name { get; set; }
        public float FloatSpeed { get; set; }
        public List<GameObject> Tiles { get; private set; }
        public List<GameObject> Objects { get; private set; }
        public GameWorld World { get; private set; }

        public Layer(GameWorld world)
        {
            Name = "";
            FloatSpeed = 1.0f;
            Tiles = new List<GameObject>();
            Objects = new List<GameObject>();
            World = world;
        }

        public void Update(GameTime time)
        {
            foreach (GameObject go in Tiles)
            {
                go.Update(time);
            }

            //Collision
            foreach (GameObject go in Objects)
            {
                CollisionComponent Component = go.GetComponent<CollisionComponent>();
                float x = go.Position_NextFrame.X;
                float y = go.Position_NextFrame.Y;
                if (Component != null)
                {
                    foreach (GameObject t in Tiles)
                    {
                        if (go.Position_NextFrame.X + Component.Offset.X + Component.Width > t.Position.X &&
                            go.Position_NextFrame.X + Component.Offset.X < t.Position.X + t.Width &&
                            go.Position.Y + Component.Offset.Y + Component.Height > t.Position.Y &&
                            go.Position.Y + Component.Offset.Y < t.Position.Y + t.Height ||
                            go.Position_NextFrame.X + Component.Offset.X < 0 ||
                            go.Position_NextFrame.X + Component.Offset.X + Component.Width > World.PixelHeight)
                        {
                            x = go.Position.X;
                        }

                        if (go.Position.X + Component.Offset.X + Component.Width > t.Position.X &&
                            go.Position.X + Component.Offset.X < t.Position.X + t.Width &&
                            go.Position_NextFrame.Y + Component.Offset.Y + Component.Height > t.Position.Y &&
                            go.Position_NextFrame.Y + Component.Offset.Y < t.Position.Y + t.Height ||
                            go.Position_NextFrame.Y + Component.Offset.Y < 0 ||
                            go.Position_NextFrame.Y + Component.Offset.Y + Component.Height > World.PixelHeight)
                        {
                            y = go.Position.Y;
                        }
                    }
                }

                go.Position = new Vector2(x, y);

                foreach (GameObject go2 in Objects)
                {
                    if ((go != null && go2 != null) &&
                        (go != go2))
                    {
                        CollisionComponent c1 = go.GetComponent<CollisionComponent>();
                        CollisionComponent c2 = go2.GetComponent<CollisionComponent>();

                        if ((c1 != null && c2 != null) && (c1.Active && c2.Active) &&
                            ((c1.Type == CollisionComponent.CollisionType.STATIC && c2.Type == CollisionComponent.CollisionType.DYNAMIC) ||
                            (c1.Type == CollisionComponent.CollisionType.DYNAMIC && c2.Type == CollisionComponent.CollisionType.STATIC) ||
                            (c1.Type == CollisionComponent.CollisionType.DYNAMIC && c2.Type == CollisionComponent.CollisionType.DYNAMIC)) &&
                            (c1.CollisionTypes.Find(s => s == go2.Type) != null || c2.CollisionTypes.Find(s => s == go.Type) != null))
                        {
                            if (CollisionChecker.Intersect(new Rectangle((int)(c1.Position.X + c1.Offset.X),(int)(c1.Position.Y + c1.Offset.Y),
                                                            c1.Width,c1.Height), new Rectangle((int)(c2.Position.X + c2.Offset.X),
                                                            (int)(c2.Position.Y + c2.Offset.Y), c2.Width,c2.Height)))
                            {
                                if (c1.CollisionTypes.Find(s => s == go2.Type) != null)
                                {
                                    c1.AddCollisionObject(go2);
                                }
                                else
                                {
                                    c2.AddCollisionObject(go);
                                }
                            }
                        }
                    }
                }

                go.Update(time);
            }
        }

        public void Draw(ref SpriteBatch batch)
        {
            foreach (GameObject go in Tiles)
            {
                go.Draw(ref batch);
            }

            foreach (GameObject go in Objects)
            {
                go.Draw(ref batch);
            }
        }
    }
}
