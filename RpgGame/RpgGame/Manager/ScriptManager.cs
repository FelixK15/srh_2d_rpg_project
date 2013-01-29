using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

using RpgGame.Tools;

namespace RpgGame.Manager
{
    class ScriptManager
    {
        private static CSharpCodeProvider CodeProvider { get; set; }
        private static CompilerParameters CompileParameters { get; set; }
        private static Dictionary<String, CompiledScript> CompiledScripts { get; set; }
        private static Dictionary<String, FileSystemWatcher> ScriptWatcher { get; set; }

        public static void Initialize()
        {
            CodeProvider = new CSharpCodeProvider();
            CompileParameters = new CompilerParameters();
            CompiledScripts = new Dictionary<String, CompiledScript>();
            ScriptWatcher = new Dictionary<String, FileSystemWatcher>();

            CompileParameters.GenerateInMemory = true;
            CompileParameters.GenerateExecutable = false;
            CompileParameters.IncludeDebugInformation = true;

            //Add Assembly?
            CompileParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            CompileParameters.ReferencedAssemblies.Add(Environment.GetEnvironmentVariable("XNAGSv4") + @"\References\Windows\x86\Microsoft.Xna.Framework.dll");
        }

        public static CompiledScript CompileScript(string ScriptFile)
        {
            String ScriptPath = Path.GetFullPath(ScriptFile);
            ScriptPath = ScriptPath.Remove(ScriptPath.LastIndexOf("\\") + 1);
            FileSystemWatcher Watcher;
            if (!ScriptWatcher.ContainsKey(ScriptPath))
            {
                Watcher = new FileSystemWatcher();
                Watcher.Path = ScriptPath;
                Watcher.Filter = "*.cs";
                Watcher.NotifyFilter = NotifyFilters.LastWrite;
                Watcher.Changed += delegate(object sender, FileSystemEventArgs e)
                {
                    String ScriptClassName = Path.GetFileNameWithoutExtension(e.FullPath);

                    CompiledScript OldScript;
                    CompiledScripts.TryGetValue(ScriptClassName, out OldScript);

                    CompiledScript NewScript = _Compile(e.FullPath);
                    if (OldScript != null && OldScript.OnChanged != null)
                    {
                        OldScript.OnChanged(NewScript.Compilation);
                    }
                };
                Watcher.EnableRaisingEvents = true;
                ScriptWatcher.Add(ScriptPath, Watcher);
            }

            return _Compile(ScriptFile);
        }

        private static String _WriteSecretFunctions(string ScriptFile)
        {
            String SecretCode = "\nprivate GameObject Parent {get;set;}\n" +
                                "public void SetParent(GameObject parent){Parent = parent;}";

            String FileContent = null;
            bool ReadAll = true;

            do
            {          
                try
                {
                    FileContent = System.IO.File.ReadAllText(ScriptFile);
                    ReadAll = true;
                }
                catch (IOException ex)
                {
                    ReadAll = false;
                }
            } while (!ReadAll);

            int StartOfClass = FileContent.IndexOf('{');
            FileContent = FileContent.Insert(StartOfClass + 1, SecretCode);

            return FileContent;
        }

        private static CompiledScript _Compile(string ScriptFile)
        {
            String ClassName = Path.GetFileNameWithoutExtension(ScriptFile);
            String SourceCode = _WriteSecretFunctions(ScriptFile);
            //Try to check if the script has been loaded before.
//             if (CompiledScripts.ContainsKey(ClassName))
//             {
//                 //If it has, don't recompiled it. Just return the previously compiled version.
//                 CompiledScript Script = null;
//                 if (CompiledScripts.TryGetValue(ClassName, out Script))
//                 {
//                     return Script;
//                 }
//             }

            CompilerResults Results = CodeProvider.CompileAssemblyFromSource(CompileParameters, new String[] { SourceCode });

            //Check for Errors during compilatn
            if (Results.Errors.Count > 0)
            {
                String ErrorString = "";
                foreach (CompilerError ce in Results.Errors)
                {
                    ErrorString += ce.ErrorText + "\n";
                }

                //Throw Exception if errors happend during compilation
                throw new Exception(ErrorString);
            }

            //Get the compiled script
            object Compilation = Results.CompiledAssembly.CreateInstance(ClassName);

            //Create the new CompiledScript object and save it in the dictionary
            CompiledScript NewScript = new CompiledScript(Compilation);
            if (CompiledScripts.ContainsKey(ClassName))
            {
                CompiledScript OldScript = null;
                CompiledScripts.TryGetValue(ClassName,out OldScript);
                CompiledScripts.Remove(ClassName);
                NewScript.OnChanged = OldScript.OnChanged;
            }

            CompiledScripts.Add(ClassName, NewScript);
            
            return NewScript;
        }

    }
}
