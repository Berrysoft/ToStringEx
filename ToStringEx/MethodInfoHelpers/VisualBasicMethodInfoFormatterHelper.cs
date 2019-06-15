using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToStringEx.MethodInfoHelpers
{
    internal static class VisualBasicMethodInfoFormatterHelper
    {
        private static readonly Dictionary<Type, string> PreDefinedTypes = new Dictionary<Type, string>
        {
            [typeof(bool)] = "Boolean",
            [typeof(byte)] = "Byte",
            [typeof(sbyte)] = "SByte",
            [typeof(char)] = "Char",
            [typeof(short)] = "Short",
            [typeof(ushort)] = "UShort",
            [typeof(int)] = "Integer",
            [typeof(uint)] = "UInteger",
            [typeof(long)] = "Long",
            [typeof(ulong)] = "ULong",
            [typeof(float)] = "Single",
            [typeof(double)] = "Double",
            [typeof(decimal)] = "Decimal",
            [typeof(string)] = "String",
            [typeof(object)] = "Object",
            [typeof(void)] = string.Empty,
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
                builder.Append("()");
            else if (t.IsPointer)
            {
                if (et != typeof(void))
                    builder.Append(' ');
                builder.Append("Pointer");
            }
            return builder.ToString();
        }

        private static (string pre, string post) GetTypeFullName(ParameterInfo p)
        {
            Type t = p.ParameterType;
            Type et = t.GetElementType() ?? t;
            StringBuilder preBuilder = new StringBuilder();
            if (t.IsByRef)
            {
                if (p.IsIn)
                    preBuilder.Append("<In> ");
                else if (p.IsOut)
                    preBuilder.Append("<Out> ");
                preBuilder.Append("ByRef ");
            }
            StringBuilder postBuilder = new StringBuilder();
            if (et != t || (et == t && et != typeof(void)))
                postBuilder.AppendFormat(" As {0}", GetTypeName(t));
            return (preBuilder.ToString(), postBuilder.ToString());
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
