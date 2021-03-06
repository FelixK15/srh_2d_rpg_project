﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;

namespace RpgGame.Tools
{
    public class CompiledScript
    {        
        public string           FilePath                { get; set; }
        public string           ClassName               { get; set; }
        public string           SourceCode              { get; set; }
        public object           ByteCode                { get; set; }
        public bool             CompiledSuccessfully    { get; set; }
        public string[]         CompileErrors           { get; set; }

        public CompiledScript()
        {
            ByteCode             = null;
            CompileErrors        = null;
            CompiledSuccessfully = false;
            SourceCode           = "";
            FilePath             = "";
            ClassName            = "";
        }

        public CompiledScript(object bytecode) : this()
        {
            ByteCode = bytecode;
        }

        public bool HasFunction(String FunctionName)
        {
            bool ReturnValue = false;
            if(ByteCode != null){
                MethodInfo Info = ByteCode.GetType().GetMethod(FunctionName);
                ReturnValue = Info != null;
            }

            return ReturnValue;
        }

        public void CallFunction(String FunctionName)
        {
            CallFunction(FunctionName, null);
        }

        public void CallFunction(String FunctionName, object[] Parameter)
        {
            if(ByteCode != null){
                MethodInfo Info = ByteCode.GetType().GetMethod(FunctionName);
                if (Info != null){
                    Info.Invoke(ByteCode, Parameter);
                }
            }
        }

        public void Update(CompiledScript newScript)
        {
            //This function just updates the value from another script.
            this.ByteCode   = newScript.ByteCode;
            this.SourceCode = newScript.SourceCode;
        }
    }
}
