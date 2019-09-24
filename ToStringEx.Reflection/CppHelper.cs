using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToStringEx.Reflection
{
    internal class CppHelper : ILanguageHelper
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

        private static string GetTypeName(Type t, string[] genericTypes)
        {
            Type et = t.GetElementType() ?? t;
            StringBuilder builder = new StringBuilder();
            if (t.IsArray)
            {
                builder.Append("cli::array<");
            }
            if (CppCliPreDefinedTypes.TryGetValue(et, out string type))
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
                    if (genericTypes != null && et.IsGenericParameter)
                    {
                        builder.Append(genericTypes[et.GenericParameterPosition]);
                    }
                    else if (et.IsGenericType)
                    {
                        builder.Append(et.Namespace);
                        builder.Append("::");
                        builder.Append(et.Name.Substring(0, et.Name.IndexOf('`'))).Replace("/", "::");
                        builder.Append('<');
                        builder.AppendJoin(", ", et.GetGenericArguments().Select(tt => genericTypes != null && tt.IsGenericParameter ? genericTypes[tt.GenericParameterPosition] : GetTypeName(tt, genericTypes)));
                        builder.Append('>');
                    }
                    else
                    {
                        builder.Append(et.FullName ?? et.Name).Replace("/", "::");
                    }
                }
                builder.Replace(".", "::");
            }
            if (t.IsArray)
                builder.Append('>');
            if (t.IsPointer)
            {
                builder.Append('*');
            }
            else if (t.IsClass && !t.IsByRef && !t.IsGenericParameter)
            {
                builder.Append('^');
            }
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
                    builder.Append("[Out] ");
                }
                else if (p.IsIn)
                {
                    builder.Append("[In] ");
                }
            }
            if (p.CustomAttributes.Any() && p.GetCustomAttribute(typeof(ParamArrayAttribute)) != null)
                builder.Append("... ");
            builder.Append(GetTypeName(t, genericTypes));
            if (t.IsByRef)
            {
                builder.Append('%');
            }
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
            var genericTypes = method.GetGenericArguments().Select(t => GetTypeName(t, null)).ToArray();
            if (method.IsGenericMethod)
            {
                builder.Append("generic ");
                builder.Append('<');
                builder.AppendJoin(", ", method.GetGenericArguments().Where(t => t.FullName == null).Select(t => $"typename {t.Name}"));
                builder.Append("> ");
            }
            bool virt = method.Attributes.HasFlag(MethodAttributes.Virtual);
            if (virt)
                builder.Append("virtual ");
            builder.Append(GetTypeFullName(method.ReturnParameter, genericTypes));
            builder.Append(' ');
            builder.Append(method.Name);
            if (method.IsGenericMethod)
            {
                var specializedGenericTypes = method.GetGenericArguments().Where(t => t.FullName != null).Select(t => GetTypeName(t, genericTypes)).ToArray();
                if (specializedGenericTypes.Length > 0)
                {
                    builder.Append('<');
                    builder.AppendJoin(", ", specializedGenericTypes);
                    builder.Append('>');
                }
            }
            builder.Append('(');
            var ps = method.GetParameters().Select(p => GetFullParameter(p, genericTypes));
            if (method.CallingConvention.HasFlag(CallingConventions.VarArgs))
                ps = ps.Append("...");
            builder.AppendJoin(", ", ps);
            builder.Append(')');
            if (virt)
            {
                if (method.Attributes.HasFlag(MethodAttributes.Abstract))
                    builder.Append(" abstract");
                else
                {
                    if (!method.Attributes.HasFlag(MethodAttributes.NewSlot))
                        builder.Append(" override");
                    if (method.Attributes.HasFlag(MethodAttributes.Final))
                        builder.Append(" final");
                }
            }
            return builder.ToString();
        }

        public string FormatType(Type type) => GetTypeName(type, null);
        public string FormatTypeInfo(TypeInfo type) => FormatType(type);

        public string FormatEventInfo(EventInfo eventInfo)
            => $"event {FormatType(eventInfo.EventHandlerType)} {eventInfo.Name}";
    }
}
