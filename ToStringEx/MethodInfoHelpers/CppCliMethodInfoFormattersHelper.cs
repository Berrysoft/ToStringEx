using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToStringEx.MethodInfoHelpers
{
    internal static class CppCliMethodInfoFormattersHelper
    {
        private static readonly Dictionary<Type, string> PreDefinedTypes = new Dictionary<Type, string>
        {
            [typeof(bool)] = "bool",
            [typeof(short)] = "short",
            [typeof(ushort)] = "unsigned short",
            [typeof(int)] = "int",
            [typeof(uint)] = "unsigned int",
            [typeof(long)] = "long",
            [typeof(ulong)] = "unsigned long",
            [typeof(float)] = "float",
            [typeof(double)] = "double",
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
                    builder.Append("[OutAttribute] ");
                }
                else if (p.IsIn)
                {
                    builder.Append("[InAttribute] ");
                }
            }
            if (PreDefinedTypes.TryGetValue(et, out string type))
            {
                builder.Append(type);
            }
            else
            {
                builder.Append(et.FullName.Replace(".", "::"));
            }
            if (et.IsClass)
            {
                builder.Append('^');
            }
            if (t.IsByRef)
            {
                builder.Append('%');
            }
            else if (t.IsPointer)
            {
                builder.Append('*');
            }
            return builder.ToString();
        }

        public static string FormatInternal(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
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
