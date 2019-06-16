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
            builder.Append(GetTypeName(t));
            return builder.ToString();
        }

        private static string GetFullParameter(ParameterInfo p)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetTypeFullName(p));
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
            if (method.ReturnType.IsPointer || method.GetParameters().Any(p => p.ParameterType.IsPointer))
            {
                builder.Append("unsafe ");
            }
            builder.Append(GetTypeFullName(method.ReturnParameter));
            builder.Append(' ');
            builder.Append(method.Name);
            builder.Append('(');
            builder.Append(string.Join(", ", method.GetParameters().Select(GetFullParameter)));
            builder.Append(')');
            return builder.ToString();
        }
    }
}
