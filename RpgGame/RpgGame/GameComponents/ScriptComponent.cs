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
        public string ScriptFile { get; private set; }

        private CompiledScript Script { get; set; }
        private bool Compiled { get; set; }

        public ScriptComponent(string scriptfile) : base("ScriptComponent")
        {
            ScriptFile = scriptfile;
            Compiled = false;
        }

        public override void Init()
        {
            try
            {
                Script = ScriptManager.CompileScript(ScriptFile);
                Script.OnChanged += delegate(object compilation)
                {
                    Script.Compilation = compilation;
                };
                Compiled = true;
                CallFunction("SetParent", new object[] { Parent });
                CallFunction("Init");
            }
            catch (System.Exception ex)
            {
                //console
            }
        }

        public override void Update(GameTime gameTime)
        {
            CallFunction("Update", new object[] { gameTime.ElapsedGameTime.Milliseconds });
        }

        public override void Draw(ref SpriteBatch batch)
        {
            CallFunction("Draw",null);
        }

        public void CallFunction(String FunctionName)
        {
            CallFunction(FunctionName,null);
        }

        public void CallFunction(String FunctionName, object[] Parameter)
        {
            if (Compiled && Active)
            {
                Script.CallFunction(FunctionName, Parameter);
            }
        }
    }
}
