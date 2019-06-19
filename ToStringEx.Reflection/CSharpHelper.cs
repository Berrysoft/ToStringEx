using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToStringEx.Reflection
{
    internal class CSharpHelper : ILanguageHelper
    {
        private static readonly Dictionary<Type, string> PreDefinedTypes = new Dictionary<Type, string>
        {
            [typeof(bool)] = "bool",
            [typeof(byte)] = "byte",
            [typeof(sbyte)] = "sbyte",
            [typeof(char)] = "char",
            [typeof(short)] = "short",
            [typeof(ushort)] = "ushort",
            [typeof(int)] = "int",
            [typeof(uint)] = "uint",
            [typeof(long)] = "long",
            [typeof(ulong)] = "ulong",
            [typeof(float)] = "float",
            [typeof(double)] = "double",
            [typeof(decimal)] = "decimal",
            [typeof(string)] = "string",
            [typeof(object)] = "object",
            [typeof(void)] = "void"
        };

        private static readonly Dictionary<MethodAttributes, string> AccessStringMap = new Dictionary<MethodAttributes, string>
        {
            [MethodAttributes.Private] = "private",
            [MethodAttributes.Public] = "public",
            [MethodAttributes.Family] = "protected",
            [MethodAttributes.Assembly] = "internal",
            [MethodAttributes.FamORAssem] = "protected internal",
            [MethodAttributes.FamANDAssem] = "private protected"
        };

        private static string GetVTableString(MethodAttributes attr)
        {
            StringBuilder builder = new StringBuilder();
            if (attr.HasFlag(MethodAttributes.Abstract))
                builder.Append("abstract");
            else if (attr.HasFlag(MethodAttributes.Static))
                builder.Append("static");
            else if (attr.HasFlag(MethodAttributes.Virtual))
            {
                if (attr.HasFlag(MethodAttributes.Final))
                    builder.Append("sealed ");
                if (attr.HasFlag(MethodAttributes.NewSlot))
                    builder.Append("virtual");
                else
                    builder.Append("override");
            }
            return builder.ToString();
        }

        private static string GetTypeName(Type t, string[] genericTypes)
        {
            Type et = t.GetElementType() ?? t;
            StringBuilder builder = new StringBuilder();
            if (PreDefinedTypes.TryGetValue(et, out string type))
            {
                builder.Append(type);
            }
            else
            {
                if (et != t)
                {
                    builder.Append(GetTypeName(et, genericTypes));
                }
                else
                {
                    if (genericTypes != null)
                    {
                        if (et.IsGenericParameter)
                        {
                            builder.Append(genericTypes[et.GenericParameterPosition]);
                        }
                        else if (et.IsGenericType)
                        {
                            builder.Append(et.Namespace);
                            builder.Append('.');
                            builder.Append(et.Name.Substring(0, et.Name.IndexOf('`'))).Replace('/', '.');
                            builder.Append('<');
                            builder.AppendJoin(", ", et.GetGenericArguments().Select(tt => tt.IsGenericParameter ? genericTypes[tt.GenericParameterPosition] : GetTypeName(tt, genericTypes)));
                            builder.Append('>');
                        }
                        else
                        {
                            builder.Append(et.FullName ?? et.Name).Replace('/', '.');
                        }
                    }
                    else
                    {
                        builder.Append(et.FullName ?? et.Name).Replace('/', '.');
                    }
                }
            }
            if (t.IsArray)
                builder.Append("[]");
            else if (t.IsPointer)
                builder.Append('*');
            return builder.ToString();
        }

        private static string GetTypeFullName(ParameterInfo p, string[] genericTypes)
        {
            Type t = p.ParameterType;
            StringBuilder builder = new StringBuilder();
            if (t.IsByRef)
            {
                if (p.IsOut)
                {
                    builder.Append("out ");
                }
                else if (p.IsIn)
                {
                    builder.Append("in ");
                }
                else
                {
                    builder.Append("ref ");
                }
            }
            if (p.GetCustomAttribute(typeof(ParamArrayAttribute)) != null)
                builder.Append("params ");
            builder.Append(GetTypeName(t, genericTypes));
            return builder.ToString();
        }

        private static string GetFullParameter(ParameterInfo p, string[] genericTypes)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetTypeFullName(p, genericTypes));
            builder.Append(' ');
            builder.Append(p.Name);
            if (p.IsOptional)
                builder.AppendFormat(" = {0}", p.DefaultValue);
            return builder.ToString();
        }

        public string FormatMethodInfo(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(AccessStringMap[method.Attributes & MethodAttributes.MemberAccessMask]);
            builder.Append(' ');
            var vs = GetVTableString(method.Attributes);
            if (!string.IsNullOrEmpty(vs))
            {
                builder.Append(vs);
                builder.Append(' ');
            }
            if (method.ReturnType.IsPointer || method.GetParameters().Any(p => p.ParameterType.IsPointer))
            {
                builder.Append("unsafe ");
            }
            var genericTypes = method.GetGenericArguments().Select(t => GetTypeName(t, null)).ToArray();
            builder.Append(GetTypeFullName(method.ReturnParameter, genericTypes));
            builder.Append(' ');
            builder.Append(method.Name);
            if (genericTypes.Length > 0)
            {
                builder.Append('<');
                builder.AppendJoin(", ", genericTypes);
                builder.Append('>');
            }
            builder.Append('(');
            var ps = method.GetParameters().Select(p => GetFullParameter(p, genericTypes));
            if (method.CallingConvention.HasFlag(CallingConventions.VarArgs))
                ps = ps.Append("__arglist");
            builder.AppendJoin(", ", ps);
            builder.Append(')');
            return builder.ToString();
        }

        public string FormatType(Type type) => GetTypeName(type, null);
    }
}
