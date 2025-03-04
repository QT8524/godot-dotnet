﻿namespace Godot
{
    public sealed partial class CSharpLanguage : ScriptLanguageExtension
    {
        internal static readonly nint NamePtr = (nint)(StringName)"C#";

        private static CSharpLanguage _self;
        public static CSharpLanguage Singleton { get => _self ??= ClassDB.InstantiateManaged<CSharpLanguage>(); }
        internal static nint LanguageHandle => Singleton.Handle;
        internal static unsafe void Register()
        {
            Engine.RegisterScriptLanguage(Singleton);
        }

        internal static unsafe void Unregister()
        {
            Engine.UnregisterScriptLanguage(Singleton);
        }

        public override GodotObject _CreateScript()
            => new CSharpScript();
        public override string _GetName()
            => "C#";
        public override string[] _GetReservedWords()
            => new string[]
            {
                "abstract",
                "as",
                "base",
                "bool",
                "break",
                "byte",
                "case",
                "catch",
                "char",
                "checked",
                "class",
                "const",
                "continue",
                "decimal",
                "default",
                "delegate",
                "do",
                "double",
                "else",
                "enum",
                "event",
                "explicit",
                "extern",
                "false",
                "finally",
                "fixed",
                "for",
                "foreach",
                "goto",
                "if",
                "implicit",
                "in",
                "int",
                "interface",
                "internal",
                "is",
                "lock",
                "long",
                "namespace",
                "new",
                "null",
                "object",
                "operator",
                "out",
                "override",
                "params",
                "private",
                "protected",
                "public",
                "readonly",
                "ref",
                "return",
                "sbyte",
                "sealed",
                "short",
                "sizeof",
                "stackalloc",
                "static",
                "string",
                "struct",
                "switch",
                "this",
                "throw",
                "true",
                "try",
                "typeof",
                "uint",
                "ulong",
                "unchecked",
                "unsafe",
                "ushort",
                "using",
                "virtual",
                "void",
                "volatile",
                "while",
            };
        public override bool _SupportsDocumentation()
            => false;
        public override string _GetExtension()
            => "cs";
        public override string _GetType()
            => nameof(CSharpScript);
        public override bool _HandlesGlobalClassType(string type)
            => TypeDB.Search(type, out _);

        public override void _Init()
        {
            System.Console.WriteLine("hi c#!! :3");
        }

        public override void _Frame()
        {
        }


        public static bool _CanCall(StringName method)
        {
            return method == "_create_script" ||
                method == "_get_name" ||
                method == "_supports_documentation" ||
                method == "_get_extension" ||
                method == "_get_type" ||
                method == "_init" ||
                method == "_handles_global_classa_type" ||
                method == "_frame";
        }

        protected override void _Call(StringName method, ref Variant arg0, ref Variant ret)
        {
            if (method == "_create_script")
            {
                ret = _CreateScript();
                return;
            }

            if (method == "_get_name")
            {
                ret = _GetName();
                return;
            }

            if (method == "_supports_documentation")
            {
                ret = _SupportsDocumentation();
                return;
            }

            if (method == "_get_extension")
            {
                ret = _GetExtension();
                return;
            }

            if (method == "_get_type")
            {
                ret = _GetType();
                return;
            }

            if (method == "_init")
            {
                _Init();
                return;
            }    

            if (method == "_handles_global_class_type")
            {
                ret = _HandlesGlobalClassType(arg0.StringName);
                return;
            }

            if (method == "_frame")
            {
                _Frame();
                return;
            }
        }

        internal CSharpLanguage() : base(false) { }
    }
}
