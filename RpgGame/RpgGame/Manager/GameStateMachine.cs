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
                if(GameStates.Count > 0){
                    return GameStates.Last<IGameState>();
                }

                return null;
            }
        }
        
        public static void Initialize()
        {
            GameStates = new List<IGameState>();
            
        }

        public static void AddState(IGameState State)
        {
            if(CurrentState != null){
                CurrentState.Stop();
            }

            GameStates.Add(State);
            State.Start();
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
