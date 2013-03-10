using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using RpgGame.Manager;
using RpgGame.Tools;

using Microsoft.Xna.Framework.Graphics;

namespace RpgGame.GameComponents
{
    class ScriptComponent : BaseGameComponent
    {
        public  string          ScriptFile  { get; private set; }
        private CompiledScript  Script      { get; set; }


        public ScriptComponent(string scriptfile) : base("ScriptComponent")
        {
            ScriptFile = scriptfile;
        }

        public override void Init()
        {
            //Try to load script via script manager
            Script = ScriptManager.CompileScript(ScriptFile);

            //Call secretly wrote function
            CallFunction("SetParent", new object[] { Parent });
            
            //Call init method.
            CallFunction("Init");
        }

        public override void Update(GameTime gameTime)
        {
            CallFunction("Update", new object[] { gameTime.ElapsedGameTime.Milliseconds });
        }

        public override void Draw(ref SpriteBatch batch)
        {
            CallFunction("Draw",null);
        }

        public bool HasFunction(String FunctionName)
        {
            if(Script == null){
                return false;
            }

            return Script.HasFunction(FunctionName);
        }

        public void CallFunction(String FunctionName)
        {
            CallFunction(FunctionName,null);
        }

        public void CallFunction(String FunctionName, object[] Parameter)
        {
            //Only call a function in the script if the script has been compiled successfully and if the
            //component is active.
            if (Active && Script != null && Script.CompiledSuccessfully){
                Script.CallFunction(FunctionName, Parameter);
            }
        }
    }
}
