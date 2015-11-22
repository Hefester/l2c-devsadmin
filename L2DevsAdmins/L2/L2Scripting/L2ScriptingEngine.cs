using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.Emit;
using System.IO;
using System.Reflection;
using NLua;
using L2.Net;
using L2.Net.Attributes;

namespace L2.L2Scripting
{


    public class PythonEngine
    {
        //some
    }

    /// <summary>
    /// Lua Engine.
    /// </summary>
    public class LuaEngine
    {

        private Lua engine = new Lua();
        private Quest q = new Quest(-1, "Test", "---");

        public LuaEngine(string path)
        {

            if (File.Exists(path))
            {
                Path = path;
                LoadVisibleFunctions();
                LoadVisibleVars();
                engine.DoFile(path);
                engine.LoadCLRPackage();
            }
            else
            {
                Logger.WriteLine("Missing Script: " + path);
            }

        }

        void LoadVisibleFunctions()
        {
            Type t = typeof(Quest);
            MethodInfo[] ms = t.GetMethods();
            foreach (MethodInfo method in ms)
            {
                LuaVisible attr = (LuaVisible)method.GetCustomAttribute(typeof(LuaVisible), true);
                if (attr.Visible)
                {
                    engine.RegisterFunction(method.Name, q, method);
                }
            }
        }

        void LoadVisibleVars()
        {
            Type t = typeof(Quest);
            FieldInfo[] fields = t.GetFields();

            foreach (FieldInfo field in fields)
            {
                LuaVisible attr = (LuaVisible)field.GetCustomAttribute(typeof(LuaVisible), true);
                if (attr.Visible)
                {
                    engine[field.Name] = field.GetValue(q);
                }
            }

        }

        public object GetProperties(string name)
        {
            return engine[name];
        }

        /// <summary>
        /// Clear All
        /// </summary>
        public void Dispose()
        {
            try
            {
                engine.Dispose();
                engine.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Compilator Lua.
        /// </summary>
        public Lua Compiler
        {
            get
            {
                return engine;
            }
            protected set
            {
                engine = value;
            }
        }

        public string Path
        {
            get;
            protected set;
        }

        public bool ContainsMethod(string func)
        {
            LuaFunction funct = engine.GetFunction(func);
            return funct != null;
        }

        /// <summary>
        /// Run a method by LUA
        /// </summary>
        /// <param name="func"></param>
        /// <param name="args"></param>
        public void RunMethod(string func, params object[] args)
        {
            LuaFunction funct = engine.GetFunction(func);
            funct.Call(args);
        }
        /// <summary>
        /// Get Method Execute.
        /// </summary>
        /// <param name="func"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object GetMethodReturn(string func, params object[] args)
        {
            LuaFunction funct = engine.GetFunction(func);
            return funct.Call(args);
        }

    }


    public class VBScriptEngine
    {
        private SyntaxTree tree;
        private List<MetadataReference> references = new List<MetadataReference>();
        private VisualBasicCompilation compiler;

        public string Path
        {
            get;
            protected set;
        }

        /// <summary>
        /// a full class main
        /// </summary>

        public string AssemblySpaceName
        {
            get;
            set;
        }

        public VisualBasicCompilation Compiler
        {
            get
            {
                return compiler;
            }
            set
            {
                compiler = value;
            }
        }

        public VBScriptEngine(string path)
        {
            if (File.Exists(path))
            {
                Path = path;
                string code = File.ReadAllText(path);
                tree = VisualBasicSyntaxTree.ParseText(@code);
                //Basic references.
                AddReference(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
                AddReference(MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location));
                AddReference(MetadataReference.CreateFromFile(typeof(Quest).Assembly.Location));
            }
            else
            {
                Logger.WriteLine("[Scripting]: Missing Script: " + path);
            }
        }

        /// <summary>
        /// Add a reference.
        /// </summary>
        /// <param name="refe"></param>
        /// <returns></returns>
        public virtual bool AddReference(MetadataReference refe)
        {
            references.Add(refe);
            return true;
        }



        /// <summary>
        /// Build Script
        /// </summary>
        public virtual void Build()
        {

            Compiler = VisualBasicCompilation.Create(
                AssemblySpaceName,
                syntaxTrees: new[] { tree },
                references: references.ToArray(),
                options: new VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        //clear compiler.
        public void Dispose()
        {
            Compiler = null;
            tree = null;
            references.Clear();
        }

        /// <summary>
        /// New instance SCRIPT.
        /// </summary>
        /// <returns></returns>
        public object NewInstance()
        {
            object obj = null;
            using (var ms = new MemoryStream())
            {
                EmitResult result = Compiler.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Logger.WriteLine("[Scripting]: Error {0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType(AssemblySpaceName);
                    obj = Activator.CreateInstance(type);
                }
            }
            return obj;
        }

        public void RunMethod(string fullClass, string name, params object[] args)
        {
            using (var ms = new MemoryStream())
            {
                EmitResult result = Compiler.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Logger.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType(fullClass);
                    object obj = Activator.CreateInstance(type);

                    type.InvokeMember(name,
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        args);
                }
            }
        }

    }

    public class CSharpScriptEngine
    {
        private SyntaxTree tree;
        private List<MetadataReference> references = new List<MetadataReference>();
        private CSharpCompilation compiler;

        public string Path
        {
            get;
            protected set;
        }

        public string AssemblySpaceName
        {
            get;
            set;
        }

        public CSharpCompilation Compiler
        {
            get
            {
                return compiler;
            }
            set
            {
                compiler = value;
            }
        }

        public CSharpScriptEngine(string path)
        {
            if (File.Exists(path))
            {
                Path = path;
                string code = File.ReadAllText(path);
                tree = CSharpSyntaxTree.ParseText(@code);
                //Basic references.
                AddReference(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
                AddReference(MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location));
                AddReference(MetadataReference.CreateFromFile(typeof(Quest).Assembly.Location));
            }
            else
            {
                Logger.WriteLine("[Scripting]: Missing Script: " + path);
            }
        }

        public virtual bool AddReference(MetadataReference refe)
        {
            references.Add(refe);
            return true;
        }



        /// <summary>
        /// Build Script
        /// </summary>
        public virtual void Build()
        {

            Compiler = CSharpCompilation.Create(
                AssemblySpaceName,
                syntaxTrees: new[] { tree },
                references: references.ToArray(),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        public void Dispose()
        {
            Compiler = null;
            tree = null;
            references.Clear();

        }

        public object NewInstance()
        {
            object obj = null;
            using (var ms = new MemoryStream())
            {
                EmitResult result = Compiler.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    Type type = assembly.GetType(AssemblySpaceName);
                    obj = Activator.CreateInstance(type);
                }
            }
            return obj;
        }


        public void RunMethod(string name, params object[] args)
        {
            using (var ms = new MemoryStream())
            {
                EmitResult result = Compiler.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    Type type = assembly.GetType(AssemblySpaceName);
                    object obj = Activator.CreateInstance(type);

                    type.InvokeMember(name,
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        obj,
                        args);
                }
            }
        }

    }

    /// <summary>
    /// Read All Scripts
    /// </summary>
    public class L2ScriptingEngine
    {
        //Singleton Thread-Safe
        private static readonly object _lock = new object();
        private static L2ScriptingEngine _instance = null;
        //Vars
        private List<Quest> _scripts = new List<Quest>();
        private const string DefaultDirectory = @"Game\Data\Scripts\";
        private const string XmlDirectory = @DefaultDirectory + "scripts.xml";

        private L2ScriptingEngine()
        {
            load();
        }

        /// <summary>
        /// Gets singleton instance (Thread Safe)
        /// </summary>
        public static L2ScriptingEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new L2ScriptingEngine();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Read & Load the scripts
        /// </summary>
        public void load()
        {
            _scripts.Clear();
            XDocument doc = XDocument.Load(XmlDirectory);

            var scripts = from script in doc.Descendants("Scripts") select script;

            foreach (XElement script in scripts.Elements("Script"))
            {
                Quest quest = null;
                string file = DefaultDirectory + @script.Attribute("File").Value;
                string language = script.Attribute("Language").Value;
                string classSpace = script.Attribute("ClassSpace").Value;
                XElement xreferences = script.Element("References");

                var references = from reference in xreferences.Descendants("Reference") select reference;

                if (file.EndsWith(".cs"))
                {
                    CSharpScriptEngine engine = new CSharpScriptEngine(file);
                    engine.AssemblySpaceName = classSpace;
                    AddMetaRefs(ref engine, references);
                    engine.Build();
                    try
                    {
                        Quest q = engine.NewInstance() as Quest;//loaded quest
                        _scripts.Add(q);
                        if (q.PrintDebug)
                        {
                            Logger.WriteLine("Quest: [{0}]: Loaded Quest. [C#]", q.Name);
                        }
                        engine.Dispose();
                    }
                    catch (Exception e)
                    {
                        Logger.WriteLine("Error in Script not instance of Quest: " + file);
                        Logger.WriteLine("Error es: " + e.Message);
                    }

                }
                else if (file.EndsWith(".vb"))
                {
                    VBScriptEngine engine = new VBScriptEngine(file);
                    engine.AssemblySpaceName = classSpace;
                    AddMetaRefs(ref engine, references);
                    engine.Build();
                    try
                    {
                        Quest q = (Quest)engine.NewInstance();
                        _scripts.Add(q);
                        if (q.PrintDebug)
                        {
                            Logger.WriteLine("Quest: [{0}]: Loaded Quest. [VB]", q.Name);
                        }
                        engine.Dispose();
                    }
                    catch
                    {
                        Logger.WriteLine("Error in Script not instance of Quest: " + file);
                    }
                }
                else if (file.EndsWith(".lua", StringComparison.OrdinalIgnoreCase))
                {
                    LuaEngine engine = new LuaEngine(file);
                    int q_Id = (int)engine.GetProperties("QuestId");
                    string name = (string)engine.GetProperties("Name");
                    string descr = (string)engine.GetProperties("Descr");
                    quest = new Quest(q_Id, name, descr);
                    quest.loadByScriptEngine(engine);
                }
                else if (file.EndsWith(".py"))
                {
                    //action
                }
            }
            Logger.WriteLine("[Scripting]: Loaded {0} Quests.", _scripts.Where(q => q.IsRealQuest).Count());
        }

        List<MetadataReference> GetReferences(IEnumerable<XElement> elms)
        {
            List<MetadataReference> data = new List<MetadataReference>();
            foreach (XElement elm in elms)
            {
                string ass = elm.Attribute("ref").Value;
                data.Add(MetadataReference.CreateFromFile(Assembly.Load(ass).Location));
            }
            return data;
        }


        void AddMetaRefs(ref CSharpScriptEngine engine, IEnumerable<XElement> elms)
        {
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                engine.AddReference(MetadataReference.CreateFromFile(ass.Location));
            }
        }

        void AddMetaRefs(ref VBScriptEngine engine, IEnumerable<XElement> elms)
        {
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                engine.AddReference(MetadataReference.CreateFromFile(ass.Location));
            }
        }

    }
}
