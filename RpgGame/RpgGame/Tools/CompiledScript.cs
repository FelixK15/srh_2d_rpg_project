using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;

namespace RpgGame.Tools
{
    class CompiledScript
    {
        public delegate void OnChangedHandler(object compilation);

        public OnChangedHandler OnChanged { get; set; }
        public object Compilation { get; set; }

        public CompiledScript(object compilation)
        {
            OnChanged = null;
            Compilation = compilation;
        }

        public void CallFunction(String FunctionName)
        {
            CallFunction(FunctionName, null);
        }

        public void CallFunction(String FunctionName, object[] Parameter)
        {
            MethodInfo Info = Compilation.GetType().GetMethod(FunctionName);
            if (Info != null)
            {
                Info.Invoke(Compilation, Parameter);
            }
        }
    }
}
