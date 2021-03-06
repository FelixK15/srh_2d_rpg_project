﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RpgGame;
using RpgGame.GameComponents;
using RpgGame.Tools;
using RpgGame.Manager;

namespace ConsoleContext
{
    public class Context
    {
        public void ShowComponents(string ObjectName)
        {
            GameObject Object = GameObject.AllObjects.Find(go => go.Name == ObjectName);
            String Output = "";
            if(Object != null){
                foreach(IGameObjectComponent Component in Object.Components){
                    Output += String.Format("{0} - {1}\n",Component.Name,Component.ToString());
                }
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.TRACE,Output);
            }else{
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,"There's no object with the name '" + ObjectName + "'");
            }
        }

        public void ListObjects()
        {
            foreach(GameObject Obj in GameObject.AllObjects){
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.TRACE,Obj.Name);
            }
        }

        public void test(String a,String b,String c,String d)
        {
            DeveloperConsole.AddMessage(DeveloperConsole.MessageType.DEBUG,a + b + c + d);
        }

        //public void gamestates()
        //{
        //    String Output = "";
            
        //    DeveloperConsole.AddMessage(DeveloperConsole.MessageType.TRACE, Output);
        //}
    }
}
