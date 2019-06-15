using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;


#if NETSTANDARD2_0
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class IsReadOnlyAttribute : Attribute { }
}
#endif

namespace ToStringEx
{
    internal static class CSharpMethodInfoFormatterHelper
    {
        private static readonly Dictionary<Type, string> PreDefinedTypes = new Dictionary<Type, string>
        {
            [typeof(bool)] = "bool",
            [typeof(char)] = "char",
            [typeof(short)] = "short",
            [typeof(ushort)] = "ushort",
            [typeof(int)] = "int",
            [typeof(uint)] = "uint",
            [typeof(long)] = "long",
            [typeof(ulong)] = "ulong",
            [typeof(float)] = "float",
            [typeof(double)] = "double",
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

        private static string GetTypeFullName(ParameterInfo p)
        {
            Type t = p.ParameterType;
            StringBuilder builder = new StringBuilder();
            if (t.IsByRef)
            {
                if (p.IsOut)
                {
                    builder.Append("out");
                }
                else if (p.IsIn)
                {
                    builder.Append("in");
                }
                else
                {
                    builder.Append("ref");
                }
                builder.Append(' ');
                t = t.GetElementType();
            }
            if (PreDefinedTypes.TryGetValue(t, out string type))
            {
                builder.Append(type);
            }
            else
            {
                builder.Append(t.FullName);
            }
            return builder.ToString();
        }

        public static string FormatInternal(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(AccessStringMap[method.Attributes & MethodAttributes.MemberAccessMask]);
            builder.Append(' ');
            builder.Append(GetTypeFullName(method.ReturnParameter));
            builder.Append(' ');
            builder.Append(method.Name);
            builder.Append('(');
            builder.Append(string.Join(", ", method.GetParameters().Select(p => $"{GetTypeFullName(p)} {p.Name}")));
            builder.Append(')');
            return builder.ToString();
        }
    }
}
