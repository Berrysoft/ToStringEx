using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToStringEx.MethodInfoHelpers
{
    internal static class CppMethodInfoFormatterHelper
    {
        private static readonly Dictionary<Type, string> CppCliPreDefinedTypes = new Dictionary<Type, string>
        {
            [typeof(bool)] = "bool",
            [typeof(byte)] = "unsigned char",
            [typeof(sbyte)] = "char",
            [typeof(char)] = "wchar_t",
            [typeof(short)] = "short",
            [typeof(ushort)] = "unsigned short",
            [typeof(int)] = "int",
            [typeof(uint)] = "unsigned int",
            [typeof(long)] = "long long",
            [typeof(ulong)] = "unsigned long long",
            [typeof(float)] = "float",
            [typeof(double)] = "double",
            [typeof(void)] = "void"
        };

        private static readonly Dictionary<Type, string> CppWinRTPreDefinedTypes = new Dictionary<Type, string>
        {
            [typeof(bool)] = "bool",
            [typeof(byte)] = "std::uint8_t",
            [typeof(sbyte)] = "std::int8_t",
            [typeof(char)] = "wchar_t",
            [typeof(short)] = "std::int16_t",
            [typeof(ushort)] = "std::uint16_t",
            [typeof(int)] = "std::int32_t",
            [typeof(uint)] = "std::uint32_t",
            [typeof(long)] = "std::int64_t",
            [typeof(ulong)] = "std::uint64_t",
            [typeof(float)] = "float",
            [typeof(double)] = "double",
            [typeof(string)] = "hstring",
            [typeof(object)] = "Windows::Foundation::IInspectable",
            [typeof(void)] = "void"
        };

        private static string GetTypeName(Type t, bool cli)
        {
            Type et = t.GetElementType() ?? t;
            StringBuilder builder = new StringBuilder();
            if (t.IsArray)
            {
                if (cli)
                    builder.Append("cli::array<");
                else
                    builder.Append("array_view<");
            }
            if ((cli ? CppCliPreDefinedTypes : CppWinRTPreDefinedTypes).TryGetValue(et, out string type))
            {
                builder.Append(type);
            }
            else
            {
                builder.Append(t == et ? et.FullName : GetTypeName(et, cli)).Replace(".", "::").Replace("/", "::");
            }
            if (t.IsArray)
                builder.Append('>');
            if (t.IsPointer)
            {
                builder.Append('*');
            }
            else if (cli && t.IsClass && !t.IsByRef)
            {
                builder.Append('^');
            }
            return builder.ToString();
        }

        private static string GetTypeFullName(ParameterInfo p, bool cli)
        {
            Type t = p.ParameterType;
            StringBuilder builder = new StringBuilder();
            if (cli && t.IsByRef)
            {
                if (p.IsOut)
                {
                    builder.Append("[Out] ");
                }
                else if (p.IsIn)
                {
                    builder.Append("[In] ");
                }
            }
            builder.Append(GetTypeName(t, cli));
            if (!cli && p.IsRetval && t.IsClass && !t.IsByRef && !t.IsPointer)
            {
                builder.Append(" const&");
            }
            if (t.IsByRef)
            {
                builder.Append(cli ? '%' : '&');
            }
            return builder.ToString();
        }

        public static string FormatInternal(MethodInfo method, bool cli)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetTypeFullName(method.ReturnParameter, cli));
            builder.Append(' ');
            builder.Append(method.Name);
            builder.Append('(');
            builder.Append(string.Join(", ", method.GetParameters().Select(p => $"{GetTypeFullName(p, cli)} {p.Name}")));
            builder.Append(')');
            return builder.ToString();
        }
    }
}
