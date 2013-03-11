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
        private static CSharpCodeProvider                       CodeProvider      { get; set; }
        private static CompilerParameters                       CompileParameters { get; set; }
        private static Dictionary<String, CompiledScript>       CompiledScripts   { get; set; }
        private static Dictionary<String, FileSystemWatcher>    ScriptWatcher     { get; set; }

        public static void FileChangedHandler(object sender,FileSystemEventArgs e)
        {
            //Get the class name of the script that changed (class name = script filename without extensions)
            String ClassName = Path.GetFileNameWithoutExtension(e.FullPath);

            DeveloperConsole.AddMessage(DeveloperConsole.MessageType.TRACE,"Update of script '" + Path.GetFileName(e.FullPath) + "' detected.");

            //Compile the new script
            CompiledScript UpdatedScript = _Compile(e.FullPath);

            //Check if the new script has been compiled successfully
            if(UpdatedScript.CompiledSuccessfully){
                //Get the script we saved previously during first compilation
                CompiledScript SavedScript = null;
                CompiledScripts.TryGetValue(ClassName,out SavedScript);

                //Update the values from the SavedScript with the values of the updated script
                SavedScript.Update(UpdatedScript);
            }
        }

        public static void Initialize()
        {
            CodeProvider        = new CSharpCodeProvider();
            CompileParameters   = new CompilerParameters();
            CompiledScripts     = new Dictionary<String, CompiledScript>();
            ScriptWatcher       = new Dictionary<String, FileSystemWatcher>();

            CompileParameters.GenerateInMemory      = true;
            CompileParameters.GenerateExecutable    = false;
            
            #if (DEBUG)
                CompileParameters.IncludeDebugInformation = true;
            #endif

            //Add local RpgGame assembly
            CompileParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            
            //Add XNA assembly
            CompileParameters.ReferencedAssemblies.Add(Environment.GetEnvironmentVariable("XNAGSv4") + @"\References\Windows\x86\Microsoft.Xna.Framework.dll");
        
            //Add system assembly
            CompileParameters.ReferencedAssemblies.Add("System.dll");
        }

        public static CompiledScript CompileScript(string ScriptFile)
        {
            //Get the full script path
            String ScriptPath   = Path.GetFullPath(ScriptFile);
            ScriptPath          = ScriptPath.Remove(ScriptPath.LastIndexOf("\\") + 1);

            FileSystemWatcher Watcher = null;
            
            //the filesystemwatcher checks whether or not the script has been changed during runtime.
            //if the scriptfile changes, the filesystemwatcher notifies the scriptmanager which in turn
            //recompiled the script.
            if (!ScriptWatcher.ContainsKey(ScriptPath))
            {
                Watcher = new FileSystemWatcher();
                Watcher.Path = ScriptPath;
                Watcher.Filter = "*.cs";
                Watcher.NotifyFilter = NotifyFilters.LastWrite;

                //Set changed handler that recompiles the script.
                Watcher.Changed += FileChangedHandler;

                Watcher.EnableRaisingEvents = true;
                ScriptWatcher.Add(ScriptPath, Watcher);
            }

            //Compile Script
            CompiledScript Script = _Compile(ScriptFile);
            
            //If the script has been compiled successfully, add it to a list of compiled scripts.
            if(Script.CompiledSuccessfully){
                //Check if the script has been added previously
                if(CompiledScripts.ContainsKey(Script.ClassName)){
                    //remove old script from the list
                    CompiledScripts.Remove(Script.ClassName);
                }
                //add script to the list.
                CompiledScripts.Add(Script.ClassName,Script);
            }

            return Script;
        }

        private static String _WriteSecretFunctions(string sourceCode)
        {
            String SecretCode = "\nprivate GameObject Parent {get;set;}\n" +
                                "public void SetParent(GameObject parent){Parent = parent;}";

            int StartOfClass = sourceCode.IndexOf('{');
            sourceCode = sourceCode.Insert(StartOfClass + 1, SecretCode);

            return sourceCode;
        }

        private static CompiledScript _Compile(string scriptFile)
        {
            CompiledScript Script   = new CompiledScript();
            Script.ClassName        = Path.GetFileNameWithoutExtension(scriptFile);
            Script.FilePath         = scriptFile;

            //Try the load the script file
            if(File.Exists(Script.FilePath)){
                Script.SourceCode   = File.ReadAllText(Script.FilePath);
                Script.SourceCode   = _WriteSecretFunctions(Script.SourceCode);
            }else{
                //Could not find script file. Mark compilation as unsuccessful, set an error and return the script.
                Script.CompiledSuccessfully = false;
                Script.CompileErrors        = new string[] {"Scriptfile '" + Script.FilePath + "' not found."};

                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,String.Concat(Script.CompileErrors));

                return Script;
            }

            DeveloperConsole.AddMessage(DeveloperConsole.MessageType.TRACE,"Starting to compile '" + Script.FilePath + "'...");

            //Compile the script.
            CompilerResults Results = CodeProvider.CompileAssemblyFromSource(CompileParameters, new String[] { Script.SourceCode });
            
            //Check for Errors during compilation
            if (Results.Errors.Count > 0)
            {
                String ErrorString = "";
                List<String> Errors = new List<String>();
                foreach (CompilerError Error in Results.Errors)
                {
                    //Only check for errors, not warnings
                    if(!Error.IsWarning){
                        ErrorString = String.Format("Error {0}\nFile: {1} Line: {2}\nDescription:{3}\n\n",
                                                     Error.ErrorNumber,Script.FilePath,Error.Line,Error.ErrorText);

                        Errors.Add(ErrorString);
                    }
                }

                //Mark compilation as unsuccessful and set errors if there where any errors during compilation
                if(Errors.Count > 0){
                    Script.CompiledSuccessfully = false;
                    Script.CompileErrors        = Errors.ToArray();

                    DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,String.Concat(Script.CompileErrors));

                    return Script;
                }
            }

            //Try to load the class from the compiled script (class = filename without extension)
            try{
                Script.ByteCode             = Results.CompiledAssembly.CreateInstance(Script.ClassName);
                Script.CompiledSuccessfully = true;
            }catch(Exception ex){
                //If the class could not be found, mark the compilation as unsuccessful and set the stack trace
                //as compile error.
                Script.CompiledSuccessfully = false;
                Script.CompileErrors        = new string[] {ex.StackTrace};

                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.ERROR,String.Concat(Script.CompileErrors));
            }
        
            if(Script.CompiledSuccessfully){
                DeveloperConsole.AddMessage(DeveloperConsole.MessageType.TRACE,"Script '" + Script.FilePath + "' compiled successfully.");
            }

            return Script;
        }
    }
}
