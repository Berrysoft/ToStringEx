using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToStringEx.Reflection
{
    internal class VisualBasicHelper : ILanguageHelper
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

        private static string GetVTableString(MethodAttributes attr)
        {
            StringBuilder builder = new StringBuilder();
            if (attr.HasFlag(MethodAttributes.Abstract))
                builder.Append("MustOverride");
            else if (attr.HasFlag(MethodAttributes.Static))
                builder.Append("Shared");
            else if (attr.HasFlag(MethodAttributes.Virtual))
            {
                if (attr.HasFlag(MethodAttributes.Final))
                    builder.Append("NotOverridable ");
                if (attr.HasFlag(MethodAttributes.NewSlot))
                    builder.Append("Overridable");
                else
                    builder.Append("Overrides");
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
                            builder.Append("(Of ");
                            builder.AppendJoin(", ", et.GetGenericArguments().Select(tt => tt.IsGenericParameter ? genericTypes[tt.GenericParameterPosition] : GetTypeName(tt, genericTypes)));
                            builder.Append(')');
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
                builder.Append("()");
            else if (t.IsPointer)
            {
                if (et != typeof(void))
                    builder.Append(' ');
                builder.Append("Pointer");
            }
            return builder.ToString();
        }

        private static (string pre, string post) GetTypeFullName(ParameterInfo p, string[] genericTypes)
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
            if (p.IsOptional)
                preBuilder.Append("Optional ");
            if (p.GetCustomAttribute(typeof(ParamArrayAttribute)) != null)
                preBuilder.Append("ParamArray ");
            StringBuilder postBuilder = new StringBuilder();
            if (et != t || (et == t && et != typeof(void)))
                postBuilder.AppendFormat(" As {0}", GetTypeName(t, genericTypes));
            if (p.IsOptional)
                postBuilder.AppendFormat(" = {0}", p.DefaultValue);
            return (preBuilder.ToString(), postBuilder.ToString());
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
            var genericTypes = method.GetGenericArguments().Select(t => GetTypeName(t, null)).ToArray();
            var (pre, post) = GetTypeFullName(method.ReturnParameter, genericTypes);
            builder.Append(pre);
            builder.Append(method.ReturnType == typeof(void) ? "Sub" : "Function");
            builder.Append(' ');
            builder.Append(method.Name);
            if (genericTypes.Length > 0)
            {
                builder.Append("(Of ");
                builder.AppendJoin(", ", genericTypes);
                builder.Append(')');
            }
            builder.Append('(');
            var ps = method.GetParameters().Select(p =>
            {
                var (tpre, tpost) = GetTypeFullName(p, genericTypes);
                return $"{tpre}{p.Name}{tpost}";
            });
            builder.AppendJoin(", ", ps);
            builder.Append(')');
            builder.Append(post);
            return builder.ToString();
        }

        public string FormatType(Type type) => GetTypeName(type, null);
    }
}
