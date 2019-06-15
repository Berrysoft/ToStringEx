using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ToStringEx.MethodInfoHelpers
{
    internal static class FSharpMethodInfoFormatterHelper
    {
        private static readonly Dictionary<Type, string> PreDefinedTypes = new Dictionary<Type, string>
        {
            [typeof(bool)] = "bool",
            [typeof(byte)] = "byte",
            [typeof(sbyte)] = "sbyte",
            [typeof(char)] = "char",
            [typeof(short)] = "int16",
            [typeof(ushort)] = "uint16",
            [typeof(int)] = "int",
            [typeof(uint)] = "uint32",
            [typeof(long)] = "int64",
            [typeof(ulong)] = "uint64",
            [typeof(IntPtr)] = "nativeint",
            [typeof(UIntPtr)] = "unativeint",
            [typeof(float)] = "single",
            [typeof(double)] = "double",
            [typeof(decimal)] = "decimal",
            [typeof(string)] = "string",
            [typeof(void)] = "void"
        };

        private static string GetTypeFullName(ParameterInfo p)
        {
            Type t = p.ParameterType;
            Type et = t.GetElementType() ?? t;
            StringBuilder builder = new StringBuilder();
            if (t.IsByRef)
            {
                if (p.IsOut)
                {
                    builder.Append("outref<");
                }
                else if (p.IsIn)
                {
                    builder.Append("inref<");
                }
                else
                {
                    builder.Append("byref<");
                }
            }
            if (PreDefinedTypes.TryGetValue(et, out string type))
            {
                builder.Append(type);
            }
            else
            {
                builder.Append(et.FullName).Replace('/', '.');
            }
            if (t.IsByRef)
                builder.Append('>');
            else if (t.IsArray)
                builder.Append("[]");
            else if (t.IsPointer)
                builder.Append('*');
            return builder.ToString();
        }

        public static string FormatInternal(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("let ");
            builder.Append(method.Name);
            builder.Append(' ');
            foreach (var p in method.GetParameters())
            {
                builder.AppendFormat("({0} : {1}) ", p.Name, GetTypeFullName(p));
            }
            builder.Append(": ");
            builder.Append(GetTypeFullName(method.ReturnParameter));
            return builder.ToString();
        }
    }
}
