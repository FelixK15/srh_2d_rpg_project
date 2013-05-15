using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RpgGame.GameStates;

namespace RpgGame.Manager
{
    class GameStateMachine
    {
        public static List<IGameState> GameStates   { get; set; }
        public static IGameState       CurrentState 
        { 
            get{
                return GameStates.Last<IGameState>();
            }
        }
        
        public static void Initialize()
        {
            GameStates = new List<IGameState>();
        }

        public static void AddState(IGameState State)
        {
            if (GameStates.Count > 0)
            {
                CurrentState.Stop();
            }
            GameStates.Add(State);
            CurrentState.Start();
        }

        public static void RemoveTopState()
        {
            if(GameStates.Count > 0){
                CurrentState.Stop();
                GameStates.Remove(CurrentState);
            }
        }
    }
}
