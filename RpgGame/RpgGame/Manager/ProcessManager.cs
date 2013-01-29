using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using RpgGame.Processes;

namespace RpgGame.Manager
{
    class ProcessManager
    {
        public static List<Process> Processes
        {
            get;
            private set;
        }

        public static void Initialize()
        {
            Processes = new List<Process>();
        }

        public static void Update(GameTime gameTime)
        {
            List<Process> ToRemove = new List<Process>();
            List<Process> ToAdd = new List<Process>();

            foreach(Process p in Processes)
            {
                if (!p.Started)
                {
                    p.Start();
                }

                p.Update(gameTime);

                if (p.Finished)
                {
                    p.End();
                    ToRemove.Add(p);
                    if (p.Next != null)
                    {
                        ToAdd.Add(p.Next);
                    }
                }
            }

            foreach (Process p in ToAdd)
            {
                Processes.Add(p);
            }

            foreach (Process p in ToRemove)
            {
                Processes.Remove(p);
            }

            ToRemove.Clear();
        }
    }
}
