using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ToStringEx.Reflection
{
    internal class FSharpHelper : ILanguageHelper
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

        private static string GetTypeName(Type t)
        {
            Type et = t.GetElementType() ?? t;
            StringBuilder builder = new StringBuilder();
            if (PreDefinedTypes.TryGetValue(et, out string type))
            {
                builder.Append(type);
            }
            else
            {
                builder.Append(et == t ? et.FullName : GetTypeName(et)).Replace('/', '.');
            }
            if (t.IsArray)
                builder.Append("[]");
            else if (t.IsPointer)
                builder.Append('*');
            return builder.ToString();
        }

        private static string GetTypeFullName(ParameterInfo p)
        {
            Type t = p.ParameterType;
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
            builder.Append(GetTypeName(t));
            if (t.IsByRef)
                builder.Append('>');
            return builder.ToString();
        }

        public string FormatMethodInfo(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            if (method.Attributes.HasFlag(MethodAttributes.Static))
                builder.Append("static member ");
            else if (method.Attributes.HasFlag(MethodAttributes.Virtual))
            {
                if (method.Attributes.HasFlag(MethodAttributes.NewSlot))
                    builder.Append("abstract member ");
                else
                    builder.Append("override this.");
            }
            else
                builder.Append("member ");
            builder.Append(method.Name);
            builder.Append(' ');
            foreach (var p in method.GetParameters())
            {
                builder.Append('(');
                var realAttrs = p.CustomAttributes.Where(attr =>
                {
                    Type t = attr.AttributeType;
                    return t != typeof(InAttribute) && t != typeof(OutAttribute) && t.FullName != "System.Runtime.CompilerServices.IsReadOnlyAttribute";
                }).ToArray();
                bool hasAttr = realAttrs.Length > 0;
                if (hasAttr)
                    builder.Append("[<");
                builder.AppendJoin("; ", realAttrs.Select(attr =>
                {
                    var name = attr.AttributeType.Name;
                    StringBuilder b = new StringBuilder();
                    b.Append(name.Substring(0, name.Length - 9));
                    bool hasArguments = attr.ConstructorArguments.Count > 0 || attr.NamedArguments.Count > 0;
                    if (hasArguments)
                    {
                        b.Append('(');
                        b.AppendJoin(", ", attr.ConstructorArguments.Select(arg => arg.Value.ToString()).Concat(attr.NamedArguments.Select(arg => $"{arg.MemberName}={arg.TypedValue}")));
                        b.Append(')');
                    }
                    if (attr.AttributeType == typeof(OptionalAttribute))
                    {
                        b.Append("; DefaultParameterValue(");
                        b.Append(p.DefaultValue);
                        b.Append(')');
                    }
                    return b.ToString();
                }));
                if (hasAttr)
                    builder.Append(">] ");
                builder.AppendFormat("{0} : {1}", p.Name, GetTypeFullName(p));
                builder.Append(") ");
            }
            builder.Append(": ");
            builder.Append(GetTypeFullName(method.ReturnParameter));
            return builder.ToString();
        }
    }
}
