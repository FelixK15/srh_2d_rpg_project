using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RpgGame.Manager;
using RpgGame.GameComponents;
using RpgGame.Tools;
using RpgGame.Events;

namespace RpgGame.World
{
    class Layer : IEventListener
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
            Objects = new List<GameObject>();
            Tiles = new List<GameObject>();
            World = world;

            EventManager.AddListener(Event.Types.INTERACTION, this);
        }

        ~Layer()
        {
            EventManager.RemoveListener(Event.Types.INTERACTION, this);
        }

        public void Update(GameTime time)
        {
            List<GameObject> alreadyChecked = new List<GameObject>();

            foreach (GameObject tile in Tiles)
            {
                tile.Update(time);
            }

            foreach (GameObject go1 in Objects)
            {
                _CheckTileCollision(go1);
                foreach (GameObject go2 in Objects)
                {
                    if (go1 != go2 && !alreadyChecked.Contains(go2))
                    {
                        _CheckForCollision(go1, go2);
                    }
                }
                go1.Update(time);
                alreadyChecked.Add(go1);
            }
        }

        public void Draw(ref SpriteBatch batch)
        {
            foreach (GameObject tile in Tiles)
            {
                tile.Draw(ref batch);
            }

            Objects.Sort((go, go2) => (int)(go.Position.Y - go2.Position.Y));

            foreach (GameObject go in Objects)
            {
                go.Draw(ref batch);
            }
        }

        public void HandleEvent(Event eGameEvent)
        {
            if (eGameEvent is InteractionEvent)
            {
                _HandleInteraction((InteractionEvent)eGameEvent);
            }
        }

        private void _CheckTileCollision(GameObject go)
        {
            float x = go.Velocity.X;
            float y = go.Velocity.Y;
            CollisionComponent collision = go.GetComponent<CollisionComponent>();
            if(collision != null)
            {
                Rectangle goRect_NextFrame_X = new Rectangle((int)(go.Position_NextFrame.X + collision.Offset.X),
                                                            (int)(go.Position.Y + collision.Offset.Y),
                                                            collision.Width,collision.Height);

                Rectangle goRect_NextFrame_Y = new Rectangle((int)(go.Position.X + collision.Offset.X),
                                                            (int)(go.Position_NextFrame.Y + collision.Offset.Y),
                                                            collision.Width,collision.Height);
                foreach (GameObject tile in Tiles)
                {
                    Rectangle tileRect = new Rectangle((int)tile.Position.X, (int)tile.Position.Y, tile.Width, tile.Height);

                    if (tileRect.Intersects(goRect_NextFrame_X))
                    {
                        x = 0;
                    }

                    if (tileRect.Intersects(goRect_NextFrame_Y))
                    {
                        y = 0;
                    }
                }
            }

            go.Velocity = new Vector2(x, y);
        }

        private void _CheckForCollision(GameObject go1, GameObject go2)
        {
            List<CollisionComponent> components1 = go1.GetComponents<CollisionComponent>();
            List<CollisionComponent> components2 = go2.GetComponents<CollisionComponent>();
            List<CollisionComponent> alreadyChecked = new List<CollisionComponent>();

            foreach (CollisionComponent c1 in components1)
            {
                foreach (CollisionComponent c2 in components2)
                {
                    if (c1 != c2 && c1.Active && c2.Active && !alreadyChecked.Contains(c2))
                    {
                        if(c1.Type == CollisionComponent.CollisionType.STATIC &&
                           c2.Type == CollisionComponent.CollisionType.DYNAMIC)
                        {
                            _ProcessStaticDynamicCollision(c1,c2);
                        }
                        else if(c1.Type == CollisionComponent.CollisionType.DYNAMIC &&
                                c2.Type == CollisionComponent.CollisionType.STATIC)
                        {
                            _ProcessStaticDynamicCollision(c2, c1);
                        }
                        else
                        {
                            _ProcessDynamicCollision(c1, c2);
                        }
                    }
                }

                alreadyChecked.Add(c1);
            }
        }

        private void _ProcessStaticDynamicCollision(CollisionComponent cStatic, CollisionComponent cDynamic)
        {
            float x = cDynamic.Parent.Velocity.X;
            float y = cDynamic.Parent.Velocity.Y;
            Vector2 nextFrame = cDynamic.Parent.Position_NextFrame;

            Rectangle staticRect = new Rectangle((int)(cStatic.Position.X + cStatic.Offset.X),
                                                 (int)(cStatic.Position.Y + cStatic.Offset.Y),
                                                 cStatic.Width, cStatic.Height);

            Rectangle dynamicRect_X = new Rectangle((int)(cDynamic.Parent.Position_NextFrame.X + cDynamic.Offset.X),
                                                    (int)(cDynamic.Position.Y + cDynamic.Offset.Y),
                                                    cDynamic.Width,cDynamic.Height);
            Rectangle dynamicRect_Y = new Rectangle((int)(cDynamic.Position.X + cDynamic.Offset.X),
                                                    (int)(cDynamic.Parent.Position_NextFrame.Y + cDynamic.Offset.Y),
                                                    cDynamic.Width, cDynamic.Height);

            if (staticRect.Intersects(dynamicRect_X))
            {
                x = 0;
            }

            if (staticRect.Intersects(dynamicRect_Y))
            {
                y = 0;
            }

            cDynamic.Parent.Velocity = new Vector2(x, y);
        }

        private void _ProcessDynamicCollision(CollisionComponent c1, CollisionComponent c2)
        {
            Rectangle rect1 = new Rectangle((int)(c1.Position.X + c1.Offset.X),
                                            (int)(c1.Position.Y + c1.Offset.Y),
                                            c1.Width, c1.Height);

            Rectangle rect2 = new Rectangle((int)(c2.Position.X + c2.Offset.X),
                                            (int)(c2.Position.Y + c2.Offset.Y),
                                            c2.Width, c2.Height);

            if (rect1.Intersects(rect2))
            {
                if (c1.CollisionTypes.Find(s => s == c2.Parent.Type) != null)
                {
                    c1.AddCollisionObject(c2.Parent);
                }

                if (c2.CollisionTypes.Find(s => s == c1.Parent.Type) != null)
                {
                    c2.AddCollisionObject(c1.Parent);
                }
            }
        }

        private void _HandleInteraction(InteractionEvent interEvent)
        {
            if (Objects.Find(go => go == interEvent.Source) != null)
            {
                foreach (GameObject go in Objects)
                {
                    ScriptComponent scriptComponent = go.GetComponent<ScriptComponent>();
                    if (scriptComponent != null)
                    {
                        List<CollisionComponent> collisionComponents = go.GetComponents<CollisionComponent>();
                        foreach (CollisionComponent c in collisionComponents)
                        {
                            if (c.Type == CollisionComponent.CollisionType.DYNAMIC)
                            {
                                Rectangle collisionRect = new Rectangle((int)(c.Position.X + c.Offset.X),
                                                                        (int)(c.Position.Y + c.Offset.Y),
                                                                        c.Width, c.Height);

                                if (interEvent.InteractionArea.Intersects(collisionRect))
                                {
                                    scriptComponent.CallFunction("OnInteraction", new object[] { interEvent.Source });
                                }
                            }
                        }
                    }                     
                }
            }
        }
    }
}
