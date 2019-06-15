using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToStringEx
{
    internal static class VisualBasicMethodInfoFormatterHelper
    {
        private static readonly Dictionary<Type, string> PreDefinedTypes = new Dictionary<Type, string>
        {
            [typeof(bool)] = "Boolean",
            [typeof(char)] = "Char",
            [typeof(short)] = "Short",
            [typeof(ushort)] = "UShort",
            [typeof(int)] = "Integer",
            [typeof(uint)] = "UInteger",
            [typeof(long)] = "Long",
            [typeof(ulong)] = "ULong",
            [typeof(float)] = "Single",
            [typeof(double)] = "Double",
            [typeof(string)] = "String",
            [typeof(object)] = "Object",
            [typeof(DateTime)] = "Date"
        };

        private static readonly Dictionary<MethodAttributes, string> AccessStringMap = new Dictionary<MethodAttributes, string>
        {
            [MethodAttributes.Private] = "Private",
            [MethodAttributes.Public] = "Public",
            [MethodAttributes.Family] = "Protected",
            [MethodAttributes.Assembly] = "Friend",
            [MethodAttributes.FamORAssem] = "Protected Friend",
            [MethodAttributes.FamANDAssem] = "Private Protected"
        };

        private static (string pre, string post) GetTypeFullName(ParameterInfo p)
        {
            Type t = p.ParameterType;
            string pre = t.IsByRef ? "ByRef" : string.Empty;
            if (t.IsByRef) t = t.GetElementType();
            string post;
            if (PreDefinedTypes.TryGetValue(t, out string type))
            {
                post = $" As {type}";
            }
            else if (t == typeof(void))
            {
                post = string.Empty;
            }
            else
            {
                post = $" As {t.FullName}";
            }
            return (pre, post);
        }

        public static string FormatInternal(MethodInfo method)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(AccessStringMap[method.Attributes & MethodAttributes.MemberAccessMask]);
            builder.Append(' ');
            var (pre, post) = GetTypeFullName(method.ReturnParameter);
            builder.Append(pre);
            builder.Append(method.ReturnType == typeof(void) ? "Sub" : "Function");
            builder.Append(' ');
            builder.Append(method.Name);
            builder.Append('(');
            builder.Append(string.Join(", ", method.GetParameters().Select(p =>
            {
                var (tpre, tpost) = GetTypeFullName(p);
                return $"{tpre}{p.Name}{tpost}";
            })));
            builder.Append(')');
            builder.Append(post);
            return builder.ToString();
        }
    }
}
